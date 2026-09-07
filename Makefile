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

.PHONY: build
build: build-backoffice build-webapp

.PHONY: build-backoffice
build-backoffice:
	@echo "\n================================"
	@echo "Build backoffice container image\n"
	cd . && \
		docker compose build backoffice

.PHONY: build-webapp
build-webapp:
	@echo "\n============================"
	@echo "Build webapp container image\n"
	cd . && \
		docker compose build webapp

.PHONY: serve
serve:
	@echo "\n==========================================="
	@echo "Spinning up the service with Docker Compose\n"
	cd . && \
		docker compose up

##
# Utils
##
.PHONY: clean
clean:
	docker compose down
