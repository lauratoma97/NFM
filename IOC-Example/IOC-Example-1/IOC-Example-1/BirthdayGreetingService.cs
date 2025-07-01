
namespace IOC_Example_1;

public class BirthdayGreetingService
{
    public void PrintBirthdayGreetings()
    {
        // 1. Using SqlServerDbContext
        //var context = new SqlServerDbContext("Server=localhost;Database=DemoIoc;Trusted_Connection=True;TrustServerCertificate=true");

        // 2. Using SqliteDbContext
        var context = new SqliteDbContext("D:\\1.WORK2\\Summer-practice\\IOC-Example-1\\IOC-Example-1\\CreateDatabasesDemo\\demo-sqlite.db");

        var people = context.GetPeople();
        var greetings = people
            .Select(p => $"Happy Birthday, {p.Name}!")
            .ToList();

        var text = string.Join(Environment.NewLine, greetings);
        Console.WriteLine("Result is:");
        Console.WriteLine(text);
    }
}