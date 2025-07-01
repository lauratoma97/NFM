using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace IOC_Example_1;

// SQLite context
public class SqliteDbContext
{
    private readonly string _connectionString;

    public SqliteDbContext(string dbFilePath)
    {
        _connectionString = $"Data Source={dbFilePath}";
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
