# Migration Script README

This script is designed to facilitate the migration of data from a Microsoft PowerApps environment to a .NET Core application. It pulls data from specified endpoints in the PowerApps environment and pushes it to corresponding endpoints in the .NET Core application.

## Prerequisites

Before running the migration script, ensure the following:

1. **Environment Setup**:
    - The script assumes you have access to both the Microsoft PowerApps environment and the .NET Core application environment.
    - Make sure the .NET Core application is running and accessible at the specified base URL (`http://localhost:5000` by default).

2. **Access Tokens and API Keys**:
    - Obtain the necessary access tokens and API keys required for authentication in both environments.
    - These credentials should be securely stored and accessible during script execution.

3. **Configuration File**:
    - Ensure that the `config.json` file is properly configured with the entities you want to migrate, along with their corresponding endpoints in both environments.

4. **Dependencies**:
    - The script utilizes `jq` for JSON processing. Make sure `jq` is installed on your system.

5. **Secrets Configuration**:
    - Create a `set_secrets.sh` script in the `config/` directory and populate it with necessary secrets like `POWERAPPS_CLIENT_ID`, `POWERAPPS_CLIENT_SECRET`, and `DOTNET_API_KEY`. Retrieve these secrets from 1Password.

## Usage

To execute the migration script, follow these steps:

1. **Set up Secrets**:
    - Create the `set_secrets.sh` script in the `config/` directory and populate it with necessary secrets.
    - Pull down the `set_secrets.sh` script from your secure vault (1Password) to ensure sensitive information remains protected.

2. **Configure Entities**:
    - Edit the `config.json` file to specify the entities you want to migrate along with their endpoints in both environments.

3. **Run the Migration Script**:
    - Execute the `migrate_entities.sh` script.
    - The script will pull data from PowerApps endpoints specified in the configuration file and push it to corresponding endpoints in the .NET Core application.

4. **Review Output**:
    - Check the output of the migration script for any errors or warnings.
    - Ensure that data is successfully migrated to the .NET Core application.

## Additional Information

### Scripts Overview:

- **migrate_entities.sh**:
  - Orchestrates the migration process by reading configuration data from a JSON file and executing the migration steps for each entity specified in the configuration.

- **pull_data.sh**:
  - Responsible for pulling data from PowerApps endpoints. It makes HTTP GET requests to retrieve data and processes the responses. Handles special processing for notes and documents.

- **push_data.sh**:
  - Pushes data to the .NET Core API endpoints. Reads data from a specified file and sends HTTP POST requests to the API endpoints, incorporating the data into the requests.

- For more detailed information about each script and its functionalities, refer to the script comments and documentation within the script files.

- Make sure to handle any errors or exceptions encountered during the migration process appropriately to ensure data integrity and consistency.

- Regularly review and update the configuration file (`config.json`) as per your evolving migration requirements.
