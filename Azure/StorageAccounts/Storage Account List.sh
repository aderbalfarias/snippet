Interactive login: az login
Go to https://microsoft.com/devicelogin and enter code

az account list-locations --query [*].[displayName,name] --out table

az storage account check-name --name stgacctnm2

az storage account create --name stgacctnm2 --resource-group stgacctnmRG --location eastus --sku Standard_LRS --kind StorageV2 
az storage account show -g stgacctnmRG -n stgacctnm2

az storage account list -g stgacctnmRG

az storage account list -g stgacctnmRG --query [*].{Name:name,Location:primaryLocation,Sku:sku.name,Kind:kind} --out table

az storage account update --name stracctnm2 --resource-group stgacctnmRG --tags "environment=dev"

az storage account list -g stgacctnmRG --query [*].{Name:name,Location:primaryLocation,Sku:sku.name,Kind:kind,Tags:tags.environment} --out table

az resource list --tag environment=dev
