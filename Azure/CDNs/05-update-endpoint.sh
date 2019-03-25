#!/bin/sh
az cdn endpoint update \
  --name psazurestoragecdn \
  --profile-name psazurestoragecdn \
  --resource-group pluralsight-azure-storage-cdn \
  --origin-host-header 'psazurestoragecdn.blob.core.windows.net' | jq