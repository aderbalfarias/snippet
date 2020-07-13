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
|implement partitioning schemes||
|interact with data using the appropriate SDK||
|set the appropriate consistency level for operations||
|create Cosmos DB containers||
|implement scaling (partitions, containers) ||
|implement server-side programming including stored procedures, triggers, and change feed notifications||
|<b>Develop solutions that use blob storage</b>||
|move items in Blob storage between storage accounts or containers||
|set and retrieve properties and metadata||
|interact with data using the appropriate SDK||
|implement data archiving and retention||
|implement hot, cool, and archive storage||

## 3. Implement Azure security (15-20%)
| Topic | Links |
| ---- | ----------- |
|<b>Implement user authentication and authorization</b>||
|implement OAuth2 authentication||
|create and implement shared access signatures||
|register apps and use Azure Active Directory to authenticate users||
|control access to resources by using role-based access controls (RBAC)||
|<b>Implement secure cloud solutions</b>||
|secure app configuration data by using the App Configuration and KeyVault API||
|manage keys, secrets, and certificates by using the KeyVault API||
|implement Managed Identities for Azure resources||

## 4. Monitor, troubleshoot, and optimize Azure solutions (10-15%)
| Topic | Links |
| ---- | ----------- |
|<b>Integrate caching and content delivery within solutions</b>||
|develop code to implement CDNs in solutions||
|configure cache and expiration policies for FrontDoor, CDNs, or Redis caches Store and retrieve data in Azure Redis cache||
|<b>Instrument solutions to support monitoring and logging</b>||
|configure instrumentation in an app or service by using Application Insights||
|analyze log data and troubleshoot solutions by using Azure Monitor||
|implement Application Insights Web Test and Alerts||
|implement code that handles transient faults||

## 5. Connect to and consume Azure services and third-party services (25-30%)
| Topic | Links |
| ---- | ----------- |
|<b>Develop an App Service Logic App</b>||
|create a Logic App||
|create a custom connector for Logic Apps||
|create a custom template for Logic Apps||
|<b>Implement API Management</b>||
|create an APIM instance||
|configure authentication for API||
|define policies for APIs||
|<b>Develop event-based solutions</b>||
|implement solutions that use Azure Event Grid||
|implement solutions that use Azure Notification Hubs||
|implement solutions that use Azure Event Hub||
|<b>Develop message-based solutions</b>||
|implement solutions that use Azure Service Bus||
|implement solutions that use Azure Queue Storage queues||

## Additional 
- [Practice test](https://www.examtopics.com/exams/microsoft/az-204/)
- [Azure Charts](https://azurecharts.com/highlights)
