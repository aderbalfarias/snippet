# Create full VM image

$vmName = 'rg-vms'
$rgName = 'myResourceGroup'
$location = 'northeurope'
$imageName = 'myImage'

Stop-AzureRmVM -ResourceGroupName $rgName -Name $vmName -Force

Set-AzureRmVm -ResourceGroupName $rgName -Name $vmName -Generalized

$vm = Get-AzureRmVM -Name $vmName -ResourceGroupName $rgName

$image = New-AzureRmImageConfig -Location $location -SourceVirtualMachineId $vm.ID

New-AzureRmImage -Image $image -ImageName $imageName -ResourceGroupName $rgName

# Create an image from a snapshot

$rgName = 'rg-vms'
$location = 'northeurope'
$snapshotName = 'mySnapshot'
$imageName = 'myImage'

$snapshot = Get-AzureRmSnapshot -ResourceGroupName $rgName -SnapshotName $snapshotName

$imageConfig = New-AzureRmImageConfig -Location $location

$imageConfig = Set-AzureRmImageOsDisk -Image $imageConfig -OsState Generalized -OsType Windows -SnapshotId $snapshot.Id

New-AzureRmImage -ImageName $imageName -ResourceGroupName $rgName -Image $imageConfig






