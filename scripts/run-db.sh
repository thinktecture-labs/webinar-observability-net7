#!/bin/sh

# Exit on error
set -e

docker run -e 'ACCEPT_EULA=1' \
    -e 'MSSQL_SA_PASSWORD=yourStrong(!)Password' \
    -p 1433:1433 \
    --name azuresqledge -d \
    mcr.microsoft.com/azure-sql-edge
