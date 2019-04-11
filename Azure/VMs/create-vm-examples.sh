#Examples
    #Create a default Ubuntu VM with automatic SSH authentication.
        az vm create -n MyVm -g MyResourceGroup --image UbuntuLTS


    #Create a default RedHat VM with automatic SSH authentication using an image URN.
        az vm create -n MyVm -g MyResourceGroup --image RedHat:RHEL:7-RAW:7.4.2018010506


    #Create a default Windows Server VM with a private IP address.
        az vm create -n MyVm -g MyResourceGroup --public-ip-address "" --image Win2012R2Datacenter


    #Create a VM from a custom managed image.
        az vm create -g MyResourceGroup -n MyVm --image MyImage


    #Create a VM by attaching to a managed operating system disk.
        az vm create -g MyResourceGroup -n MyVm --attach-os-disk MyOsDisk --os-type linux


    #Create an Ubuntu Linux VM using a cloud-init script for configuration. See:
    #https://docs.microsoft.com/azure/virtual-machines/virtual-machines-linux-using-cloud-init.
        az vm create -g MyResourceGroup -n MyVm --image debian --custom-data MyCloudInitScript.yml


    #Create a Debian VM with SSH key authentication and a public DNS entry, located on an existing virtual network and availability set.
        az vm create -n MyVm -g MyResourceGroup --image debian --vnet-name MyVnet --subnet subnet1 \
            --availability-set MyAvailabilitySet --public-ip-address-dns-name MyUniqueDnsName \
            --ssh-key-value @key-file


    #Create a simple Ubuntu Linux VM with a public IP address, DNS entry, two data disks (10GB and 20GB), and then generate ssh key pairs.
        az vm create -n MyVm -g MyResourceGroup --public-ip-address-dns-name MyUniqueDnsName \
            --image ubuntults --data-disk-sizes-gb 10 20 --size Standard_DS2_v2 \
            --generate-ssh-keys


    #Create a Debian VM using Key Vault secrets.
        az keyvault certificate create --vault-name vaultname -n cert1 \
          -p "$(az keyvault certificate get-default-policy)"

        secrets=$(az keyvault secret list-versions --vault-name vaultname \
          -n cert1 --query "[?attributes.enabled].id" -o tsv)

        vm_secrets=$(az vm secret format -s "$secrets")

        az vm create -g group-name -n vm-name --admin-username deploy  \
          --image debian --secrets "$vm_secrets"


    #Create a CentOS VM with a system assigned identity. The VM will have a 'Contributor' role with access to a storage account.
        az vm create -n MyVm -g rg1 --image centos --assign-identity --scope
        /subscriptions/99999999-1bf0-4dda-aec3-cb9272f09590/MyResourceGroup/myRG/providers/Microsoft.Storage/storageAccounts/storage1


    #Create a debian VM with a user assigned identity.
        az vm create -n MyVm -g rg1 --image debian --assign-identity
        /subscriptions/99999999-1bf0-4dda-aec3-cb9272f09590/resourcegroups/myRG/providers/Microsoft.ManagedIdentity/userAssignedIdentities/myID


    #Create a debian VM with both system and user assigned identity.
        az vm create -n MyVm -g rg1 --image debian --assign-identity  [system]
        /subscriptions/99999999-1bf0-4dda-aec3-cb9272f09590/resourcegroups/myRG/providers/Microsoft.ManagedIdentity/userAssignedIdentities/myID


    #Create a VM in an availability zone in the current resource group's region
        az vm create -n MyVm -g MyResourceGroup --image Centos --zone 1
