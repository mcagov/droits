-include .env

ifeq ($(DROITS_LOCAL_AUTH),true)
include local-auth.env
-include .env
export $(shell sed -n 's/^\([A-Za-z_][A-Za-z0-9_]*\)=.*/\1/p' local-auth.env $(wildcard .env))
endif

export BUILDAH_FORMAT := docker

.PHONY: setup
setup: setup-root setup-backoffice setup-webapp

.PHONY: setup-root
setup-root:
	@echo "\n==================================================="
	@echo "Installing root level dependencies and commit hooks\n"
	cd . && \
		mise install && \
		node --version && \
		npm install && \
		npm run prepare

.PHONY: setup-backoffice
setup-backoffice:
	@echo "\n=================================="
	@echo "Installing backoffice dependencies\n"
	cd ./backoffice/src && \
		mise install && \
		node --version && \
		npm install

.PHONY: setup-webapp
setup-webapp:
	@echo "\n=============================="
	@echo "Installing webapp dependencies\n"
	cd ./webapp && \
		mise install && \
		node --version && \
		npm install

.PHONY: developer-certificate
developer-certificate:
	@echo "\n==============================="
	@echo "Creating developer certificate\n"
	dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password

##
# Applications
##

.PHONY: build
build: build-backoffice build-webapp

.PHONY: build-backoffice
build-backoffice:
	@echo "\n================================"
	@echo "Build backoffice container image\n"
	cd . && \
		podman compose build backoffice

.PHONY: build-webapp
build-webapp:
	@echo "\n============================"
	@echo "Build webapp container image\n"
	cd . && \
		podman compose build webapp

ifeq ($(DROITS_LOCAL_AUTH),true)
.PHONY: serve
serve: serve-backing-services
	@$(MAKE) --no-print-directory -j 2 serve-backoffice serve-webapp
else
.PHONY: serve
serve:
	@echo "\n==========================================="
	@echo "Spinning up the service with Podman Compose\n"
	cd . && \
		podman compose up
endif

.PHONY: serve-backing-services
serve-backing-services:
	@echo "\n=============================================="
	@echo "Starting Postgres, Redis and LocalStack in Podman\n"
	podman compose --profile local-auth up --detach database redis localstack
	podman wait --condition=healthy droits_database droits_localstack > /dev/null

.PHONY: serve-backoffice
serve-backoffice:
	@echo "\n======================================"
	@echo "Starting the backoffice with dotnet watch\n"
	cd ./backoffice/src && \
		DOTNET_WATCH_RESTART_ON_RUDE_EDIT=true mise exec -- dotnet watch run --no-launch-profile

.PHONY: serve-webapp
serve-webapp:
	@echo "\n================================"
	@echo "Starting the webapp in watch mode\n"
	cd ./webapp && \
		(test -f .env.json || echo '{}' > .env.json) && \
		mise exec -- npm run dev

##
# Utils
##
.PHONY: clean
clean:
	podman compose --profile local-auth down
