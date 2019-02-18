#login interactively and set a subscription to be the current active subscription
Connect-AzureRmAccount -Subscription 'Demonstration Account'

#Creating a new data disk with PowerShell and attach it to our VM. This can be done hot.
#1 - Define a new DiskConfig, Create the DataDisk and attach the new disk
$diskConfig = New-AzureRmDiskConfig `
    -Location 'Central US' `
    -OsType 'Windows' `
    -CreateOption Empty `
    -DiskSizeGB 50 `
    -SkuName 'Premium_LRS' #'Standard_LRS, HDD'

$dataDisk = New-AzureRmDisk `
    -ResourceGroupName 'psdemo-rg' `
    -DiskName 'psdemo-win-1c-st0' `
    -Disk $diskConfig 

$vm = Get-AzureRmVM `
    -ResourceGroupName 'psdemo-rg' `
    -Name 'psdemo-win-1c' 

$vm = Add-AzureRmVMDataDisk `
    -VM $vm `
    -Name "psdemo-win-1c-st0" `
    -CreateOption Attach `
    -ManagedDiskId $dataDisk.Id `
    -Lun 1

Update-AzureRmVM `
    -ResourceGroupName 'psdemo-rg' `
    -VM $vm 

#2 - Prepare the disk for use by the operating system, initialize disk, add a partition and format the disk
#PERFORM THIS OPERATION ON THE SERVER VIA Remote Desktop.
$NewDisk = Get-Disk | Where-Object { $_.Location -like "*LUN 1*" }

$DriveLetter = 'P'
$label = "DATA1"

$NewDisk | Initialize-Disk -PartitionStyle MBR -PassThru | `
    New-Partition -UseMaximumSize -DriveLetter $driveLetter | `
    Format-Volume -FileSystem NTFS -NewFileSystemLabel $label -Force

#Resizing a data disk
#1 - Stop and deallocate the VM. This has to be an offline operation.
Stop-AzureRmVM `
    -ResourceGroupName 'psdemo-rg' `
    -Name 'psdemo-win-1c' `
    -Force

#2 - Find the disk's name we want to expand. This is the disk we added to our VM in the previous demo.
$disk = Get-AzureRmDisk `
    -ResourceGroupName 'psdemo-rg' `
    -DiskName "psdemo-win-1c-st0"

#3 - Update the disk's size and update the Disk configuration.
$disk.DiskSizeGB = 1024

Update-AzureRmDisk `
    -ResourceGroupName 'psdemo-rg' `
    -Disk $disk `
    -DiskName $disk.Name

#4 - start up the VM again
Start-AzureRmVm `
    -ResourceGroupName 'psdemo-rg' `
    -Name 'psdemo-win-1c' 


#5 - Log into the guest OS via Remote Desktop and resize the volume using diskpart. 
diskpart
list volume #we're looking for P:
select volume NN
extend



#Removing a disk 
#1 - Detaching the disk from the virtual machine. This can be done online too.
$vm = Get-AzureRmVM `
    -ResourceGroupName 'psdemo-rg' `
    -Name 'psdemo-win-1c' 

#2 - Remove the disk from the VM config
Remove-AzureRmVMDataDisk `
    -VM $vm `
    -Name "psdemo-win-1c-st0"

#3 - Update the VM config, this actually detaches the disk
Update-AzureRmVM `
    -ResourceGroupName 'psdemo-rg' `
    -VM $vm

#4 - Delete the disk from our Resource Group. This ACTUALLY DELETES the disk
Remove-AzureRmDisk `
    -ResourceGroupName 'psdemo-rg' `
    -DiskName 'psdemo-win-1c-st0' 



#Snapshotting the OS disk
#1 - Create a snapshot of the OS disk
$vm = Get-AzureRmVM `
    -ResourceGroupName 'psdemo-rg' `
    -Name 'psdemo-win-1c' 

$snapshotconfig = New-AzureRmSnapshotConfig `
    -Location 'Central US' `
    -DiskSizeGB 127 `
    -AccountType 'Premium_LRS' `
    -OsType Windows `
    -CreateOption Empty
    
New-AzureRmSnapshot `
    -ResourceGroupName 'psdemo-rg' `
    -Snapshot $snapshotconfig `
    -SnapshotName "psdemo-win-1-OSDisk-1-snap-1" 

#2 - Getting the snapshot we just created
$SnapShot = Get-AzureRmSnapshot `
    -ResourceGroupName 'psdemo-rg' `
    -SnapshotName "psdemo-win-1-OSDisk-1-snap-1" 

#3 - Create a new disk from the snapshot we just created
#If this was a data disk, we could just mount this disk to a VM.
$DiskConfig = New-AzureRmDiskConfig `
    -Location $SnapShot.Location `
    -SourceResourceId $SnapShot.Id `
    -CreateOption Copy
 
$Disk = New-AzureRmDisk `
    -ResourceGroupName 'psdemo-rg' `
    -Disk $DiskConfig `
    -DiskName 'psdemo-win-1g-OSDisk-1'

#4 - Create a VM from the disk we just created, we'll also need to create a network config here too.
$VirtualMachine = New-AzureRmVMConfig `
    -VMName 'psdemo-win-1g' `
    -VMSize 'Standard_D1'

$VirtualMachine = Set-AzureRmVMOSDisk `
    -VM $VirtualMachine `
    -ManagedDiskId $disk.Id `
    -CreateOption Attach `
    -Windows

$vnet = Get-AzureRmVirtualNetwork `
    -ResourceGroupName 'psdemo-rg' `
    -Name 'psdemo-vnet-1'

$nic = New-AzureRmNetworkInterface `
    -ResourceGroupName 'psdemo-rg' `
    -Location 'centralus' `
    -SubnetId $vnet.Subnets[0].Id `
    -Name 'psdemo-win-1g-nic-1' 

$VirtualMachine = Add-AzureRmVMNetworkInterface `
    -VM $VirtualMachine `
    -Id $nic.Id

New-AzureRmVM `
    -ResourceGroupName 'psdemo-rg' `
    -VM $VirtualMachine `
    -Location $snapshot.Location

#5 - If we want we can delete a snapshot when we're finished
Remove-AzureRmSnapshot `
    -ResourceGroupName 'psdemo-rg' `
    -SnapshotName "psdemo-win-1-OSDisk-1-snap-1" `
    -Force
