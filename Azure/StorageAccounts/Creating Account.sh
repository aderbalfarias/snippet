#az login

# Create a resource group.
az group create --name myResourceGroupAz203 --location westeurope

# Creating the storage account.
az storage account create \
    --resource-group myResourceGroupAz203 \
    --name storageAz203 \
    --location westeurope \
    --sku Standard_LRS