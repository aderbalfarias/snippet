# Creating Group, Storage Account, Batch Account, Pool, Job and Task

az group create --name az203-batches  --location westeurope

az storage account create --name az203batchesstorage --resource-group az203-batches

az batch account create --name az203batchtest  --storage-account az203batchesstorage --resource-group az203-batches --location westeurope
az203batchtest.westeurope.batch.azure.com

az batch pool create --id az203-pool --vm-size Standard_A1_v2 --target-dedicated-nodes 2 --image canonical:ubuntuserver:16.04-LTS --node-agent-sku-id "batch.node.ubuntu 16.04" --account-name az203batchtest

az batch pool show --pool-id az203-pool --query "allocationState"

az batch job create --id az203-job --pool-id az203-pool

for i in {1..4}
do
   az batch task create \
    --task-id az203-task$i \
    --job-id az203-job \
    --command-line "/bin/bash -c 'printenv | grep AZ_BATCH; sleep 90s'"

az batch task show \
    --job-id az203-job \
    --task-id az203-task1

az batch task file list \
    --job-id az203-job \
    --task-id az203-task1 \
    --output table

az batch task file download \
    --job-id az203-job \
    --task-id az203-task1 \
    --file-path stdout.txt \
    --destination ./stdout.txt