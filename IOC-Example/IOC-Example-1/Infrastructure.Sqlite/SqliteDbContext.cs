using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.Data.Sqlite;

namespace Infrastructure.Sqlite;

// SQLite context
public class SqliteDbContext : IDbContext
{
    private readonly string _connectionString;

    public SqliteDbContext(DemoConfig config)
    {
        _connectionString = $"Data Source={config.SqlLiteDbFile}";
    }

    public List<Person> GetPeople()
    {
        var people = new List<Person>();
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = new SqliteCommand("SELECT Id, Name, BirthDay FROM People", conn);
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
