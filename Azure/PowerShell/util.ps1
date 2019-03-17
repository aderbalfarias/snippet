#Let's set our subscription context
Connect-AzureRmAccount -Subscription "Visual Studio Professional"

#And get a listing of VMs
Get-AzureRmVM

#We can store PowerShell scripts in our cloud drive too
cd ~/clouddrive

#Create a simple script and add a cmdlet to it
vi demo.ps1
Get-AzureRmVM

#Run our script
./demo.ps1

#PowerShell slapping techinique
$params = @{
    Param1 = "test"
    Param2 = 1
}
$content = $params | ConvertFrom-Json
$var1 = $content.Param1

