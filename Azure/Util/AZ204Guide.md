# Azure Certification Exam AZ-204 Guide

## 1. Develop Azure compute solutions (25-30%)

| Topic | Links |
| ---- | ----------- |
| <b>Implement IaaS solutions</b> |
| Provision VMs | [Create VM - PowerShell](https://docs.microsoft.com/en-us/azure/virtual-machines/windows/quick-create-powershell]) <br> [Create VM - Azure Portal](https://docs.microsoft.com/en-us/azure/virtual-machines/windows/quick-create-portal])|
| Configure VMs for remote access | [Just in time VMs](https://docs.microsoft.com/en-us/azure/security-center/security-center-just-in-time?tabs=jit-config-asc%2Cjit-request-asc) |
|Create ARM templates| [Create and deploy ARM](https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/quickstart-create-templates-use-the-portal) <br> [ARM tamplates on GitHub](https://github.com/Azure/azure-quickstart-templates) |
|Create container images for solutions by using Docker| [Create a container image](https://docs.microsoft.com/en-us/azure/container-instances/container-instances-tutorial-prepare-app) |
|Publish an image to the Azure Container Registry|[Create container and push](https://docs.microsoft.com/en-us/azure/container-instances/container-instances-tutorial-prepare-acr#push-image-to-azure-container-registry)|
|Run containers by using Azure Container Instance|[Deploy a container](https://docs.microsoft.com/en-us/azure/container-instances/container-instances-tutorial-deploy-app)|
|<b>Create Azure App Service Web Apps</b>||
|Create an Azure App Service Web App|[Create a web app](https://docs.microsoft.com/en-us/azure/app-service/environment/app-service-web-how-to-create-a-web-app-in-an-ase)<br>[ASP.NET Core web app in Azure](https://docs.microsoft.com/en-us/azure/app-service/app-service-web-get-started-dotnet)|
|Enable diagnostics logging|[Enable diagnostics in Azure](https://docs.microsoft.com/en-us/azure/app-service/troubleshoot-diagnostic-logs)|
|Deploy code to a web app|[Deploy your app to Azure](https://docs.microsoft.com/en-us/azure/app-service/deploy-zip)|
|Configure web app settings including SSL, API, and connection strings|[TLS/SSL binding in Azure](https://docs.microsoft.com/en-us/azure/app-service/configure-ssl-bindings)<br>[Configure an App Service app](https://docs.microsoft.com/en-us/azure/app-service/configure-common)<br>[RESTful API with CORS](https://docs.microsoft.com/en-us/azure/app-service/configure-common)|
|Implement autoscaling rules, including scheduled autoscaling, and scaling by operational or system metrics|[Autoscale in Azure](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/autoscale-get-started) <br> [Autoscale Setting](https://docs.microsoft.com/en-us/azure/azure-monitor/learn/tutorial-autoscale-performance-schedule) <br> [Autoscale patterns](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/autoscale-common-scale-patterns) <br>[Autoscaling](https://docs.microsoft.com/en-us/azure/architecture/best-practices/auto-scaling)|
|<b>Implement Azure functions</b>||
|Implement input and output bindings for a function|[Triggers and bindings concepts](https://docs.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings)|
|Implement function triggers by using data operations, timers, and webhooks|[Timer trigger](https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-timer?tabs=csharp)<br>[Create a function in Azure](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-scheduled-function)|
|Implement Azure Durable Functions|[Create Durable Functions](https://docs.microsoft.com/en-us/azure/azure-functions/durable/durable-functions-create-portal)|

## 2. Develop for Azure storage (10-15%)

| Topic | Links |
| ---- | ----------- |
|<b>Develop solutions that use Cosmos DB storage</b>||
|Select the appropriate API for your solution|[Azure Cosmos DB Module](https://docs.microsoft.com/en-us/learn/modules/choose-api-for-cosmos-db/)|
|Implement partitioning schemes|[Model and partition data](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-model-partition-example)|
|Interact with data using the appropriate SDK|[Azure Cosmos DB by using .NET](https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-dotnet-application)<br>[Azure Cosmos DB SQL API](https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-get-started)|
|Set the appropriate consistency level for operations|[Consistency levels in Azure](https://docs.microsoft.com/en-us/azure/cosmos-db/consistency-levels)<br>[Consistency level](https://docs.microsoft.com/en-us/azure/cosmos-db/consistency-levels-choosing)|
|Create Cosmos DB containers|[Create an Azure Cosmos](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-create-container)|
|Implement scaling (partitions, containers) |[Partitioning and horizontal scaling](https://docs.microsoft.com/en-us/azure/cosmos-db/partition-data)<br>[Provision autoscale](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-provision-autoscale-throughput?tabs=api-async)|
|Implement server-side programming including stored procedures, triggers, and change feed notifications|[Azure Cosmos DB Server-side](https://azure.microsoft.com/en-in/resources/videos/azure-cosmosdb-server-side-programmability/)<br>[Store Procedures](https://docs.microsoft.com/en-us/rest/api/cosmos-db/stored-procedures)<br>[Triggers](https://docs.microsoft.com/en-us/rest/api/cosmos-db/triggers)<br>[Additional](https://docs.microsoft.com/en-us/azure/cosmos-db/how-to-write-stored-procedures-triggers-udfs)|
|<b>Develop solutions that use blob storage</b>||
|Move items in Blob storage between storage accounts or containers|[Move Azure storage blobs](https://docs.microsoft.com/en-us/learn/modules/copy-blobs-from-command-line-and-code/3-move-blobs-using-cli)|
|Set and retrieve properties and metadata|[Setting properties and metadata](https://docs.microsoft.com/en-us/previous-versions/azure/storage/common/storage-import-export-tool-setting-properties-metadata-import-v1?toc=%2Fazure%2Fstorage%2Fblobs%2Ftoc.json)|
|Interact with data using the appropriate SDK|[Quickstart: Azure Blob](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet)|
|Implement data archiving and retention|[Manage the Azure Blob](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-lifecycle-management-concepts?tabs=azure-portal)|
|Implement hot, cool, and archive storage|[Azure Blob storage tiers](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-storage-tiers?tabs=azure-portal)|

## 3. Implement Azure security (15-20%)
| Topic | Links |
| ---- | ----------- |
|<b>Implement user authentication and authorization</b>||
|Implement OAuth2 authentication|[Add sign-in](https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-asp-webapp)|
|Create and implement shared access signatures|[Grant limited access](https://docs.microsoft.com/en-us/azure/storage/common/storage-sas-overview)|
|Register apps and use Azure Active Directory to authenticate users|[Quickstart: Register an app](https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-register-app)|
|Control access to resources by using role-based access controls (RBAC)|[Add or remove Azure role](https://docs.microsoft.com/en-us/azure/role-based-access-control/role-assignments-portal)|
|<b>Implement secure cloud solutions</b>||
|Secure app configuration data by using the App Configuration and KeyVault API|[Securely save secret application](https://docs.microsoft.com/en-us/azure/key-vault/general/vs-secure-secret-appsettings)<br>[Use Key Vault](https://docs.microsoft.com/en-us/azure/app-service/app-service-key-vault-references)|
|Manage keys, secrets, and certificates by using the KeyVault API|[Configure and manage secrets](https://docs.microsoft.com/en-us/learn/modules/configure-and-manage-azure-key-vault/)|
|Implement Managed Identities for Azure resources|[Resource Manager](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/tutorial-windows-vm-access-arm)|

## 4. Monitor, troubleshoot, and optimize Azure solutions (10-15%)
| Topic | Links |
| ---- | ----------- |
|<b>Integrate caching and content delivery within solutions</b>||
|Develop code to implement CDNs in solutions|[Azure CDN development](https://docs.microsoft.com/en-us/azure/cdn/cdn-app-dev-net)|
|Configure cache and expiration policies for FrontDoor, CDNs, or Redis caches Store and retrieve data in Azure Redis cache|[Caching with Azure](https://docs.microsoft.com/en-us/azure/frontdoor/front-door-caching)<br>[Control Azure CDN caching](https://docs.microsoft.com/en-us/azure/cdn/cdn-caching-rules)<br>[Azure Cache for Redis](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-configure)<br>[Quickstart: Use Azure Cache](https://docs.microsoft.com/en-us/azure/azure-cache-for-redis/cache-dotnet-core-quickstart)|
|<b>Instrument solutions to support monitoring and logging</b>||
|Configure instrumentation in an app or service by using Application Insights|[Start Monitoring App](https://docs.microsoft.com/en-us/azure/azure-monitor/learn/dotnetcore-quick-start#start-monitoring-in-the-azure-portal)|
|Analyze log data and troubleshoot solutions by using Azure Monitor|[Azure Activity log](https://docs.microsoft.com/en-us/azure/azure-monitor/platform/activity-log)|
|Implement Application Insights Web Test and Alerts|[Creating an Application Insights](https://azure.microsoft.com/en-in/blog/creating-a-web-test-alert-programmatically-with-application-insights/)|
|Implement code that handles transient faults|[Transient Fault Handling](https://docs.microsoft.com/en-us/aspnet/aspnet/overview/developing-apps-with-windows-azure/building-real-world-cloud-apps-with-windows-azure/transient-fault-handling)|

## 5. Connect to and consume Azure services and third-party services (25-30%)
| Topic | Links |
| ---- | ----------- |
|<b>Develop an App Service Logic App</b>||
|Create a Logic App|[Azure Logic Apps](https://docs.microsoft.com/en-us/azure/logic-apps/quickstart-create-first-logic-app-workflow)|
|Create a custom connector for Logic Apps|[Create a custom connector](https://docs.microsoft.com/en-us/connectors/custom-connectors/define-blank)|
|Create a custom template for Logic Apps|[Deployment for Logic Apps](https://docs.microsoft.com/en-us/azure/logic-apps/logic-apps-create-azure-resource-manager-templates)|
|<b>Implement API Management</b>||
|Create an APIM instance|[Azure API Management](https://docs.microsoft.com/en-us/azure/api-management/get-started-create-service-instance)|
|Configure authentication for API|[APIs using client certificate](https://docs.microsoft.com/en-us/azure/api-management/api-management-howto-mutual-certificates-for-clients)|
|Define policies for APIs|[Azure APIM policies](https://docs.microsoft.com/en-us/azure/api-management/set-edit-policies)|
|<b>Develop event-based solutions</b>||
|Implement solutions that use Azure Event Grid|[Tutorial: Automate Event Grid](https://docs.microsoft.com/en-us/azure/event-grid/resize-images-on-storage-blob-upload-event?tabs=dotnet)|
|Implement solutions that use Azure Notification Hubs|[Azure Notification Hubs](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-windows-store-dotnet-get-started-wns-push-notification)|
|Implement solutions that use Azure Event Hub|[Azure Event Hubs](https://docs.microsoft.com/en-us/azure/event-hubs/event-hubs-tutorial-visualize-anomalies)|
|<b>Develop message-based solutions</b>||
|Implement solutions that use Azure Service Bus|[Tutorial: Update inventory](https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-tutorial-topics-subscriptions-powershell)|
|Implement solutions that use Azure Queue Storage queues|[Tutorial: Work with Azure storage](https://docs.microsoft.com/en-us/azure/storage/queues/storage-tutorial-queues?toc=%2Fazure%2Fstorage%2Fqueues%2Ftoc.json&tabs=dotnet)|

## Additional 
- [Azure Charts](https://azurecharts.com/highlights)
