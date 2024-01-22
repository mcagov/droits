#!/bin/bash

#INPUTS
ACCESS_TOKEN="$1"
OUTPUT_FILE="$2"
REQUEST_ENDPOINT="$3"


#echo "Access Token: $ACCESS_TOKEN"

api_base_endpoint="https://reportwreckmaterial.crm11.dynamics.com/api/data/v9.2/"

api_endpoint=$(printf "%s%s" "$api_base_endpoint" "$REQUEST_ENDPOINT")

method="GET"
headers="Authorization: Bearer $ACCESS_TOKEN"

# Make the API call using curl
response=$(curl -s -X $method -H "$headers" "$api_endpoint")

echo "$response" > "$OUTPUT_FILE"
