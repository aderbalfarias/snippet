#!/bin/sh

# ! Ensure you've set AZURE_STORAGE_KEY and AZURE_STORAGE_ACCOUNT env variables !

# Then generate a Blob container SAS,
# passing in container name and access policy name
az storage container generate-sas \
    --name staticsite \
    --policy-name cdn_v1 \

# You can then use the generated SAS and set env
# var AZURE_STORAGE_SAS_TOKEN to use in other az storage
# commands automatically!