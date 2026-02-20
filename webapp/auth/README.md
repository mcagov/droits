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

2. **Integration with Azure B2C Policies**: Modify your Azure B2C user flow policies to reference these custom pages. Follow these steps to link your custom HTML to the sign-up/sign-in experience or update the `page UI customization` section in your policy XML file to include the URLs of these custom pages.
    - Log in to the Azure Portal.
    - Search for and select **Azure AD B2C**.

3. **Select User Flow:**
    - In the left-hand menu, select **User flows**.
    - Click on the **B2C_1_login** user flow.

4. **Enable Custom Page Content:**
    - In the left sidebar of the User Flow, select **Page layouts**.
    - Locate the section labeled **Unified sign-up or sign-in page**.
    - For the setting **Use custom page content**, select **Yes**.

5. **Link the URI:**
    - In the **Custom page URI** field, enter the full URL for your `login.html` file.
    - _Note: Ensure the URI is accessible via HTTPS._

6. **Apply Changes:**
    - Select **Save** at the top of the page.
    -

## Testing the Interface

Perform a manual test to ensure the CSS and HTML are rendering correctly.

1.  **Open the Run Context:**
    - Navigate back to **User flows** (or stay on the current blade).
    - Select the **B2C_1_login** user flow.
    - Click **Run user flow** at the top of the page.

2.  **Execute the Test:**
    - A pane will appear on the right side of the screen.
    - Select the **Run user flow** button.

3.  **Verify Results:**
    - A new browser tab or window will open.
    - **Success Criteria:** You should see the sign-in page with elements centered (or styled) based on the CSS file linked in your HTML template.

## Updating assets

- Create a new directory ensure it has a sensible name for example **assets-2026-16-02** and **portal-2026-16-02**.
- Once changes are tested and validated, clean up the old files in S3 after release.

## Troubleshooting

If the page loads the default Azure B2C look rather than your custom look:

- **Check CORS:** Ensure Cross-Origin Resource Sharing is enabled on the storage account hosting your HTML. Azure B2C requires permission to pull the content.
- **Check HTTPS:** Azure B2C only accepts custom URIs that use HTTPS.
- **Browser Cache:** If you made changes to the CSS file, run the user flow in an **Incognito/Private** window to ensure you aren't viewing a cached version.
- More information available [here](https://learn.microsoft.com/en-us/azure/active-directory-b2c/customize-ui-with-html?pivots=b2c-user-flow)

