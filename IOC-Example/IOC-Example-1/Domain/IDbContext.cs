namespace Domain;

public interface IDbContext
{
    List<Person> GetPeople();
}