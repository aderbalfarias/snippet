Copy-dbEnv -SourceEnv '[Origin server]' -Dest '[Destination server]' -DatabasePattern '[db name]' -suffix '[suffix name]' -Force $True # Copy database
Get-EventLog -Log "Application" # Get log from a specific app
Stop-Process -processname [name of process] # Start a process