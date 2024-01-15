#!/bin/bash

API_ENDPOINT="$1/api/MigrateDroit"
DATA_FILE="./data/droits_data.json"

sh post_data.sh $API_ENDPOINT $DATA_FILE
