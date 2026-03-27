TODO:

- Move all this to Makefile
- Just make the Dockerfile if we need it and commit it to the repository
- Update the local development instructions in the main README
- Delete this file.

# Local Development Environment Setup Guide

This guide provides a simplified, step-by-step process for setting up your local environment with specific tool versions.

## Prerequisites

* **This guide assumes a MacOS installation. Please refer to your system for specific steps.**
* **Homebrew** installed
* **Docker Desktop** installed and running

### Setup Steps

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
