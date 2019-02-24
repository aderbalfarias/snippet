# Create a column master key in Azure Key Vault.
Login-AzureRmAccount

$subscriptionName = 'Pay-As-You-Go'
$userPrincipalName = 'aderbal@aderbal.com'
$applicationId = '386424df-c14a-4436-b872-f186ea2ddc98'
$resourceGroupName = 'Pluralsight'
$location = 'East US'
$vaultName = 'AwysEncKV'

$subscriptionId = (Get-AzureRmSubscription -SubscriptionName $subscriptionName).Id
Set-AzureRmContext -SubscriptionId $subscriptionId

New-AzureRmKeyVault -VaultName $vaultName -ResourceGroupName $resourceGroupName -Location $location

# grant key vault access to the user (user will be used to login to Azure in the SSMS wizard)
Set-AzureRmKeyVaultAccessPolicy -VaultName $vaultName -ResourceGroupName $resourceGroupName `
-PermissionsToKeys create,get,wrapKey,unwrapKey,sign,verify,list -UserPrincipalName $userPrincipalName

# grant key vault access to MyAddressBook+ application
Set-AzureRmKeyVaultAccessPolicy  -VaultName $vaultName  -ResourceGroupName $resourceGroupName `
-ServicePrincipalName $applicationId -PermissionsToKeys get,wrapKey,unwrapKey,sign,verify,list  