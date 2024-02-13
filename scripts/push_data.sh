#!/bin/bash

if [ "$#" -ne 3 ]; then
    echo "Usage: $0 <API_ENDPOINT> <DATA_FILE> <API_KEY>"
    exit 1
fi



API_ENDPOINT="$1"
DATA_FILE="$2"
DOTNET_API_KEY="$3"

echo "SENDING " "$DATA_FILE"


# Validate if jq is available
if ! command -v jq > /dev/null; then
    echo "jq is not installed. Please install jq to process JSON data."
    exit 1
fi

# Read the value array from the data file
DATA_ARRAY=$(jq -r '.value | walk(if type == "string" then gsub("[\u0000-\u001F]"; " ") | gsub("\\\\"; "\\\\") else . end)' "$DATA_FILE")

# Check if the array is not empty
if [ -z "$DATA_ARRAY" ]; then
    echo "No data found in the specified file."
    exit 1
fi

# Iterate through the objects and send POST requests
echo "$DATA_ARRAY" | jq -c '.[]' | while IFS= read -r item; do
    # Check if the item is not empty
    if [ -n "$item" ]; then
    #     # Make POST request to API endpoint
        echo "$item" >> ./data/latest_item.json
        curl -X "POST" -H "Content-Type: application/json" -H "X-API-Key: $DOTNET_API_KEY" -d "$item" "$API_ENDPOINT" >> $DATA_FILE.output
    fi
done
