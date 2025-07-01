// See https://aka.ms/new-console-template for more information
using CreateDatabasesDemo;

Console.WriteLine("Creating database for demo");


var instance = new DatabaseInitializer();
instance.InitializeSqlite("demo-sqlite.db");
instance.InitializeSqlServer("Server=localhost;Database=DemoIoc;Trusted_Connection=True;TrustServerCertificate=true");
