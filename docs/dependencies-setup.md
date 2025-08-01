
# Local Development Environment Setup Guide

This guide provides a simplified, step-by-step process for setting up your local environment with specific tool versions.

## Prerequisites

* **This guide assumes a MacOS installation. Please refer to your system for specific steps.**
* **Homebrew** installed
* **Docker Desktop** installed and running

### Setup Steps

Follow these steps in order:

#### 1. Configure `pyenv` for Python

We use `pyenv` to manage Python versions needed for Node.js builds.

* Install `pyenv`: `brew install pyenv`
* Configure `pyenv`: Add the following lines to your `~/.zshrc` file:

```bash
    export PYENV_ROOT="$HOME/.pyenv"
    command -v pyenv >/dev/null || export PATH="$PYENV_ROOT/bin:$PATH"
    eval "$(pyenv init -)"
```

* Apply changes: `source ~/.zshrc`

#### 2. Install Python for Node.js 14.x Build

* Install Python 3.9.x: `pyenv install 3.9.18`
* Set as global (temporarily): `pyenv global 3.9.18`

#### 3. Install `nvm` and Node.js 14.19.2

`nvm` is used to manage multiple Node.js versions.

* Install `nvm`: `brew install nvm`
* Configure `nvm`: Add these lines to `~/.zshrc` **before** the `pyenv` configurations:

```bash
    export NVM_DIR="$HOME/.nvm"
    [ -s "/opt/homebrew/opt/nvm/nvm.sh" ] && \. "/opt/homebrew/opt/nvm/nvm.sh"
    [ -s "/opt/homebrew/opt/nvm/etc/_completion.d/nvm" ] && \. "/opt/homebrew/opt/nvm/etc/_completion.d/nvm"
```

* Apply changes: `source ~/.zshrc`
* Install Node.js: `nvm install 14.19.2`
* Set as default: `nvm alias default 14.19.2`

#### 4. Install Other Dependencies

* **.NET 8 SDK**: `brew install dotnet-sdk@8`
* **Terraform 1.4.6**: `brew install terraform@1.4.6`

### Using Docker for Node.js (Alternative)

For older versions of Node.js, using Docker provides an isolated, pre-built environment.

#### Building and Running Your Application

1. **Create a `Dockerfile`** in your project root:

    ```dockerfile
        FROM node:14.19.2-slim
        WORKDIR /app
        COPY package*.json ./
        RUN npm install
        COPY . .
        EXPOSE 3000
        CMD ["npm", "start"]
    ```

2. **Build the image**: `docker build -t your-app-name:1.0 .`
3. **Run the container**: `docker run -p 3000:3000 -v "$(pwd)":/app your-app-name:1.0`

### Post-Installation

* **Reset Python**: After a successful Node.js installation, you can revert `pyenv` to your system's default Python:
    `pyenv global system`
