﻿using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EFGetStarted;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }

    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, $"{nameof(EFGetStarted)}.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Blog
{
    public int BlogId { get; set; }

    public string? Url { get; set; }

    public List<Post> Posts { get; } = new();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class Post
{
    public int PostId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int BlogId { get; set; }

    [JsonIgnore]
    public Blog? Blog { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}