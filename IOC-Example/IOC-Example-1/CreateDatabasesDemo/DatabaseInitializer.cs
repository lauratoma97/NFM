using System.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;

namespace CreateDatabasesDemo;

public class DatabaseInitializer
{
    public void InitializeSqlite(string fileName)
    {
        Console.WriteLine("Starting SQLite database initialization...");

        var baseDir = GetBaseDir();
        var dbPath = Path.Combine(baseDir, fileName);
        Console.WriteLine($"SQLite database file: {dbPath}");

        var connectionString = $"Data Source={dbPath}";
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        Console.WriteLine("Connected to SQLite database.");

        // Create table if it doesn't exist
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS People (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    BirthDay TEXT NOT NULL
                );";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Created 'People' table if it did not exist (SQLite).");
        }

        // Insert records
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT INTO People (Name, BirthDay) VALUES
                ('Alice de la Sqlite', '1990-01-01'),
                ('Bob de la Sqlite', '1985-05-15'),
                ('Charlie de la Sqlite', '2000-12-31');";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Inserted sample records into 'People' table (SQLite).");
        }

        Console.WriteLine("SQLite database initialization complete.");
        Console.WriteLine();
        Console.WriteLine();
    }

    public void InitializeSqlServer(string connectionString)
    {
        Console.WriteLine("Starting SQL Server database initialization...");

        using var conn = new SqlConnection(connectionString);
        conn.Open();
        Console.WriteLine("Connected to SQL Server database.");

        // Create table if it doesn't exist
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                IF OBJECT_ID('People', 'U') IS NULL
                CREATE TABLE People (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(100) NOT NULL,
                    BirthDay TEXT NOT NULL
                );";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Created 'People' table if it did not exist (SQL Server).");
        }

        // Insert records
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = @"
                INSERT INTO People (Name, BirthDay) VALUES
                ('Alice sqlserver', '1990-01-01'),
                ('Bob sqlserver', '1985-05-15'),
                ('Charlie sqlserver', '2000-12-31');";
            cmd.ExecuteNonQuery();
            Console.WriteLine("Inserted sample records into 'People' table (SQL Server).");
        }

        Console.WriteLine("SQL Server database initialization complete.");
        Console.WriteLine();
        Console.WriteLine();
    }

    private string GetBaseDir()
    {
        var directory = Directory.GetCurrentDirectory();
        return directory.Substring(0, directory.IndexOf(@"\CreateDatabasesDemo\", StringComparison.Ordinal) + @"\CreateDatabasesDemo\".Length);
    }
}