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
        var letters = new char[] { 'a', 'b' };
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
                from student in Student.Collection
                join department in Department.Collection on student.DepartmentID equals department.ID
                select new { student.Name, Department = department.Name };
        }
        else
        {
            query = 
                Student.Collection.Join(
                    Department.Collection,
                    student => student.DepartmentID,
                    department => department.ID,
                    (student, department) => new { student.Name, Department = department.Name });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void GroupJoin()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from department in Department.Collection
                join student in Student.Collection on department.ID equals student.DepartmentID into sgroup
                select new { department.Name, Students = sgroup };
        }
        else
        {
            query = 
                Department.Collection.GroupJoin(
                    Student.Collection,
                    department => department.ID,
                    student => student.DepartmentID,
                    (department, sgroup) => new { department.Name, Students = sgroup });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item.Name);

            foreach (var student in item.Students)
            {
                Console.WriteLine($"  {student}");
            }
        }
    }

    static void GroupJoin2()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from student in Student.Collection
                join department in Department.Collection on student.DepartmentID equals department.ID into dgroup
                select new { student.Name, Department = dgroup.SingleOrDefault()?.Name ?? "(null)" };
        }
        else
        {
            query = 
                Student.Collection.GroupJoin(
                    Department.Collection,
                    student => student.DepartmentID,
                    department => department.ID,
                    (student, dgroup) => new { student.Name, Department = dgroup.SingleOrDefault()?.Name ?? "(null)" });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    /*
     * NOTE
     *
     * This is a "mandatory" pattern to generate "left join" SQL in EF. See more about it at
     * https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators#left-join
     */
    static void LeftJoin()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from student in Student.Collection
                join department in Department.Collection on student.DepartmentID equals department.ID into dgroup
                from dep in dgroup.DefaultIfEmpty()
                select new { student.Name, Department = dep?.Name ?? "(null)" };
        }
        else
        {
            query = 
                Student.Collection.GroupJoin(
                    Department.Collection,
                    student => student.DepartmentID,
                    department => department.ID,
                    (student, dgroup) => new { Student = student, Group = dgroup })
                .SelectMany(
                    item => item.Group.DefaultIfEmpty(),
                    (item, department) => new { Name = item.Student.Name, Department = department?.Name ?? "(null)" });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    static void Main(string[] args)
    {
        if (args.Length > 0 && "-m".Equals(args[0]))
        {
            _useMethod = true;
        }

        Console.WriteLine("\n---------- SelectMany ----------");

        SelectMany();

        Console.WriteLine("\n---------- SelectMany2 ----------");

        SelectMany2();

        Console.WriteLine("\n---------- Join ----------");

        Join();

        Console.WriteLine("\n---------- GroupJoin ----------");

        GroupJoin();

        Console.WriteLine("\n---------- GroupJoin2 ----------");

        GroupJoin2();

        Console.WriteLine("\n---------- LeftJoin ----------");

        LeftJoin();
    }
}
