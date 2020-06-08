# Get help for CLI and specific commands 
az -h
az login -h
az account set -h

# Authenticate with Azure management services
az login

# Select the appropriate Azure subscription
az account set -s MySubscription

# Get help on Azure Batch
az batch -h

# Get help on Azure Batch accounts
az batch account -h

# List the Azure Batch accounts available
az batch account list

# Authenticate with an Azure Batch account
az batch account login -g azurebatchdemo -n azurebatch05

# Get help on Azure Batch jobs
az batch job -h

# Get help on creating Azure Batch jobs
az batch job create -h

# Create a job assigned to a specific pool
az batch job create --id CliJob01 --pool-id RenderPool

# Create the Tasks
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000000.pi -o F000000.tga" --resource-files F000000.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000000
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000001.pi -o F000001.tga" --resource-files F000001.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000001
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000002.pi -o F000002.tga" --resource-files F000002.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000002
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000003.pi -o F000003.tga" --resource-files F000003.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000003
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000004.pi -o F000004.tga" --resource-files F000004.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000004
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000005.pi -o F000005.tga" --resource-files F000005.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000005
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000006.pi -o F000006.tga" --resource-files F000006.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000006
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000007.pi -o F000007.tga" --resource-files F000007.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000007
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000008.pi -o F000008.tga" --resource-files F000008.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000008
az batch task create --job-id CliJob01 --command-line "cmd /c %AZ_BATCH_APP_PACKAGE_POLYRAY%\polyray.exe F000009.pi -o F000009.tga" --resource-files F000009.pi=https://azstorageteste01.blob.core.windows.net/demoscenes/F000000.pi --task-id F000009

