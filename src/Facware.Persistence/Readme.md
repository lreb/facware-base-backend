#

## CLI

https://docs.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools


## add initial migration

```
dotnet ef migrations add init --startup-project ../Facware.Api --context ApplicationDbContext --verbose
```

## Add migration

```
dotnet ef migrations add demo --startup-project ../Facware.Api --context ApplicationDbContext --verbose
```

## Update

```
dotnet ef database update --startup-project ../Facware.Api --context ApplicationDbContext --verbose
```



dotnet ef migrations add init --startup-project ../Facware.Api --verbose --context ApplicationDbContext

dotnet ef migrations add init --startup-project ../Facware.Api --verbose --context ApplicationDbContext

dotnet ef database update --startup-project ../Facware.Api --verbose --context ApplicationDbContext

Add-Migration InitialDatabaseCreate

Add-Migration AddEmailInCustomers


Update-Database


dotnet ef database update -- --environment Production