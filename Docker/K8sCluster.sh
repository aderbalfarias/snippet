az group create --name MyK8sRG --location northeurope

az acs create --orchestrator-type kubernetes \
--resource-group MyK8sRG \
--name myK8sCluster \
--generate-ssh-keys

az acs kubernetes get-credentials \
--resource-group=MyK8sRG \
--name=myK8sCluster

kubectl get nodes