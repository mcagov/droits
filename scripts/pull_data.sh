#!/bin/bash

# Function to process notes
processNotes() {
    local temp_file="$1"
    local ACCESS_TOKEN="$2"
    local OUTPUT_FILE="$3"

    # Replace "/api/data/" with the full URL
    local modified_output=$(sed 's|/api/data/|https://reportwreckmaterial.crm11.dynamics.com/api/data/|g' "$temp_file")

    # Extract and replace image URLs with their base64-encoded content
    while read -r image_url; do
        if [ -z "$image_url" ]; then
            echo "No image URLs found. Skipping getting image data..."
            break
        fi
        echo "Processing image URL: $image_url"
        local temp_image_file=$(mktemp)
        # Retrieve the image and save it to a temporary file
        local response=$(curl -s -H "Authorization: Bearer $ACCESS_TOKEN" -o "$temp_image_file" "$image_url")
        if [[ $? -eq 0 ]]; then
            # Convert the image content to base64
            local new_src="data:image/png;base64,$(base64 < "$temp_image_file")"
            # Replace the image URL with the base64-encoded content
            local sed_script_file=$(mktemp)
            echo "s|$image_url|$new_src|g" > "$sed_script_file"
            # Use the sed script file for replacement
            modified_output=$(sed -f "$sed_script_file" <<< "$modified_output")
            # Clean up: Remove temporary files
            rm "$temp_image_file" "$sed_script_file"
        else
            echo "Error retrieving image from $image_url"
        fi
    done <<< "$(echo "$modified_output" | grep -oE 'https:\/\/reportwreckmaterial.crm11.dynamics.com\/api\/data\/v9\.0\/msdyn_richtextfiles([^"]*)\/msdyn_imageblob\/\$value\?size=full' | sed -e 's/src=\?\"//' -e 's/\\?\"$//')"

    DATA=$(sed -e 's/\\/\\\\/g' -e 's/\\"/\"/g' <<< "$modified_output" | jq '. | walk(if type == "string" then gsub("[\u0000-\u001F]"; "\\n") | gsub("[\u0000-\u001F]"; "\"") else . end)')
    echo "$DATA" | jq '.' > "$OUTPUT_FILE"

    echo "Annotation data processed and saved to $OUTPUT_FILE"
}


# INPUTS
POWERAPPS_ENDPOINT="$1"
OUTPUT_FILE="$2"
ACCESS_TOKEN="$3"

# Make GET request to PowerApps endpoint and save response to a temporary file
temp_file=$(mktemp)

# Retrieve data from PowerApps endpoint and save it to the temporary file
curl_output=$(curl -s -X GET -H "Authorization: Bearer $ACCESS_TOKEN" "$POWERAPPS_ENDPOINT" -o "$temp_file")

# Check if the request was successful (HTTP status code 2xx)
if [[ $? -ne 0 ]]; then
    echo "Error: Failed to retrieve data from PowerApps endpoint."
    exit 1
fi

# Check if POWERAPPS_ENDPOINT starts with "/annotations"
if [[ "$POWERAPPS_ENDPOINT" == *"/annotations"* ]]; then
    echo "Processing annotation data..."
    # Call the function to process notes
    processNotes "$temp_file" "$ACCESS_TOKEN" "$OUTPUT_FILE"
else
    # Output regular JSON data to the output file
    cat "$temp_file" | jq '.' | sed -e 's/\\/\\\\/g' -e 's/\\"/\"/g' -e 's/’/'\''/g' > "$OUTPUT_FILE"
    echo "Regular JSON data saved to $OUTPUT_FILE"
fi

# Clean up: Remove temporary file
rm "$temp_file"

