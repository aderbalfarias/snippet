Connect-AzureRmAccount

# creating a new Azure Key Vault

New-AzureRmKeyVault -VaultName 'AddressBookPlusVault03' -ResourceGroupName 'rm-security' -Location 'northeurope'

# convert our secret value to a "secure string"
$secretvalue = ConvertTo-SecureString String 'myaddressbookplus.redis.cache.windows.net:6380,password=hQwiwqd+jij2nZZHzyW5AtawOTq71P4DkNn3n5BFPrw=,ssl=True,abortConnect=False' -AsPlainText -Force

# add the secure string to our new Key Vault
$secret = Set-AzureKeyVaultSecret -VaultName 'AddressBookPlusVault03' -Name 'CacheConnection' -SecretValue $secretvalue

$secret.Id

Set-AzureRmKeyVaultAccessPolicy -VaultName 'AddressBookPlusVault03' -ServicePrincipalName 386424df-c14a-4436-b872-f186ea2ddc98 -PermissionsToSecrets Get

