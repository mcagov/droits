#!/bin/bash


#Sets POWERAPPS_CLIENT_ID & POWERAPPS_CLIENT_SECRET
source data/set_secrets.sh


# Obtain the access token
access_token=$(curl -s -X POST \
  -d "grant_type=client_credentials" \
  -d "client_id=$POWERAPPS_CLIENT_ID" \
  -d "client_secret=$POWERAPPS_CLIENT_SECRET" \
  -d "resource=https://reportwreckmaterial.crm11.dynamics.com/" \
  "https://login.microsoftonline.com/3fd408b5-82e6-4dc0-a36c-6e2aa815db3e/oauth2/token" | jq -r '.access_token')

