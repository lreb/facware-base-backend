dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet --version 2.0.3
https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools.DotNet/

dotnet ef migrations add init --startup-project ../Facware.Api --verbose

dotnet ef migrations update --startup-project ../Facware.Api --verbose