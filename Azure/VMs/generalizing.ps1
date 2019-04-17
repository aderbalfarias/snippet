#Generalizing and creating a custom image using PowerShell
#Setup - pre-stage the RDP connection for the Windows VM - 'az203-win-1'
#Ensure we're in the PowerShell Integrated Console.

#You can use Azure CLI or PowerShell on Windows or Linux Systems.

#Start a connection with Azure
Connect-AzureRmAccount -Subscription 'Visual Studio Professional'

#Open and RDP session to this Windows VM and run this command in a command prompt. 
#This will Generalize the VM and shut it down.    
%WINDIR%\system32\sysprep\sysprep.exe /generalize /shutdown /oobe

#Let's get the status of our VM and ensure it's shut down first.
Get-AzureRmVM `
    -ResourceGroupName 'az203-learn' `
    -Name 'az203-win-1' `
    -Status 

#Find our Resource Group
$rg = Get-AzureRmResourceGroup `
    -Name 'az203-learn' `
    -Location 'northeurope'

#Find our VM in our Resource Group
$vm = Get-AzureRmVm `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name "az203-win-1"

#Deallocate the virtual machine
Stop-AzureRmVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name $vm.Name `
    -Force

#Check the status of the VM to see if it's deallocated
Get-AzureRmVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name $vm.Name `
    -Status 

#Mark the virtual machine as "generalized"
Set-AzureRmVM `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name $vm.Name `
    -Generalized

#Start an Image Configuration from our source Virtual Machine $vm
$image = New-AzureRmImageConfig `
    -Location $rg.Location `
    -SourceVirtualMachineId $vm.ID

#Create a VM from the custom image config we just created, simply specify the image config as a source.
New-AzureRmImage `
    -ResourceGroupName $rg.ResourceGroupName `
    -Image $image `
    -ImageName "az203-win-ci-1"

#Summary image information. You'll see two images, one Linux and on Windows.
Get-AzureRmImage `
    -ResourceGroupName $rg.ResourceGroupName

#Create user object, this will be used for the Windows username/password
$password = ConvertTo-SecureString 'password123412123$%^&*' -AsPlainText -Force
$WindowsCred = New-Object System.Management.Automation.PSCredential ('demoadmin', $password)

#Let's create a VM from our new image, we'll use a more terse definition for this VM creation
New-AzureRmVm `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name "az203-win-1c" `
    -ImageName "az203-win-ci-1" `
    -Location 'northeurope' `
    -Credential $WindowsCred `
    -VirtualNetworkName 'az203-vnet-2' `
    -SubnetName 'az203-subnet-2' `
    -SecurityGroupName 'az203-win-nsg-2' `
    -OpenPorts 3389

#Check out the status of our provisioned VM from the Image
Get-AzureRmVm `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name "az203-win-1c"

#You can delete the deallocated source VM
Remove-AzureRmVm `
    -ResourceGroupName $rg.ResourceGroupName `
    -Name "az203-win-1" `
    -Force

#And that still leaves the image in our Resource Group
Get-AzureRmImage `
    -ResourceGroupName $rg.ResourceGroupName `
    -ImageName 'az203-win-ci-1'

# $images = Get-AzureRMResource -ResourceType Microsoft.Compute/images 
# $images.name

# Remove-AzureRmImage `
#     -ImageName myOldImage `
#     -ResourceGroupName myResourceGroup