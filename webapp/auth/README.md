# Azure B2C Custom Pages Repository

## Overview

This repository contains a copy of the files stored in the Azure Storage Account `reportwreckmaterial`. These files are specifically designed for custom pages used in Azure B2C user flows.

## Directory Structure

### Assets

- **css**: Contains custom stylesheets for the custom pages.
- **fonts**: Includes any custom fonts used in the pages.
- **images**: Houses images used in the custom pages.

### Portal

- **login.html**: The custom login page.
- **password-reset.html**: The custom password reset page.
- **signup.html**: The custom signup page.

## Usage

To use these custom pages in your Azure B2C user flows, you can directly upload these files to your Azure Storage Account (`reportwreckmaterial`) using Azure Portal, Azure Storage Explorer, or Azure CLI.

1. **Direct Upload to Azure Storage Account**: Upload these files to your Azure Storage Account (`reportwreckmaterial`) using Azure Portal, Azure Storage Explorer, or Azure CLI.

2. **Integration with Azure B2C Policies**: Modify your Azure B2C user flow policies to reference these custom pages. Update the `page UI customization` section in your policy XML file to include the URLs of these custom pages.
