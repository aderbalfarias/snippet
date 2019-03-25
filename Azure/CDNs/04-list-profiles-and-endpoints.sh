#!/bin/sh
az cdn profile list
az cdn endpoint list \
  --profile-name psazurestoragecdn \
  --resource-group pluralsight-azure-storage-cdn | jq