# Create new CDN profile
New-AzureRmCdnProfile `
    -ProfileName psazurestoragecdn `
    -Location 'East US' `
    -Sku Standard_Akamai `
    -ResourceGroupName pluralsight-azure-storage-cdn

# Create a new Azure CDN endpoint within the new
# CDN profile just created
New-AzureRmCdnEndpoint `
    -ProfileName psazurestoragecdn `
    -EndpointName psazurestoragecdn `
    -ResourceGroupName pluralsight-azure-storage-cdn `
    -Location 'Central US' `
    -OriginName psazurestoragecdn `
    -OriginHostName psazurestoragecdn.blob.core.windows.net `
    -IsHttpAllowed:$false