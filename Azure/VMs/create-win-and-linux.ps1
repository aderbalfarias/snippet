#Setup
#1 - Logged into Azure PowerShell, with Connect-AzureRmAccount.
#2 - Ensure you're in a PowerShell Integrated terminal session.

#Demo outline
#1 - Create a Linux VM, specifying individual resources. 
#2 - Create a Windows VM, using PowerShell splatting to feed all the required parameters.

#Install AzureRm.Netcore in PowerShell Core
Install-Module AzureRM.Netcore

#Exit out of the elevated pwsh
exit

#Installing AzureRm for Windows
#Launch an elevated Windows PowerShell
#Install-Module -Name AzureRM

#More information is available here
#https://docs.microsoft.com/en-us/powershell/azure/install-azurerm-ps?view=azurermps-6.7.0

#Start a connection with Azure
Connect-AzureRmAccount -Subscription 'Visual Studio Professional'

#Let's create a Linux VM with PowerShell
#1 - Get resource group created in the  demo
$rg = New-AzureRmResourceGroup `
  -Name 'vms-learn' `
  -Location 'northeurope'
$rg

#2a - Create a subnet configuration
$subnetConfig = New-AzureRmVirtualNetworkSubnetConfig `
    -Name 'vms-subnet-2' `
    -AddressPrefix '10.2.1.0/24'
$subnetConfig

#2b - Create a virtual network
$vnet = New-AzureRmVirtualNetwork `
    -ResourceGroupName $rg.ResourceGroupName `
    -Location $rg.Location `
    -Name 'vms-vnet-2' `
    -AddressPrefix '10.2.0.0/16' `
    -Subnet $subnetConfig
$vnet

#3 - Create public IP address
$pip = New-AzureRmPublicIpAddress `
    -ResourceGroupName $rg.ResourceGroupName `
    -Location $rg.Location `
    -Name 'vms-linux-2-pip-1' `
    -AllocationMethod Static
$pip

#4a - Create network security group rule for SSH 
#Example of a more granular approach to creating a security rule.
$rule1 = New-AzureRmNetworkSecurityRuleConfig `
    -Name ssh-rule `
    -Description 'Allow SSH' `
    -Access Allow `
    -Protocol Tcp `
    -Direction Inbound `
    -Priority 100 `
    -SourceAddressPrefix Internet `
    -SourcePortRange * `
    -DestinationAddressPrefix * `
    -DestinationPortRange 22
$rule1

#4a - Create network security group, with the newly created rule
$nsg = New-AzureRmNetworkSecurityGroup `
    -ResourceGroupName $rg.ResourceGroupName `
    -Location $rg.Location `
    -Name 'vms-linux-nsg-2' `
    -SecurityRules $rule1
$nsg | more

#5 - Create a virtual network card and associate with public IP address and NSG
#First, let's get an object representing our current subnet
$subnet = $vnet.Subnets | Where-Object { $_.Name -eq 'vms-subnet-2' }

$nic = New-AzureRmNetworkInterface `
    -ResourceGroupName $rg.ResourceGroupName `
    -Location $rg.Location `
    -Name 'vms-linux-2-nic-1' `
    -Subnet $subnet `
    -PublicIpAddress $pip `
    -NetworkSecurityGroup $nsg
$nic

#6a - Create a virtual machine configuration
$LinuxVmConfig = New-AzureRmVMConfig `
    -VMName 'vms-linux-2' `
    -VMSize 'Standard_B2s'

#6b - set the comptuer name, OS type and, auth methods.
$password = ConvertTo-SecureString 'password123412123$%^&*' -AsPlainText -Force
$LinuxCred = New-Object System.Management.Automation.PSCredential ('demoadmin', $password)

$LinuxVmConfig = Set-AzureRmVMOperatingSystem `
    -VM $LinuxVmConfig `
    -Linux `
    -ComputerName 'vms-linux-2' `
    -DisablePasswordAuthentication `
    -Credential $LinuxCred

#6c - Read in our SSH Keys and add to the vm config
$sshPublicKey = Get-Content "~/.ssh/id_rsa.pub"
Add-AzureRmVMSshPublicKey `
    -VM $LinuxVmConfig `
    -KeyData $sshPublicKey `
    -Path "/home/demoadmin/.ssh/authorized_keys"

#6d - get the VM image name, and set it in the VM config in this case UbuntuServer/latest
Get-AzureRmVMImageSku -Location $rg.Location -PublisherName "Canonical" -Offer "UbuntuServer"

$LinuxVmConfig = Set-AzureRmVMSourceImage `
    -VM $LinuxVmConfig `
    -PublisherName 'Canonical' `
    -Offer 'UbuntuServer' `
    -Skus '18.04-LTS' `
    -Version '18.04.201804262' 

#6e - assign the created network interface to the vm
$LinuxVmConfig = Add-AzureRmVMNetworkInterface `
    -VM $LinuxVmConfig `
    -Id $nic.Id 

# Create a virtual machine, passing in the VM Configuration, network, image etc are in the config.
New-AzureRmVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Location $rg.Location `
    -VM $LinuxVmConfig


$MyIP = Get-AzureRmPublicIpAddress `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name $pip.Name | Select-Object -ExpandProperty IpAddress
$MyIP    

#Connect to our VM via SSH
ssh -l demoadmin $MyIP


#Let's create a Windows VM with a little less code...using PowerShell Splatting.

#Create PSCredential object, this will be used for the Windows username/password
$password = ConvertTo-SecureString 'password123412123$%^&*' -AsPlainText -Force
$WindowsCred = New-Object System.Management.Automation.PSCredential ('demoadmin', $password)

#We're using the ImageName parameter, for a list of images look here
#https://docs.microsoft.com/en-us/powershell/module/azurerm.compute/new-azurermvm?view=azurermps-6.7.0#optional-parameters

#Use tab complete to help find your image name, enter into the Terminal
New-AzureRMVm -Image 

$vmParams = @{
    ResourceGroupName = 'vms-learn'
    Name = 'vms-win-2'
    Location = 'northeurope'
    Size = 'Standard_D2_V2'
    Image = 'Win2016Datacenter'
    PublicIpAddressName = 'vms-win-2-pip-1'
    Credential = $WindowsCred
    VirtualNetworkName = 'vms-vnet-2'
    SubnetName = 'vms-subnet-2'
    SecurityGroupName = 'vms-win-nsg-2'
    OpenPorts = 3389
}

New-AzureRmVM @vmParams 

Get-AzureRmPublicIpAddress `
    -ResourceGroupName 'vms-learn' `
    -Name 'vms-win-2-pip-1' | Select-Object -ExpandProperty IpAddress
