#Azure Active Directory PowerShell module.
Install-Module AzureAD

#Verify the installed versions of the modules.
Get-Module AzureRM -ListAvailable | Select-Object -Property Name,Version,Path
Get-Module AzureAD -ListAvailable | Select-Object -Property Name,Version,Path

#connect to Azure
Connect-AzureRmAccount

#Enable the Azure Key Vault provider within your Azure subscription
$rgName = "DEDemoResourceGroup"
$location = "East US"
New-AzureRmResourceGroup -Location "East US" -Name $rgName

Register-AzureRmResourceProvider -ProviderNamespace "Microsoft.KeyVault"

#create a new Key Vault
$keyVaultName = "myDEKeyVaultName"
New-AzureRmKeyVault -Location $location `
    -ResourceGroupName $rgName `
    -VaultName $keyVaultName `
    -EnabledForDiskEncryption

#Create a cryptographic key
Add-AzureKeyVaultKey -VaultName $keyVaultName `
    -Name "myDEKey" `
    -Destination "Software"

#When virtual disks are encrypted or decrypted, you specify an account to handle the authentication and exchanging of 
# cryptographic keys from Key Vault. This account, an Azure Active Directory service principal, allows the Azure platform
# to request the appropriate cryptographic keys on behalf of the VM. A default Azure Active Directory instance is available
# in your subscription, though many organizations have dedicated Azure Active Directory directories.
$appName = "My DE Demo App"
$securePassword = ConvertTo-SecureString -String "P@ssw0rd!" -AsPlainText -Force
$app = New-AzureRmADApplication -DisplayName $appName `
    -HomePage "https://myaddressbookplus.azurewebsites.net" `
    -IdentifierUris "https://myaddressbookplus.azurewebsites.net/contact" `
    -Password $securePassword
New-AzureRmADServicePrincipal -ApplicationId $app.ApplicationId

#o successfully encrypt or decrypt virtual disks, permissions on the cryptographic key stored in Key Vault must be set 
# to permit the Azure Active Directory service principal to read the keys.
Set-AzureRmKeyVaultAccessPolicy -VaultName $keyvaultName `
    -ServicePrincipalName $app.ApplicationId `
    -PermissionsToKeys "WrapKey" `
    -PermissionsToSecrets "Set"

#To test the encryption process, create a VM
$cred = Get-Credential

$vmName = "myDEDemoVM"   # aderbaladmin , Sssqloledb65!

<#
New-AzureRmVm `
    -ResourceGroupName $rgName `
    -Name $vmName `
    -Location $location `
    -VirtualNetworkName "myVnet" `
    -SubnetName "mySubnet" `
    -SecurityGroupName "myNetworkSecurityGroup" `
    -PublicIpAddressName "myPublicIpAddress" `
    -Credential $cred
#>

#To encrypt the virtual disks, you bring together all the previous components:
$keyVault = Get-AzureRmKeyVault -VaultName $keyVaultName -ResourceGroupName $rgName;
$diskEncryptionKeyVaultUrl = $keyVault.VaultUri;
$keyVaultResourceId = $keyVault.ResourceId;
$keyEncryptionKeyUrl = (Get-AzureKeyVaultKey -VaultName $keyVaultName -Name 'myDEKey').Key.kid;

Set-AzureRmVMDiskEncryptionExtension -ResourceGroupName $rgName `
    -VMName $vmName `
    -AadClientID $app.ApplicationId `
    -AadClientSecret (New-Object PSCredential "user",$securePassword).GetNetworkCredential().Password `
    -DiskEncryptionKeyVaultUrl $diskEncryptionKeyVaultUrl `
    -DiskEncryptionKeyVaultId $keyVaultResourceId `
    -KeyEncryptionKeyUrl $keyEncryptionKeyUrl `
    -KeyEncryptionKeyVaultId $keyVaultResourceId

# review the encryption status
Get-AzureRmVmDiskEncryptionStatus  -ResourceGroupName $rgName -VMName $vmName

#OsVolumeEncrypted          : Encrypted
#DataVolumesEncrypted       : Encrypted
#OsVolumeEncryptionSettings : Microsoft.Azure.Management.Compute.Models.DiskEncryptionSettings
#ProgressMessage            : OsVolume: Encrypted, DataVolumes: Encrypted

#Disable disk encryption
Disable-AzureRmVMDiskEncryption -ResourceGroupName $rgName -VMName $vmName

#confirmed working!