#!/bin/bash

API_ENDPOINT="$1/api/MigrateWreck"
DATA_FILE="./data/wrecks_data.json"

sh post_data.sh $API_ENDPOINT $DATA_FILE
