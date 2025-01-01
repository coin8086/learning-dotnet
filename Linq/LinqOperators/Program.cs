//See https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/projection-operations
//and https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/join-operations

namespace LinqOperators;

class Program
{
    static bool _useMethod = false;

    static void SelectMany()
    {
        var phrases = new string[] { "an apple a day", "the quick brown fox" };
        IEnumerable<string> query;

        if (!_useMethod)
        {
            query =
                from p in phrases
                from w in p.Split(" ")
                select w;
        }
        else
        {
            query = phrases.SelectMany(p => p.Split(" "));
        }

        foreach (var w in query)
        {
            Console.WriteLine(w);
        }
    }

    static void SelectMany2()
    {
        var numbers = new int[] { 1, 2, 3 };
        var letters = new char[] { 'a', 'b', 'c' };
        IEnumerable<(int, char)> query;

        if (!_useMethod)
        {
            query =
                from n in numbers
                from c in letters
                select (n, c);
        }
        else
        {
            query = numbers.SelectMany(n => letters, (n, c) => (n, c));
        }

        foreach (var (n, c) in query)
        {
            Console.WriteLine($"{n},{c}");
        }
    }

    static void Join()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from s in Student.Collection
                join d in Department.Collection on s.DepartmentID equals d.ID
                select new { s.Name, Department = d.Name };
        }
        else
        {
            query = 
                Student.Collection.Join(
                    Department.Collection,
                    s => s.DepartmentID,
                    d => d.ID, 
                    (s, d) => new { s.Name, Department = d.Name });
        }

        foreach (var s in query)
        {
            Console.WriteLine(s);
        }
    }

    static void GroupJoin()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from d in Department.Collection
                join s in Student.Collection on d.ID equals s.DepartmentID into sgroup
                select new { d.Name, Students = sgroup };
        }
        else
        {
            query = 
                Department.Collection.GroupJoin(
                    Student.Collection,
                    d => d.ID,
                    s => s.DepartmentID,
                    (d, sgroup) => new { d.Name, Students = sgroup });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item.Name);

            foreach (var s in item.Students)
            {
                Console.WriteLine($"  {s}");
            }
        }
    }

    static void GroupJoin2()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from s in Student.Collection
                join d in Department.Collection on s.DepartmentID equals d.ID into dgroup
                select new { s.Name, Department = dgroup.SingleOrDefault() };
        }
        else
        {
            query = 
                Student.Collection.GroupJoin(
                    Department.Collection,
                    s => s.DepartmentID,
                    d => d.ID,
                    (s, dgroup) => new { s.Name, Department = dgroup.SingleOrDefault() });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    /*
     * NOTE
     *
     * This is a mandatory pattern to generate "left join" SQL in EF.
     * See more at https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators#left-join
     */
    static void LeftJoin()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from s in Student.Collection
                join d in Department.Collection on s.DepartmentID equals d.ID into dgroup
                from dep in dgroup.DefaultIfEmpty()
                select new { s.Name, Department = dep?.Name ?? "(null)" };
        }
        else
        {
            query = 
                Student.Collection.GroupJoin(
                    Department.Collection,
                    s => s.DepartmentID,
                    d => d.ID,
                    (s, dgroup) => new { Student = s, Group = dgroup })
                .SelectMany(
                    item => item.Group.DefaultIfEmpty(),
                    (item, d) => new { Name = item.Student.Name, Department = d?.Name ?? "(null)" });
        }

        foreach (var s in query)
        {
            Console.WriteLine(s);
        }
    }

    static void Main(string[] args)
    {
        if (args.Length > 0 && "-m".Equals(args[0]))
        {
            _useMethod = true;
        }

        SelectMany();

        Console.WriteLine("--------------------");

        SelectMany2();

        Console.WriteLine("--------------------");

        Join();

        Console.WriteLine("--------------------");

        GroupJoin();

        Console.WriteLine("--------------------");

        GroupJoin2();

        Console.WriteLine("--------------------");

        LeftJoin();
    }
}
