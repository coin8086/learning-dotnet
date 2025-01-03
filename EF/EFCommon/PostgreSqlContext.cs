using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCommon;

public class PostgreSqlContext : CommonDbContext
{
    private Version? Version { get; set; }

    public PostgreSqlContext(string dbName, LogLevel logLevel = LogLevel.Information) : base(dbName, logLevel)
    {
        var connStr = Environment.GetEnvironmentVariable("PG_CONNECTION");
        if (string.IsNullOrWhiteSpace(connStr))
        {
            throw new ArgumentException("Environment variable PG_CONNECTION must be set!");
        }
        //Suppose this overrides the Database in connStr if any
        ConnectionString = connStr + $";Database={dbName}";

        var verStr = Environment.GetEnvironmentVariable("PG_VERSION");  //The version string is in the form "x.y", or "x.y.z".
        if (!string.IsNullOrWhiteSpace(verStr))
        {
            Version = new Version(verStr);
        }
    }

    public PostgreSqlContext(string dbName, string connStr, string? version = null, LogLevel logLevel = LogLevel.Information)
        : base(dbName, logLevel)
    {
        //Suppose this overrides the Database in connStr if any
        ConnectionString = connStr + $";Database={dbName}";
        if (version != null)
        {
            Version = new Version(version);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(ConnectionString, options => options.SetPostgresVersion(Version))
            .LogTo(Console.WriteLine, LogLevel);
    }
}
