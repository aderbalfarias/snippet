Connect-AzureRmAccount

# get the key vault properties
Get-AzureRmKeyVault -VaultName "AddressBookPlusVault03"

# enable soft delete on a key vault
($resource = Get-AzureRmResource -ResourceId (Get-AzureRmKeyVault -VaultName "AddressBookPlusVault03").ResourceId).Properties | Add-Member -MemberType "NoteProperty" -Name "enableSoftDelete" -Value "true"
Set-AzureRmResource -resourceid $resource.ResourceId -Properties $resource.Properties

# remove a key vault
Remove-AzureRmKeyVault -VaultName "AddressBookPlusVault03" -ResourceGroupName "Pluralsight"

# recover a "soft deleted vault"
Undo-AzureRmKeyVaultRemoval -VaultName "AddressBookPlusVault03" -ResourceGroupName "Pluralsight" -Location "East US"

#enable "Do Not Purge" on a key vault
($resource = Get-AzureRmResource -ResourceId (Get-AzureRmKeyVault -VaultName �AddressBookPlusVault03").ResourceId).Properties | Add-Member -MemberType NoteProperty -Name enablePurgeProtection  -Value "true"
Set-AzureRmResource -ResourceId $resource.ResourceId -Properties $resource.Properties

# permanantly delete a "soft deleted" key vault - does not work if "Do Not Purge" is enabled
Remove-AzureRmKeyVault -VaultName "AddressBookPlusVault03" -InRemovedState -Location "East US"