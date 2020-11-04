# Facware Base

This project has the proposition to provide an API with common features used in a regular project, besides try to implement the last technology versions to provide a new solutions to common problems.

Be free to take this code as you wish.

## Integrations

the integrations are ready to use

## Authentication

you can use OKTA or JWT methods, just need change app settings configuration

### JWT

just enable this service in appsettings[environment].json like this

```json
 "Authentication":{
    "AuthenticationMode": "Jwt"
  },
```

remeber add your own setting values in appsettings[environment].json file

### OKTA

just enable this service in appsettings[environment].json like this

```json
 "Authentication":{
    "AuthenticationMode": "Okta"
  },
```

remeber add your own setting values in appsettings[environment].json file

## OData

Use this attribute `EnableQueryFromODataToAWS` instead `EnableQuery`

### AWS lambda

Create API build
`demo`

Handler
`FacwareBase.API::FacwareBase.API.LambdaEntryPoint::FunctionHandlerAsync`

#### Role permissions

get access to S3

`arn:aws:iam::aws:policy/AmazonS3FullAccess`

get access to execute lambda function

`arn:aws:iam::aws:policy/AWSLambdaExecute`
