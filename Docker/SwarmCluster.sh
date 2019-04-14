az group create --name MyDockerRG --location northeurope

az acs create --name mySwarmCluster \
--orchestrator-type Swarm \
--resource-group MyDockerRG \
--generate-ssh-keys

az network public-ip list \
--resource-group MyDockerRG_mySwarmCluster_westus2 \
--query "[*].{Name:name,IPAddress:ipAddress}" -o table

ssh -i ~/.ssh/id_rsa -p 2200 -fNL 2375:localhost:2375 azureuser@IpFoundOnList