﻿using Microsoft.EntityFrameworkCore;
using EFMigration.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Text;

namespace EFMigration.Data;

public class SqliteContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public string DbPath { get; }

    public SqliteContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, $"{nameof(EFMigration)}.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }


    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"DB path: {DbPath}.");
        builder.AppendLine(Model.ToDebugString(MetadataDebugStringOptions.LongDefault));
        return builder.ToString();
    }
}
