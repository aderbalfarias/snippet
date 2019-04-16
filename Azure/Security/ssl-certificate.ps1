Login-AzureRmAccount

$fqdn="aderbal.com"
$pfxPath="C:\Users\aderbal\appservicecertificate.pfx"
$pfxPassword="OGJnm3wdQ0UYCTqAkNI8oXpWy7BhveaPVstD5i9l4R2FbL1fKZ"
$webappname="myaddressbookplus"
$location="northeurope"
$resourceGroupName = "Pluralsight"

Write-Host "Configure a CNAME/A record that maps $fqdn to $webappname.azurewebsites.net"
Read-Host "Press [Enter] key when ready ..."

# Before continuing, go to your DNS configuration UI for your custom domain and follow the 
# instructions at https://aka.ms/appservicecustomdns to configure a CNAME record for the 
# hostname "www" and point it your web app's default domain name.

# Add a custom domain name to the web app. 
Set-AzureRmWebApp -Name $webappname -ResourceGroupName $resourceGroupName `
-HostNames @($fqdn,"$webappname.azurewebsites.net")

# Upload and bind the SSL certificate to the web app.
New-AzureRmWebAppSSLBinding -WebAppName $webappname -ResourceGroupName $resourceGroupName -Name $fqdn `
-CertificateFilePath $pfxPath -CertificatePassword $pfxPassword -SslState SniEnabled

