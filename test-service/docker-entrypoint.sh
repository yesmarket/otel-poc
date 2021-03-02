#!/bin/sh

# Abort on any error (including if wait-for-it fails).
set -e

printf "test"
echo "test"
printf $WAIT_FOR_IT_URL
echo $WAIT_FOR_IT_URL
printf "$WAIT_FOR_IT_URL"
echo "$WAIT_FOR_IT_URL"

# Wait for the backend to be up, if we know where it is.
if [ -n "$WAIT_FOR_IT_URL" ]; then
  /usr/src/app/wait-for-it.sh "$WAIT_FOR_IT_URL"
fi

# Run the main container command.
exec "dotnet Galaxy.Entitlement.WebApi.dll"