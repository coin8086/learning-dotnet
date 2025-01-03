using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCommon;

public class SqliteContext : CommonDbContext
{
    public SqliteContext(string dbName, LogLevel logLevel = LogLevel.Information) : base(dbName, logLevel)
    {
        var dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var dbFilePath = Path.Join(dir, $"{dbName}.db");
        ConnectionString = $"Data Source={dbFilePath}";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(ConnectionString).LogTo(Console.WriteLine, LogLevel);
    }
}
