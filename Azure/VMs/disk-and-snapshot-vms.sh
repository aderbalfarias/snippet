#In Azure CLI
#1 - Attach a disk to an existing VM
#2 - Resizing a disk
#3 - Removing a disk
#4 - Snapshotting an OS volume and creating a VM from a snapshot

#Setup
#Requires a Linux VM, we're going to use psdemo-linux-1c from the m3 demo.

#login interactively and set a subscription to be the current active subscription
az login --subscription "Visual Studio Professional"

#Creating a new data disk with Azure CLI and attach it to our VM. This can be done hot.
#1 - Attach the new disk
az vm disk attach \
    --resource-group "psdemo-rg" \
    --vm-name "psdemo-linux-1c" \
    --disk "psdemo-linux-1c-st0" \
    --new \
    --size-gb 25 \
    --sku "Premium_LRS" #Other options are StandardSSD_LRS and Standard_LRS

#2 - Prepare the disk for use by the operating system
az vm list-ip-addresses \
    --name "psdemo-linux-1c" \
    --output table

ssh -l demoadmin MyIP

#3 - Find the new block decvice, we know /dev/sda is the OS, and /dev/sdb is the temporary disk.
#We also know it's 25GB, so /dev/sdc it is!
lsblk

#We can also use dmesg, like docs.microsoft.com says...
dmesg | grep SCSI

#4 - partition the disk with fdisk and use the following commands to name a new primary parition
sudo fdisk /dev/sdc
# m
# n
# p
# 1
# w

#5 - format the new partition with ext4 
sudo mkfs -t ext4 /dev/sdc1

#6 - Make a directory to mount the new disk under
sudo mkdir /data1

#7 - Add the following line to /etc/fstab. First find the UUID for this device, in our case it's /dev/sdc1
sudo -i blkid | grep sdc1
sudo vi /etc/fstab

#8 - mount the volume and verify the file system is mounted.
sudo mount -a
df -h

#9 - Exit from the Linux VM
exit

#------------------------------------------------------------------

#Resizing a disk
#1 - Stop and deallocate the VM. this has to be an offline operation.
az vm deallocate \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c"

#2 - Find the disk's name we want to expand
az disk list \
    --output table

#3 - Update the disk's size to the desired size
az disk update \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c-st0" \
    --size-gb 100

#4 - start up the VM again
az vm start \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c" 

#5 - Log into the guest OS and resize the volume
az vm list-ip-addresses \
    --name "psdemo-linux-1c" \
    --output table

ssh -l demoadmin MyIP

#6 - Unmount filesystem and expand the partition
sudo vi /etc/fstab #comment out our mount

sudo umount /data1
sudo parted /dev/sdc

#use print to find the size of the new disk, parition 1, resize, set the size to 107, quit
print 
resizepart
1
107GB
quit

#If the resizepart option isn't available update the parted package.

#7 - fsck, expand and mount the filesystem
sudo e2fsck -f /dev/sdc1
sudo resize2fs /dev/sdc1
sudo mount /dev/sdc1 /data1
sudo vi /etc/fstab
sudo mount -a

#8 - Verify the added space is available
df -h  | grep data1

#------------------------------------------------------------------------

#Removing a disk 
#1 - Umount the disk in the OS, remove the disk we added above from fstab
ssh -l demoadmin w.x.y.z
sudo vi /etc/fstab
sudo umount /data1
df -h | grep /data1
exit

#2 - Detaching the disk from the virtual machine. This can be done online too!
az vm disk detach \
    --resource-group "psdemo-rg" \
    --vm-name "psdemo-linux-1c" \
    --name "psdemo-linux-1c-st0"

#3 - Delete the disk
az disk delete \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c-st0" 

#-------------------------------------------------------------------

#Snapshotting the OS disk
#1 - Find the disk we want to snapshot and create a snapshot of the disk
az disk list --output table | grep psdemo-linux-1c

#update the --source parameter with the disk from the last command.
az snapshot create \
    --resource-group "psdemo-rg" \
    --source "psdemo-linux-1c_disk1_9c9f359319204c3a8d1f128685c13d22" \
    --name "psdemo-linux-1c-OSDisk-1-snap-1" 

#2 - Getting a list of the snapshots available
az snapshot list \
    --output table

#3 - Create a new disk from the snapshot we just created. 
#If this was a data disk, we could just attach and mount this disk to a VM
az disk create \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1f-OSDisk-1" \
    --source "psdemo-linux-1c-OSDisk-1-snap-1" \
    --size-gb "20"

#4 - Create a VM from the disk we just created
az vm create \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1f" \
    --attach-os-disk "psdemo-linux-1f-OSDisk-1" \
    --os-type "Linux"

#5 - If we want we can delete a snapshot when we're finished
az snapshot delete \
    --resource-group "psdemo-rg" \
    --name "psdemo-linux-1c-OSDisk-1-snap-1"

#Windows examples are available in the course downloads, in m4.demo1.ps1.
#Logically, the steps on Windows are the same for adding, expanding and removing a disk.