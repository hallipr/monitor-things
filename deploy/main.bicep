param appName string
param location string = resourceGroup().location


resource website 'Microsoft.Compute/cloudServices@2022-04-04' = {
  location: location
  name: appName
}
