@description('Specifies the name of the project. Will be used as prefix for resources.')
param projectName string = 'prj001001'

@description('Specifies the Azure location where the key vault should be created.')
param location string = resourceGroup().location

@description('Specifies whether Azure Virtual Machines are permitted to retrieve certificates stored as secrets from the key vault.')
param enabledForDeployment bool = false

@description('Specifies whether Azure Disk Encryption is permitted to retrieve secrets from the vault and unwrap keys.')
param enabledForDiskEncryption bool = false

@description('Specifies whether Azure Resource Manager is permitted to retrieve secrets from the key vault.')
param enabledForTemplateDeployment bool = false

@description('Specifies the Azure Active Directory tenant ID that should be used for authenticating requests to the key vault. Get it by using Get-AzSubscription cmdlet.')
param tenantId string = subscription().tenantId

@description('Specifies the object ID of a user, service principal or security group in the Azure Active Directory tenant for the vault. The object ID must be unique for the list of access policies. Get it by using Get-AzADUser or Get-AzADServicePrincipal cmdlets.')
param OwnerId string 

@description('Specifies the permissions to keys in the vault. Valid values are: all, encrypt, decrypt, wrapKey, unwrapKey, sign, verify, get, list, create, update, import, delete, backup, restore, recover, and purge.')
param keysPermissions array = [
  'list'
]

@description('Specifies the permissions to secrets in the vault. Valid values are: all, get, list, set, delete, backup, restore, recover, and purge.')
param secretsPermissions array = [
  'list'
  'get'
]

@description('Specifies whether the key vault is a standard vault or a premium vault.')
@allowed([
  'standard'
  'premium'
])
param skuName string = 'standard'

@description('The language worker runtime to load in the function app.')
@allowed([
  'node'
  'dotnet'
  'java'
])
param funcRuntime string = 'dotnet'

@description('Storage Account type')
@allowed([
  'Standard_LRS'
  'Standard_GRS'
  'Standard_RAGRS'
])
param storageAccountType string = 'Standard_LRS'

var storageAccountName = replace('${projectName}stor','-','')
var keyVaultName = '${projectName}-vault'
var hostingPlanName = '${projectName}-hosting'

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-08-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'Storage'
}

resource hostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

@description('The gsis username key (it should be \'gr-gsis-username\', unless changed in the function settings too)')
param usernameKey string = 'gr-gsis-username'

@description('Specify the username as it was declared in the gsis service.')
@sys.secure()
param usernameValue string

@description('The gsis password key (it should be \'gr-gsis-password\', unless changed in the function settings too)')
#disable-next-line secure-secrets-in-params
param passwordKey string = 'gr-gsis-password'

@description('Specify the password as it was declared in the gsis service.')
@secure()
param passwordValue string

resource theKeyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: keyVaultName
  location: location
  properties: {
    enabledForDeployment: enabledForDeployment
    enabledForDiskEncryption: enabledForDiskEncryption
    enabledForTemplateDeployment: enabledForTemplateDeployment
    tenantId: tenantId
    accessPolicies: [
      {
        objectId: OwnerId
        tenantId: tenantId
        permissions: {
          keys: keysPermissions
          secrets: secretsPermissions
        }
      }
    ]
    sku: {
      name: skuName
      family: 'A'
    }
    networkAcls: {
      defaultAction: 'Allow'
      bypass: 'AzureServices'
    }
    createMode: 'default'
    enableSoftDelete: false
  }
}

resource gsisUsername 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: theKeyVault
  name: usernameKey
  properties: {
    value: usernameValue
  }
}
resource gsisPassword 'Microsoft.KeyVault/vaults/secrets@2022-07-01' = {
  parent: theKeyVault
  name: passwordKey
  properties: {
    value: passwordValue
  }
}
