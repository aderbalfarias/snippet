#!/bin/sh
az cdn profile delete \
  --name psazurestoragecdn \
  --resource-group pluralsight-azure-storage-cdn | jq