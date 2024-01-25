#!/bin/bash

# INPUTS
POWERAPPS_ENDPOINT="$1"
OUTPUT_FILE="$2"
ACCESS_TOKEN="$3"

echo "SENDING " "$OUTPUT_FILE"
# Make GET request to PowerApps endpoint and save response to a temporary file
temp_file=$(mktemp)
curl -s -X GET -H "Authorization: Bearer $ACCESS_TOKEN" "$POWERAPPS_ENDPOINT" > "$temp_file"


# Check if the request was successful (HTTP status code 2xx)
if [[ $? -eq 0 ]]; then
    cat "$temp_file" | jq '.' > "$OUTPUT_FILE"
    echo "Data retrieved successfully and saved to $OUTPUT_FILE"
else
    echo "Error retrieving data from PowerApps endpoint."
    exit 1
fi


# Clean up: Remove temporary file
rm "$temp_file"
