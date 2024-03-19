#!/bin/bash

# Define variables
CONTEXT_NAME="Droits.Data.DroitsContext"
MIGRATIONS_PATH="Data/Migrations"

# Ensure dotnet ef tool is installed
if ! command -v dotnet ef &> /dev/null; then
    echo "Error: dotnet ef tool not found. Make sure you have installed the Entity Framework Core tools."
    exit 1
fi

# Check if arguments are provided
if [ $# -eq 0 ]; then
    # Prompt user for migration name
    read -p "Enter the name of the migration: " MIGRATION_NAME
else
    # Get migration name from command line arguments
    MIGRATION_NAME="$1"
fi

# Create migration
dotnet ef migrations add "$MIGRATION_NAME" --context "$CONTEXT_NAME" --output-dir "$MIGRATIONS_PATH"

