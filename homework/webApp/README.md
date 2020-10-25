dotnet aspnet-codegenerator controller -name DiffieHellmanController -actions -m DiffieHellmanClass -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name RSAController -actions -m RSAClass -dc ApplicationDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f

dotnet ef migrations add InitialDbCreation
dotnet ef migrations update
dotnet ef migrations remove

dotnet ef database drop
rm -f ./Data/Migrations/
dotnet ef migrations add InitialDbCreation
dotnet ef database update
