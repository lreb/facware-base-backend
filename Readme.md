
# Facware Base

This project has the proposition to provide an API with common features used in a regular project, besides try to implement the last technology versions to provide a new solutions to common problems.

Be free to take this code as you wish.

## Integrations

the integrations are ready to use

## Authentication

you can use OKTA or JWT methods

### JWT

just enable this service in `startup.cs`

`services.ConfigureOkta(Configuration);`

remeber add your own setting values in appsettings[environment].json file

### OKTA

just enable this service in `startup.cs`

`services.ConfigureJwt(Configuration);`

## OData

Use this attribute `EnableQueryFromODataToAWS` instead `EnableQuery`

### AWS lambda

Create API build
`dotnet publish -c Release -o AWSLambda`

Handler
`FacwareBase.API::FacwareBase.API.LambdaEntryPoint::FunctionHandlerAsync`
