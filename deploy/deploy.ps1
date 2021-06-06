#!/usr/local/bin/pwsh -File

Param(
  [parameter(Mandatory=$true)][string]$ConfigurationFile    
)

#read in the function appsettings file
if ( -not (Test-Path -Path $ConfigurationFile -PathType Leaf)){
  Write-Host "Cannot find the specified configruation file at $ConfigurationFile!" -ForegroundColor Red
  exit
}
$devConfig = Get-Content -Path $ConfigurationFile | ConvertFrom-Json

Write-Output $devConfig
$RESOURCE_OWNER=$devConfig.ResourceOwner
$RESOURCE_GROUP_NAME=$($devConfig.ResourceGroupPrefix+$devConfig.FunctionAppName) 
$RESOURCE_LOCATION=$devConfig.ResourceLocation
$STORAGE_NAME=$($devConfig.FunctionAppName.Replace('-', '')+$devConfig.StorageSuffix)
$FUNCTION_APP_NAME=$($devConfig.FunctionAppName+$devConfig.FunctionAppSuffix)
$FUNCTION_PLAN_NAME=$($devConfig.FunctionAppName+$devConfig.FunctionAppPlanSuffix)
$APPINSIGHTS_NAME=$($devConfig.FunctionAppName+$devConfig.AppInsightsSuffix)
$CANDIDATE_QUEUE=$devConfig.CandidateQueue
$CANDIDATE_HTML_QUEUE=$devConfig.CandidateHtmlQueue
$CANDIDATE_TABLE=$devConfig.CandidateTable
$RESUMES_CONTAINER=$devConfig.ResumesContainer

az account set --subscription $devConfig.AzureSubscription

az account list -o table 

Start-Sleep -Seconds 2

az group create -n $RESOURCE_GROUP_NAME -l $RESOURCE_LOCATION --tags Owner="$RESOURCE_OWNER" 

az storage account create -n $STORAGE_NAME `
                          -l $RESOURCE_LOCATION  `
                          -g $RESOURCE_GROUP_NAME `
                          --sku Standard_LRS `
                          --tags Owner="$RESOURCE_OWNER"

 # Get the connection string 
 $CONN_STRING=$(az storage account show-connection-string -g $RESOURCE_GROUP_NAME -n $STORAGE_NAME -o tsv)

#Create resumes container 
az storage container create --account-name $STORAGE_NAME -n $RESUMES_CONTAINER --connection-string $CONN_STRING

#Create candidates queue
az storage queue create --account-name $STORAGE_NAME --name $CANDIDATE_QUEUE --connection-string $CONN_STRING

#Create candidates-html queue
az storage queue create --account-name $STORAGE_NAME --name $CANDIDATE_HTML_QUEUE --connection-string $CONN_STRING

#Create candidates table 
az storage table create --account-name $STORAGE_NAME --name $CANDIDATE_TABLE --connection-string $CONN_STRING

# Create an app service plan for the function app 
az functionapp plan create -g $RESOURCE_GROUP_NAME -n $FUNCTION_PLAN_NAME --sku S1 --is-linux --tags Owner="$RESOURCE_OWNER"

# Create an app insights for the function app 
az resource create -n $APPINSIGHTS_NAME `
                   -g $RESOURCE_GROUP_NAME `
                   --resource-type "Microsoft.Insights/components" `
                   --properties '{\"Application_Type\":\"web\"}' 

$APPINSIGHTS_KEY = $(az resource show -g $RESOURCE_GROUP_NAME -n $APPINSIGHTS_NAME --resource-type "Microsoft.Insights/components" --query "properties.InstrumentationKey" -o tsv)

# Create the function app 
az functionapp create --name $FUNCTION_APP_NAME `
                      --os-type Linux `
                      --resource-group $RESOURCE_GROUP_NAME `
                      --runtime dotnet-isolated `
                      --functions-version $devConfig.FunctionsVersion `
                      --storage-account $STORAGE_NAME `
                      --plan $FUNCTION_PLAN_NAME `
                      --app-insights $APPINSIGHTS_NAME `
                      --app-insights-key $APPINSIGHTS_KEY`
                      --tags Owner="$RESOURCE_OWNER"

# Display the connection string to add to GitHub Actions secrets
az functionapp deployment list-publishing-profiles --name $FUNCTION_APP_NAME --resource-group $RESOURCE_GROUP_NAME --xml