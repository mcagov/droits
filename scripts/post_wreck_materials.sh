#!/bin/bash

API_ENDPOINT="$1/api/MigrateWreckMaterial"
DATA_FILE="./data/wreck_materials_data.json"

sh post_data.sh $API_ENDPOINT $DATA_FILE
