az storage account network-rule list --resource-group "rg-sa" --account-name "exampleorgstgacct02" --query virtualNetworkRules

az network vnet subnet update --resource-group "rg-sa" --vnet-name "myvnet" --name "mysubnet" --service-endpoints "Microsoft.Storage"

subnetid=$(az network vnet subnet show --resource-group "rg-sa" --vnet-name "exorgvnet01" --name "subnet01" --query id --output tsv)
az storage account network-rule add --resource-group "rg-sa" --account-name "exampleorgstgacct02" --subnet $subnetid

subnetid=$(az network vnet subnet show --resource-group "rg-sa" --vnet-name "exorgvnet01" --name "subnet01" --query id --output tsv)
az storage account network-rule remove --resource-group "rg-sa" --account-name "exampleorgstgacct02" --subnet $subnetid

az storage account update --name "mystorageaccount" --resource-group "myresourcegroup" --default-action Allow
