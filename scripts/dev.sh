#!/usr/bin/env bash
set -euo pipefail

VM_NAME="droits"
REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
LIMA_CONFIG="$REPO_ROOT/lima/droits.yaml"
SETUP_MARKER=".droits-setup-hash"
PORTS=(3000 5001 5432 6379 4566)
SETUP_INPUTS=(
  .tool-versions
  Makefile
  package-lock.json
  webapp/package-lock.json
  backoffice/src/package-lock.json
  backoffice/src/Droits.csproj
)
VM_LOCAL_DIRS=(
  node_modules
  webapp/node_modules
  webapp/dist
  backoffice/src/node_modules
  backoffice/src/wwwroot/dist
  backoffice/src/bin
  backoffice/src/obj
  backoffice/tests/bin
  backoffice/tests/obj
)

usage() {
  cat <<'EOF'
Runs Droits in a Lima VM, with local authentication and no Azure credentials.

  ./scripts/dev.sh          Start the VM if needed, then serve the app
  ./scripts/dev.sh setup    Re-run `make setup` inside the VM
  ./scripts/dev.sh shell    Open a shell in the VM, at the repository
  ./scripts/dev.sh down     Stop the VM
  ./scripts/dev.sh rebuild  Delete the VM and build it again from scratch
  ./scripts/dev.sh status   Show the VM and its containers
EOF
}

say() { printf "\n\033[1m==> %s\033[0m\n" "$1"; }
warn() { printf "\033[33m!! %s\033[0m\n" "$1" >&2; }
die() {
  printf "\033[31m!! %s\033[0m\n" "$1" >&2
  exit 1
}

if [ "${DROITS_WATCH_POLLING:-}" = "true" ]; then
  WATCH_ENV="CHOKIDAR_USEPOLLING=true WATCHPACK_POLLING=true DOTNET_USE_POLLING_FILE_WATCHER=true"
else
  WATCH_ENV=""
fi

in_vm() {
  limactl shell "$VM_NAME" -- bash -lc "cd '$REPO_ROOT' && DROITS_LOCAL_AUTH=true $WATCH_ENV $1"
}

vm_exists() { limactl list --quiet 2>/dev/null | grep -qx "$VM_NAME"; }
vm_running() { [ "$(limactl list "$VM_NAME" --format '{{.Status}}' 2>/dev/null || true)" = "Running" ]; }

require_lima() {
  if command -v limactl >/dev/null 2>&1; then return; fi

  say "Installing Lima"
  command -v brew >/dev/null 2>&1 ||
    die "Homebrew is needed to install Lima. See https://brew.sh, then run this again."
  brew install lima
}

create_vm() {
  say "Creating the '$VM_NAME' VM (this downloads Ubuntu and Podman, so the first run takes a while)"
  limactl start \
    --name="$VM_NAME" \
    --mount-only "$REPO_ROOT:w" \
    --tty=false \
    "$LIMA_CONFIG"
}

start_vm() {
  require_lima

  if ! vm_exists; then
    create_vm
    return
  fi

  if ! vm_running; then
    say "Starting the '$VM_NAME' VM"
    limactl start "$VM_NAME"
  fi

  require_podman_vm
}

require_podman_vm() {
  limactl shell "$VM_NAME" -- bash -lc "command -v podman" >/dev/null 2>&1 ||
    die "The '$VM_NAME' VM was created before Droits moved from Docker to Podman. Run ./scripts/dev.sh rebuild to replace it (this deletes the local database)."
}

mount_vm_local_dirs() {
  local rel store target
  for rel in "${VM_LOCAL_DIRS[@]}"; do
    target="$REPO_ROOT/$rel"
    store="/var/lib/droits-vm/${rel//\//_}"
    limactl shell "$VM_NAME" -- bash -lc "
      set -eu
      sudo mkdir -p '$store'
      sudo chown \"\$(id -u):\$(id -g)\" '$store'
      mkdir -p '$target'
      mountpoint -q '$target' || sudo mount --bind '$store' '$target'
    "
  done
}

setup_hash() {
  (cd "$REPO_ROOT" && cat "${SETUP_INPUTS[@]}" 2>/dev/null | shasum -a 256 | cut -d ' ' -f 1)
}

run_setup() {
  say "Installing .NET, Node and the dependencies in the VM (mise, then npm install)"
  in_vm "make setup"
  in_vm "echo '$(setup_hash)' > \"\$HOME/$SETUP_MARKER\""
}

setup_if_needed() {
  if in_vm "test \"\$(cat \"\$HOME/$SETUP_MARKER\" 2>/dev/null)\" = '$(setup_hash)'" >/dev/null 2>&1; then return; fi
  run_setup
}

warn_about_busy_ports() {
  local busy=()
  for port in "${PORTS[@]}"; do
    if lsof -nP -iTCP:"$port" -sTCP:LISTEN -Fc 2>/dev/null | grep '^c' | grep -qv '^climactl$'; then busy+=("$port"); fi
  done

  if [ ${#busy[@]} -gt 0 ]; then
    warn "Already in use on your Mac: ${busy[*]}. The VM cannot forward those ports until whatever is using them stops."
  fi
}

announce_when_ready() {
  local waited=0

  until curl -sf -o /dev/null --max-time 5 http://localhost:5001/healthz 2>/dev/null &&
    curl -sf -o /dev/null --max-time 5 http://localhost:3000/ 2>/dev/null; do
    sleep 5
    waited=$((waited + 5))
    if [ "$waited" = 60 ]; then
      say "Still starting. The first run restores the .NET packages and builds both apps, which takes a few minutes."
    fi
    [ "$waited" -ge 1200 ] && return
  done

  cat <<EOF

  --------------------------------------------------------------
  Droits is ready.

  Webapp      http://localhost:3000    portal sign in: dev@droits.local / password
  Backoffice  http://localhost:5001    signed in automatically as dev@droits.local
  LocalStack  http://localhost:4566    S3 bucket droits-local

  Edit files on your Mac as usual. Press Ctrl-C to stop.
  --------------------------------------------------------------

EOF
}

stop_vm_apps() {
  local apps="[m]ake serve|[d]otnet-watch|[d]otnet watch|[n]et8.0/Droits|[n]odemon|[w]ebpack --watch|[e]sm start.js"
  limactl shell "$VM_NAME" -- bash -c "
    pkill -INT -f '$apps' || exit 0
    for _ in 1 2 3 4 5; do pgrep -f '$apps' >/dev/null || exit 0; sleep 1; done
    pkill -KILL -f '$apps' || true
  "
}

serve() {
  say "Starting Droits with local authentication. The URLs appear once both apps are up."
  stop_vm_apps
  announce_when_ready &
  local announcer=$!
  trap "kill $announcer 2>/dev/null || true; stop_vm_apps" EXIT
  in_vm "make serve"
}

case "${1:-up}" in
up)
  start_vm
  mount_vm_local_dirs
  setup_if_needed
  warn_about_busy_ports
  serve
  ;;
setup)
  start_vm
  mount_vm_local_dirs
  run_setup
  ;;
shell)
  start_vm
  mount_vm_local_dirs
  say "Opening a shell in the VM. Run git here if your Mac has no .NET or Node."
  limactl shell --workdir "$REPO_ROOT" "$VM_NAME"
  ;;
down)
  vm_exists || die "There is no '$VM_NAME' VM."
  say "Stopping the '$VM_NAME' VM"
  limactl stop "$VM_NAME"
  ;;
rebuild)
  require_lima
  if vm_exists; then
    say "Deleting the '$VM_NAME' VM"
    limactl delete --force --tty=false "$VM_NAME"
  fi
  create_vm
  mount_vm_local_dirs
  run_setup
  ;;
status)
  require_lima
  limactl list "$VM_NAME" || true
  vm_running && in_vm "podman compose --profile local-auth ps" || true
  ;;
*)
  usage
  exit 1
  ;;
esac
