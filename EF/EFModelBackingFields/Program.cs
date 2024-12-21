//See https://learn.microsoft.com/en-us/ef/core/modeling/backing-field?tabs=data-annotations

namespace EFModelBackingFields;

class Program
{
    static void Main(string[] args)
    {
        using var db = new SqliteContext();

        {
            var blog = new Blog() { Url = "abc" };
            var blogEntry = db.Add(blog);
            db.SaveChanges();

            var result = db.Blogs.Where(b => b.Id == blogEntry.Entity.Id).FirstOrDefault();
            Console.WriteLine(result);
        }

        {
            var blog = new Blog2();
            blog.SetUrl("abc");

            var blogEntry = db.Add(blog);
            db.SaveChanges();

            //blog.SetUrl("");

            var result = db.Blog2s.Where(b => b.Id == blogEntry.Entity.Id).FirstOrDefault();
            Console.WriteLine(result);
        }

        {
            var blog = new Blog3();
            blog.SetUrl("abc");

            var blogEntry = db.Add(blog);
            db.SaveChanges();

            //blog.SetUrl("");

            var result = db.Blog3s.Where(b => b.Id == blogEntry.Entity.Id).FirstOrDefault();
            Console.WriteLine(result);
        }

        {
            var blog = new Blog4();
            blog.SetUrl("abc");

            var blogEntry = db.Add(blog);
            db.SaveChanges();

            //blog.SetUrl("");

            var result = db.Blog4s.Where(b => b.Id == blogEntry.Entity.Id).FirstOrDefault();
            Console.WriteLine(result);
        }
    }
}
