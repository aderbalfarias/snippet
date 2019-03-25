# First, we remove the endpoint
Remove-AzureRmCdnEndpoint `
    -EndpointName psazurestoragecdn `
    -ProfileName psazurestoragecdn `
    -ResourceGroupName pluralsight-azure-storage-cdn

# Now, remove profile
Remove-AzureRmCdnProfile `
    -ProfileName psazurestoragecdn `
    -ResourceGroupName pluralsight-azure-storage-cdn