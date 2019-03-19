az login -u xxx@exampleorg.com -p Saturn2005$

az storage blob upload --file "C:\Upload\testfile1.txt" --container-name marketingdocs --name training.txt --account-name exampleorgstgacct01

- storage account not found if user doesn't have permissions

az storage blob upload --file "C:\Upload\testfile1.txt" --container-name marketingdocs --name training.txt --account-name exampleorgstgacct01 --account-key myAccountKey

- successful when using account key (from portal)

az storage account list --resource-group exampleorgrg01

- the client does not have authorization to perform action read over scope...

az storage account keys list --account-name exampleorgstgacct01 --resource-group exampleorgrg01

az storage account keys renew --account-name exampleorgstgacct01 --resource-group exampleorgrg01 --key secondary

add user to Storage Account Key Operator role

az logout

az login

az storage account list --resource-group exampleorgrg01

az storage account keys list --account-name exampleorgstgacct01 --resource-group exampleorgrg01

az storage account keys renew --account-name exampleorgstgacct01 --resource-group exampleorgrg01 --key secondary

az storage account keys list --account-name exampleorgstgacct01 --resource-group exampleorgrg01

