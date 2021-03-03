#!/bin/sh

# Abort on any error (including if wait-for-it fails).
set -e

# Wait for the backend to be up, if we know where it is.
if [ -n "$WAIT_FOR_IT_URL" ]; then
  /opt/TestService.WebApi/wait-for-it.sh "$WAIT_FOR_IT_URL"
fi

# Run the main container command.
exec "$@"