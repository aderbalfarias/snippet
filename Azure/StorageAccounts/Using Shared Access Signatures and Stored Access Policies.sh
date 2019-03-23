#show generating an account SAS in the portal
#show generating a container SAP in portal no container level SAS
#show generating a blob level SAS can't reference a SAP

#Create a Stored Access Policy in Azure CLI
az storage container policy create --container-name marketingdocs --name writerpolicy01 --account-name exampleorgstgacct01 --permissions lw --expiry 2018-07-25T00:00Z --account-key LmVB0NmKRksq+IEsLDe0dEQjaqYwbPrPX2zUEPqa28Gso3BuP6Qr+rO/8DhQORJ5wfIev3Sf4XDdJ4bjlDadfw==

#If you want to delete, just change "create" to "delete"
az storage container policy delete --container-name marketingdocs --name writerpolicy01 --account-name exampleorgstgacct01 --account-key LmVB0NmKRksq+IEsLDe0dEQjaqYwbPrPX2zUEPqa28Gso3BuP6Qr+rO/8DhQORJ5wfIev3Sf4XDdJ4bjlDadfw==

#List the Stored Access Policies on a Container
az storage container policy list --container-name marketingdocs --account-name exampleorgstgacct01 --account-key LmVB0NmKRksq+IEsLDe0dEQjaqYwbPrPX2zUEPqa28Gso3BuP6Qr+rO/8DhQORJ5wfIev3Sf4XDdJ4bjlDadfw==

#Generate a SAS for a specific Blob, referencing a Stored Access Policy
az storage blob generate-sas --container-name marketingdocs --name training.txt --account-name exampleorgstgacct01 --account-key LmVB0NmKRksq+IEsLDe0dEQjaqYwbPrPX2zUEPqa28Gso3BuP6Qr+rO/8DhQORJ5wfIev3Sf4XDdJ4bjlDadfw== --policy-name readerpolicy --https-only

#"spr=https&sv=2017-07-29&si=readerpolicy&sr=b&sig=9gwY6NcJHD7t/C4czt898gmjjAnpxUDGitdNPAXtwY4%3D"

#Use the SAS to show Blob properties
az storage blob show --container-name marketingdocs --name training.txt --account-name exampleorgstgacct01 --sas-token "spr=https&sv=2017-07-29&si=readerpolicy&sr=b&sig=9gwY6NcJHD7t/C4czt898gmjjAnpxUDGitdNPAXtwY4%3D"

#Get the URL of the Blob, using the SAS
az storage blob url --container-name marketingdocs --name training.txt --account-name exampleorgstgacct01 --sas-token "spr=https&sv=2017-07-29&si=readerpolicy&sr=b&sig=9gwY6NcJHD7t/C4czt898gmjjAnpxUDGitdNPAXtwY4%3D"

#"https://exampleorgstgacct01.blob.core.windows.net/marketingdocs/training.txt"

#try URL in a browser, without SAS
#try URL in a browser, with SAS appended

