# DotNet CLI


add new class lib
```
dotnet new classlib -n Facware.Persistence -o .
```

include class lib in main sln

```
dotnet sln Facware.sln add Facware.Persistence/Facware.Persistence.csproj 
```
