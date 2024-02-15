#!/bin/bash

# Read data from the JSON file
source ./config/set_secrets.sh

config_file="./config/config.json"
entities=$(jq -c '.entities[]' "$config_file")

powerapps_base="https://reportwreckmaterial.crm11.dynamics.com/api/data/v9.2"
dotnet_base="http://localhost:5000"

# Function to construct API endpoint
construct_endpoint() {
    base="$1"
    endpoint="$2"
    printf "%s%s" "$base" "$endpoint"
}

# Obtain the access token
powerapps_access_token=$(curl -s -X POST \
  -d "grant_type=client_credentials" \
  -d "client_id=$POWERAPPS_CLIENT_ID" \
  -d "client_secret=$POWERAPPS_CLIENT_SECRET" \
  -d "resource=https://reportwreckmaterial.crm11.dynamics.com/" \
  "https://login.microsoftonline.com/3fd408b5-82e6-4dc0-a36c-6e2aa815db3e/oauth2/token" | jq -r '.access_token')

rm data/*

# Loop through entities
while IFS= read -r entity_data; do
    entity_name=$(jq -r '.entity' <<< "$entity_data")
    dotnet_endpoint=$(jq -r '.dotnet_endpoint' <<< "$entity_data")
    powerapps_endpoint=$(jq -r '.powerapps_endpoint' <<< "$entity_data")

    # Construct the API endpoints
    powerapps_full_endpoint=$(construct_endpoint "$powerapps_base" "$powerapps_endpoint")
    dotnet_full_endpoint=$(construct_endpoint "$dotnet_base" "$dotnet_endpoint")

    # Construct the data file path
    data_file="./data/${entity_name}_data.json"

    # Pull data from PowerApps API
    sh pull_data.sh "$powerapps_full_endpoint" "$data_file" "$powerapps_access_token"

    # Post data to the new API
    sh push_data.sh "$dotnet_full_endpoint" "$data_file" "$DOTNET_API_KEY"

done <<< "$entities"
