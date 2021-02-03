

dotnet ef migrations add init --startup-project ../Facware.Api --verbose --context ApplicationDbContext

dotnet ef database update --startup-project ../Facware.Api --verbose --context ApplicationDbContext

Add-Migration InitialDatabaseCreate

Add-Migration AddEmailInCustomers


Update-Database