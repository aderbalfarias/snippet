Get-AzureRmVM -ResourceGroupName az203 -Name 'vm-linux-red-az203' -Status
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/' #See VMs
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/vm-linux-red-az203' | Stop-AzureRmVM -StayProvisioned -Force -AsJob #Stop VM
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/vm-linux-red-az203' | Stop-AzureRmVM -Force -AsJob #Dealocate VM
Start-AzureRmVM -ResourceGroupName az203 -Name 'vm-linux-red-az203' -AsJob #Start VM
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/vm-linux-red-az203' | Remove-AzureRmVM -Force -AsJob #Remove VM