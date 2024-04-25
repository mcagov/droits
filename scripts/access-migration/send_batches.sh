#!/bin/bash
source ../config/set_secrets.sh

API_BASE_URL="http://localhost:5000"
ENDPOINT_PATH="/Migration/ProcessAccessFileApi"

API_ENDPOINT="$API_BASE_URL$ENDPOINT_PATH"
RESULTS_DIR="results"

# Remove existing results directory if it exists
if [ -d "$RESULTS_DIR" ]; then
    rm -r "$RESULTS_DIR"
fi

# Create new results directory
mkdir -p "$RESULTS_DIR"

# Loop through each batch file
for file in ./data/batch_*.csv; do
    echo "Processing $file..."

    # Post the batch file to the API endpoint and save the response to a JSON file
    response=$(curl -s -X POST \
                    -H "Content-Type: multipart/form-data" \
                    -H "X-API-Key: $DOTNET_API_KEY" \
                    -F "file=@$file" \
                    $API_ENDPOINT)

    # Check if the response is empty or contains an error message
    if [[ -z "$response" ]]; then
        echo "Error: Empty response received for $file."
        continue
    fi

    # Extract the batch number from the file name
    batch_number=$(echo $file | cut -d'_' -f2 | cut -d'.' -f1)

    # Save the API response to a JSON file
    echo $response > "$RESULTS_DIR/batch_$batch_number.json"

    echo "Response from API saved to $RESULTS_DIR/batch_$batch_number.json"
    echo "--------------------------------------"
done

echo "Batch files processed."
