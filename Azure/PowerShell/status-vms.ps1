#Now let's examine virtual machine states using CloudShell

#Started
Get-AzureRmVM -ResourceGroupName "az203-learn" -Name "az203-linux-1a" -Status 

#Stopping
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/az203-linux-1a' | Stop-AzureRmVM -StayProvisioned -Force -AsJob

#We can use our Get-AzureRmVM cmdlet
Get-AzureRmVM -ResourceGroupName "az203-learn" -Name "az203-linux-1a"  -Status 

#Stopped
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/az203-linux-1a'

#VM state is cached, so we can use -Force to update
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/az203-linux-1a' -Force

#Deallocating - can take several minutes
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/az203-linux-1a' | Stop-AzureRmVM -Force -AsJob

#Deallocated
Get-AzureRmVM -ResourceGroupName "az203-learn" -Name "az203-linux-1a" -Status 


#Starting
Start-AzureRmVM -ResourceGroupName "az203-learn" -Name "az203-linux-1a" -AsJob

Get-AzureRmVM -ResourceGroupName "az203-learn" -Name "az203-linux-1a"  -Status 


#Remove the VM
Get-ChildItem 'Azure:/Visual Studio Professional/VirtualMachines/az203-linux-1a' | Remove-AzureRmVM -Force -AsJob 