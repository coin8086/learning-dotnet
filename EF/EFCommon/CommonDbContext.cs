using EFCommon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCommon;

public abstract class CommonDbContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Post> Posts { get; set; }

    public string? DbName { get; private set; }

    protected string? ConnectionString { get; set; }

    public LogLevel LogLevel { get; private set; }

    protected CommonDbContext(string dbName, LogLevel logLevel = LogLevel.Information)
    {
        DbName = dbName;
        LogLevel = logLevel;
    }
}
