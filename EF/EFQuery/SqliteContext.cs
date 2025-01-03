﻿using EFQuery.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Text;

namespace EFQuery;

public class SqliteContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public SqliteContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, $"{nameof(EFQuery)}.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}").LogTo(Console.WriteLine, LogLevel.Information);
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"DB path: {DbPath}.");
        builder.AppendLine(Model.ToDebugString(MetadataDebugStringOptions.LongDefault));
        return builder.ToString();
    }
}
