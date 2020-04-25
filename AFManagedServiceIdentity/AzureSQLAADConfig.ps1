az login
for
#This part is for manual deployment for your function (ZIP deployment)
$resourceGroupName = "azuresqldbdemorg"
$functionName = "AFSQLManagedServiceIdentityAppService"
$sourceZipPath = "C:\Users\StefanoDemiliani\Global Azure Bootcamp 2020-04-24\Code\AFManagedServiceIdentity\AFManagedServiceIdentity\bin\Release\netcoreapp3.1\publish\AFMSI.zip"

az webapp deployment source config-zip -g $resourceGroupName -n $functionName --src $sourceZipPath

#Show the service principal linked to the MSI Object ID
az ad sp show --id $objectID

#Azure SQL Login creation
$servername = "azuresqldbdemosrv";
$objectID = "dd3cde7e-2a82-4ed5-aadd-cc565dde75e7"; #Retrieved after MSI activation in Azure Function

#Create AD group and add the MSI to it
az ad group create --display-name SQLUsers --mail-nickname 'NotSet'
az ad group member add -g SQLUsers --member-id $objectID

#Add the SQLUsers group as admin in Azure SQL Server panel (Active Directory Admin --> Set Admin)