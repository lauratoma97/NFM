using System.Data.SqlClient;
using Domain;

namespace Infrastructure.SqlServer;

public class SqlServerDbContext : IDbContext
{
    private readonly string _connectionString;

    public SqlServerDbContext(DemoConfig config)
    {
        _connectionString = config.SqlServerConnectionString;
    }

    public List<Person> GetPeople()
    {
        var people = new List<Person>();
        using var conn = new SqlConnection(_connectionString);
        conn.Open();
        using var cmd = new SqlCommand("SELECT Id, Name, Birthday FROM People", conn);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            people.Add(new Person
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Birthday = reader.GetString(2)
            });
        }
        return people;
    }
}