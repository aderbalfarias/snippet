#az login

# Create a resource group.
az group create --name rg-sa --location northeurope

# Creating the storage account.
az storage account create \
    --resource-group rg-sa \
    --name storageAz203 \
    --location northeurope \
    --sku Standard_LRS