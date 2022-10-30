<#
Install-Module Az.Resources -Scope CurrentUser -Repository PSGallery -Force -AllowClobber
npm i -g azure-functions-core-tools@4 --unsafe-perm true
#>
$userIdObj = $null # '660b1851-e4aa-475f-bfa2-2bf9671d5261'
if ($null -eq $userIdObj){
    $userId = az login
    if ($userId[0] -eq "[") { $userId[0]=[String]::Empty }
    if ($userId[$userId.Count-1] -eq "]") {$userId[$userId.Count-1] = "" }
    $userIdObj = ConvertFrom-Json ([String]::Join('', $userId).ToString())
} else {
    $userIdObj.id = $userIdObj
}
$NewId = [guid]::NewGuid().ToString().Substring(0,5)

do {
    $projectName = Read-Host -Prompt "Please provide a name to your project (only alphanumeric an hyphens allowed), hit enter to default: gr-taxinfo"
    $projectName = $projectName.ToLower()
    $projectNameInvalid = ($projectName -notmatch "^[a-z0-9]+[a-z0-9\-]*$") || ($projectName -match "[-]{2}")
    if ($projectNameInvalid)
    {
        Write-Error "Please make sure you are following the azure naming rules: https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/resource-name-rules"
    }
} while ($projectNameInvalid)
$rgName = ("rg-$projectName-"+$NewId)
# $vaultName = ("$projectName-Vault-"+$NewId)

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

Write-Host "Creating new Resource Group: $rgName" -ForegroundColor Blue
$group = az group create --name $rgName --location westeurope
$groupObj = ConvertFrom-Json ([String]::Join("", $group).ToString())
Write-Host "Created new Resource Group: $($groupObj.name)" -ForegroundColor Green

Write-Host "Creating new Resource: $vaultName using id:$($userIdObj.id)"
$groupObj_name=($groupObj.name)
$userIdObj_id=($userIdObj.id).ToString().Trim()
# $vault = 

az deployment group create --resource-group $groupObj_name --template-file gr-taxinfo-infra.bicep `
    --parameters OwnerId=$userIdObj_id projectName=$projectName 

Write-Host "Created new Resources: (TODO)"

<# to remove everything
Get-AzResourceGroup|Where-Object {$_.ResourceGroupName -like 'DeleteMe*'}| Remove-AzResourceGroup -Confirm:$false -Force

$functionExistsResponse = Invoke-WebRequest -Uri "https://gr-gettaxinfo-none.azurewebsites.net/"

#>