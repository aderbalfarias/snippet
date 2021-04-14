# How to call it:
# PS C:\xxx> .\UpdateDatabase.ps1 'C:\Git\DatabaseScript' 'dbserver' 'dbinstance' 'dbuser' 'userpassword' 1

$Scriptpath  = $args[0]
$Server =  $args[1]
$database = $args[2]
$user= $args[3]
$pwd= $args[4]
$Includesubfolders=$args[5]
 
Function IsDBInstalled([string]$Server, [string]$database)
{
    $t=Invoke-Sqlcmd -ServerInstance $Server -Username  $user -Password  $pwd -Database "master" -Query "select 1 from sys.databases where name='$database'" -OutputSqlErrors $true 
    if (!$t) 
    {
        Write-Host "Failed to connect to [$database] database on [$Server]" -BackgroundColor darkred 
        Write-Error "Failed to connect to [$database] database on [$Server]" -ErrorAction Stop
    } 
    else 
    {
        write-host "[$database] Database exists in SQL Server [$Server]" -BackgroundColor blue -ForegroundColor black
        write-host 
    }
}
 
IsDBInstalled $Server $database
 
if($Includesubfolders -eq 1) 
{
    $subscripts = Get-ChildItem $Scriptpath -recurse | Where-Object {$_.Extension -eq ".sql"}

    foreach ($s in $subscripts)
    {   
        Write-Host "Running Script:" $s.Name -BackgroundColor green -ForegroundColor darkRed
        # $tables=Invoke-Sqlcmd -ServerInstance $Server -Username  $user -Password  $pwd -Database  $database -InputFile $s.FullName -ErrorAction 'Stop' -querytimeout ([int]::MaxValue)
        $tables=Invoke-Sqlcmd -ServerInstance $Server -Username  $user -Password  $pwd -Database  $database -InputFile $s.FullName -querytimeout ([int]::MaxValue)
        # write-host ($tables | Format-List | Out-String)
    }
} 
else 
{
    $scripts = Get-ChildItem $Scriptpath | Where-Object {$_.Extension -eq ".sql"}

    foreach ($s in $scripts)
    {   
        Write-Host "Running Script:" $s.Name -BackgroundColor green -ForegroundColor darkRed
        # $tables=Invoke-Sqlcmd -ServerInstance $Server -Username  $user -Password  $pwd -Database  $database -InputFile $s.FullName -ErrorAction 'Stop' -querytimeout ([int]::MaxValue)
        $tables=Invoke-Sqlcmd -ServerInstance $Server -Username  $user -Password  $pwd -Database  $database -InputFile $s.FullName -querytimeout ([int]::MaxValue)
        # write-host ($tables | Format-List | Out-String) 
    }
}
