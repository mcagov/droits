#!/bin/bash

API_ENDPOINT="$1/api/MigrateNote"
DATA_FILE="./data/notes_data.json"

sh post_data.sh $API_ENDPOINT $DATA_FILE
