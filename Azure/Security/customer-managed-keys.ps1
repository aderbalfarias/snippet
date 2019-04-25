#connect to Azure
Connect-AzureRmAccount

#To use customer-managed keys with SSE, you must assign a storage account identity to the storage account. 
Set-AzureRmStorageAccount -ResourceGroupName "rm-security" -Name "myaddressbookplus" -AssignIdentity

#enable Soft Delete and Do Not Purge by executing the following PowerShell commands
$vaultName = "AddressBookPlusVault03"
($resource = Get-AzureRmResource -ResourceId (Get-AzureRmKeyVault -VaultName $vaultName).ResourceId).Properties `
 | Add-Member -MemberType NoteProperty -Name enableSoftDelete -Value 'True'

Set-AzureRmResource -resourceid $resource.ResourceId -Properties $resource.Properties

($resource = Get-AzureRmResource -ResourceId (Get-AzureRmKeyVault -VaultName $vaultName).ResourceId).Properties `
| Add-Member -MemberType NoteProperty -Name enablePurgeProtection -Value 'True'

Set-AzureRmResource -resourceid $resource.ResourceId -Properties $resource.Properties

#custermanagedkey01

#associate the above key with an existing storage account using the following PowerShell commands
$storageAccount = Get-AzureRmStorageAccount -ResourceGroupName "rm-security" -AccountName "myaddressbookplus"

$keyVault = Get-AzureRmKeyVault -VaultName $vaultName

$key = Get-AzureKeyVaultKey -VaultName $keyVault.VaultName -Name "custermanagedkey01"

Set-AzureRmKeyVaultAccessPolicy -VaultName $keyVault.VaultName -ObjectId $storageAccount.Identity.PrincipalId `
-PermissionsToKeys wrapkey,unwrapkey,get

Set-AzureRmStorageAccount -ResourceGroupName $storageAccount.ResourceGroupName -AccountName $storageAccount.StorageAccountName `
-KeyvaultEncryption -KeyName $key.Name -KeyVersion $key.Version -KeyVaultUri $keyVault.VaultUri
