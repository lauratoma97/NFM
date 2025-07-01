namespace Domain;

public class BirthdayGreetingService
{
    private readonly IDbContext _dbContext;

    public BirthdayGreetingService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void PrintBirthdayGreetings()
    {
        var people = _dbContext.GetPeople();
        var greetings = people
            .Select(p => $"Happy Birthday, {p.Name}!")
            .ToList();

        var text = string.Join(Environment.NewLine, greetings);
        Console.WriteLine("Result is:");
        Console.WriteLine(text);
    }
    // ...
}