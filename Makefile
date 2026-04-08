###
# Makefile for local development - Start developing Beacons with just one command
#
# $ make
#
###

MAKEFLAGS += --jobs

.PHONY: setup
setup: setup-root setup-backoffice setup-webapp
	asdf plugin add nodejs

.PHONY: setup-root
setup-root:
	@echo "\n==================================================="
	@echo "Installing root level dependencies and commit hooks\n"
	cd . && \
		asdf install && \
		node --version && \
		npm install && \
		npm run prepare

.PHONY: setup-backoffice
setup-backoffice:
	@echo "\n=================================="
	@echo "Installing backoffice dependencies\n"
	cd ./backoffice/src && \
		asdf install && \
		node --version && \
		npm install

.PHONY: setup-webapp
setup-webapp:
	@echo "\n=============================="
	@echo "Installing webapp dependencies\n"
	cd ./webapp && \
		asdf install && \
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

# We are using --jobs so that all jobs run in parallel
# Without this flag, the first job hogs the terminal and the others don't run
MAKEFLAGS += --jobs

.PHONY: serve
serve:
	@echo "\n==================================================="
	@echo "Installing root level dependencies and commit hooks\n"
	cd . && \
		docker compose up

##
# Utils
##
.PHONY: clean
clean:
	docker compose down
