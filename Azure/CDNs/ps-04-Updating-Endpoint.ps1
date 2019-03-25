# Get existing CDN endpoint
$Endpoint = Get-AzureRmCdnEndpoint `
    -EndpointName psazurestoragecdn `
    -ProfileName psazurestoragecdn `
    -ResourceGroupName pluralsight-azure-storage-cdn

# Set new properties and update
$Endpoint.OriginHostHeader = "psazurestoragecdn.blob.core.windows.net"
$Endpoint | Set-AzureRmCdnEndpoint