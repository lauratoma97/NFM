# NFM

````
dotnet ef migrations add InitialCreate --project NFM.Domain --startup-project NFM.WebApi 
dotnet tool update --global dotnet-ef
dotnet ef database update --project NFM.Domain --startup-project NFM.WebApi
````