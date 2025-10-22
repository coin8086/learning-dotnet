using EFCommon;
using EFCommon.Models;
using Microsoft.EntityFrameworkCore;

namespace EFIsolationLevels;

class Program
{
    static PostgreSqlContext CreateDbContext()
    {
        return new PostgreSqlContext(nameof(EFIsolationLevels));
    }

    static void ReCreateDb(DbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    static void DirtyRead()
    {
        //Init database
        {
            using var dbContext = CreateDbContext();
            ReCreateDb(dbContext);

            var blog1 = new Blog()
            {
                Name = "blog1",
            };
            dbContext.Add(blog1);
            dbContext.SaveChanges();
        }

        Console.WriteLine("=================== Demo Dirty Read =====================");
        {
            using var event1 = new ManualResetEvent(false);
            using var event2 = new ManualResetEvent(false);
            using var event3 = new ManualResetEvent(false);

            var task1 = Task.Run(() =>
            {
                using var dbContext = CreateDbContext();

                //Transaction 1: Make an uncommitted change
                using var tx1 = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var item = dbContext.Blogs.First();
                Console.WriteLine($"Blog Name in TX 1: {item.Name}");
                item.Name = "new blog1";
                Console.WriteLine($"New Blog Name in TX 1: {item.Name}");
                dbContext.SaveChanges();

                event1.Set();

                event2.WaitOne();

                tx1.Rollback();

                event3.Set();
            });

            var task2 = Task.Run(() =>
            {
                using var dbContext = CreateDbContext();

                event1.WaitOne();

                //Transaction 2: Read the uncommitted changes
                //NOTE: PosgreSQL doesn't support ReadUncommitted and it actually behaves as ReadCommitted 
                using var tx2 = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                var dirtyReadItem = dbContext.Blogs.First();
                Console.WriteLine($"Blog Name in TX 2: {dirtyReadItem.Name}");

                event2.Set();

                event3.WaitOne();

                tx2.Commit();
            });

            Task.WaitAll(task1, task2);
        }

        {
            using var dbContext = CreateDbContext();
            var item = dbContext.Blogs.First();
            Console.WriteLine($"Blog Name after TX 1 and 2: {item.Name}");
        }
    }

    static void NonRepeatableRead()
    {
        //Init database
        {
            using var dbContext = CreateDbContext();
            ReCreateDb(dbContext);

            var blog1 = new Blog()
            {
                Name = "blog1",
            };
            dbContext.Add(blog1);
            dbContext.SaveChanges();
        }

        Console.WriteLine("=================== Demo NonRepeatable Read =====================");
        {
            using var event1 = new ManualResetEvent(false);
            using var event2 = new ManualResetEvent(false);

            var task1 = Task.Run(() =>
            {
                using var dbContext = CreateDbContext();

                //Transaction 1: Read a value
                //NOTE: Change isolation level from ReadCommitted to RepeatableRead and see the differences.
                using var tx1 = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);  
                var item = dbContext.Blogs.AsNoTracking().First();
                Console.WriteLine($"Blog Name in TX 1: {item.Name}");

                event1.Set();

                event2.WaitOne();

                //Read the value again
                item = dbContext.Blogs.AsNoTracking().First();
                Console.WriteLine($"Blog Name in TX 1 again: {item.Name}");

                tx1.Rollback();
            });

            var task2 = Task.Run(() =>
            {
                using var dbContext = CreateDbContext();

                event1.WaitOne();

                //Transaction 2: Change the value when TX 1 is still open
                using var tx2 = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                var item = dbContext.Blogs.First();
                Console.WriteLine($"Blog Name in TX 2: {item.Name}");
                item.Name = "New Blog";
                Console.WriteLine($"Blog Name changed in TX 2: {item.Name}");
                dbContext.SaveChanges();
                tx2.Commit();

                event2.Set();
            });

            Task.WaitAll(task1, task2);
        }
    }

    static void PhantomRead()
    {
        //Init database
        {
            using var dbContext = CreateDbContext();
            ReCreateDb(dbContext);

            var blog1 = new Blog()
            {
                Name = "blog1",
            };
            var blog2 = new Blog()
            {
                Name = "blog2",
            };
            var blog3 = new Blog()
            {
                Name = "a blog",
            };
            dbContext.AddRange(blog1, blog2, blog3);
            dbContext.SaveChanges();
        }

        Console.WriteLine("=================== Demo Phantom Read =====================");
        {
            using var event1 = new ManualResetEvent(false);
            using var event2 = new ManualResetEvent(false);

            var task1 = Task.Run(() =>
            {
                using var dbContext = CreateDbContext();

                //Transaction 1: Count blogs whose name starting with 'b'
                using var tx1 = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.RepeatableRead);
                var num = dbContext.Blogs.Where(b => b.Name.StartsWith("b")).Count();
                Console.WriteLine($"Count blogs whose name starts with 'b' in TX 1: {num}");

                event1.Set();

                event2.WaitOne();

                //Read Count blogs whose name starting with 'b' again
                num = dbContext.Blogs.Where(b => b.Name.StartsWith("b")).Count();
                Console.WriteLine($"Count blogs whose name starts with 'b' in TX 1 again: {num}");

                tx1.Rollback();
            });

            var task2 = Task.Run(() =>
            {
                using var dbContext = CreateDbContext();

                event1.WaitOne();

                //Transaction 2: 
                using var tx2 = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);

                //NOTE: It seems PostgreSQL's RepeatableRead is so good that the typical operations don't show phantom read.
                //TO see the different, change the isolation level of tx 1 to ReadCommitted.

                //Add a new blog whose name starts with 'b'
                //var blog3 = new Blog()
                //{
                //    Name = "blog3",
                //};
                //dbContext.Blogs.Add(blog3);

                //Or delete blogs whose name starts with 'b'
                dbContext.Blogs.Where(b => b.Name.StartsWith("b")).ExecuteDelete();

                tx2.Commit();

                event2.Set();
            });

            Task.WaitAll(task1, task2);
        }
    }

    static void Main(string[] args)
    {
        DirtyRead();

        NonRepeatableRead();

        PhantomRead();
    }
}
