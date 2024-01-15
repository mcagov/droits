#!/bin/bash

if [ "$#" -ne 2 ]; then
    echo "Usage: $0 <API_ENDPOINT> <DATA_FILE>"
    exit 1
fi

API_ENDPOINT="$1"
DATA_FILE="$2"

DATA_ARRAY=$(jq -r '.value' "$DATA_FILE")

# Iterate through the objects and send POST requests
echo "$DATA_ARRAY" | jq -c '.[]' | while IFS= read -r item; do
     curl -X POST -H "Content-Type: application/json" -d "$item" "$API_ENDPOINT"
done
