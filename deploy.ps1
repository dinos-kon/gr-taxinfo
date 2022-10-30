<#
Install-Module Az.Resources -Scope CurrentUser -Repository PSGallery -Force -AllowClobber
npm i -g azure-functions-core-tools@4 --unsafe-perm true
#>

#region Get the current (connected) user's Id from azure
$userIdObj = $null 
if ($null -eq $userIdObj){
    $userId = az login
    $userId = az ad signed-in-user show
    if ($userId[0] -eq "[") { $userId[0]=[String]::Empty }
    if ($userId[$userId.Count-1] -eq "]") {$userId[$userId.Count-1] = "" }
    $userIdObj = ConvertFrom-Json ([String]::Join('', $userId).ToString())
} else {
    $userIdObj.id = $userIdObj
}
#endregion 

#region Get a random suffix (useful while developing/debugging)
$NewId = [guid]::NewGuid().ToString().Substring(0,5)
#endregion 

#region Get the name of the project
do {
    $projectName = Read-Host -Prompt "Please provide a name to your project (only alphanumeric an hyphens allowed), hit enter to default: gr-taxinfo"
    $projectName = $projectName.ToLower()
    $projectNameInvalid = ($projectName -notmatch "^[a-z0-9]+[a-z0-9\-]*$") || ($projectName -match "[-]{2}")
    if ($projectNameInvalid)
    {
        Write-Error "Please make sure you are following the azure naming rules: https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/resource-name-rules"
    }
} while ($projectNameInvalid)
#endregion

#region construct the names of the resource group and all the conained resources

$rgName = ("rg-$projectName-"+$NewId)

#region Ensure the name is not used already
#ensure we get a response even if an error's returned
$response = try { 
    $functionUri = "https://$projectName-$NewId-func.azurewebsites.net"
    (Invoke-WebRequest -Uri $functionUri -ErrorAction SilentlyContinue -SkipHttpErrorCheck ).BaseResponse
    $statusCodeInt = [int]$response.BaseResponse.StatusCode
} catch [System.Net.WebException] { 
    Write-Verbose "An exception was caught: $($_.Exception.Message)"
    $_.Exception.Response 
    $statusCodeInt = $response.BaseResponse.StatusCode.Value__
} 
if ($statusCodeInt -eq 200){
    Write-Error "This project name may be in use by someone else. the url $functionUri is hiding something..."
    Exit-PSHostProcess
    exit
}
#endregion 

Write-Host "Creating new Resource Group: $rgName" -ForegroundColor Blue
$group = az group create --name $rgName --location westeurope
$groupObj = ConvertFrom-Json ([String]::Join("", $group).ToString())
Write-Host "Created new Resource Group: $($groupObj.name)" -ForegroundColor Green

Write-Host "Creating new Resource: $vaultName using id:$($userIdObj.id)"
$groupObj_name=($groupObj.name)
$userIdObj_id=($userIdObj.id).ToString().Trim()
# $vault = 

$out = az deployment group create --resource-group $groupObj_name --template-file .\gr-taxinfo-infra\gr-taxinfo-infra.bicep `
    --parameters OwnerId=$userIdObj_id projectName=$projectName theCallerTIN=$theCallerTIN | convertfrom-json 
    # | ForEach-Object properties | ForEach-Object outputs

Write-Host "Created new Resources: ($out)"

<# to remove everything
Get-AzResourceGroup|Where-Object {$_.ResourceGroupName -like 'rg-proj*'}| Remove-AzResourceGroup -Confirm:$false -Force

$functionExistsResponse = Invoke-WebRequest -Uri "https://gr-gettaxinfo-none.azurewebsites.net/"

#>

# Set-Location ..\gr-gettaxinfo
dotnet publish -c Release
$publishFolder = ".\gr-gettaxinfo\bin\release\net6.0\publish\"

# create the zip
$publishZip = "gr-gettaxinfo-publish.zip"
if(Test-path $publishZip) {Remove-item ($publishZip)}
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($publishFolder, $publishZip)

# deploy the zipped package
az functionapp deployment source config-zip `
 -g $rgName -n ($out.properties.outputs.functionObject.value.properties.name) --src $publishZip