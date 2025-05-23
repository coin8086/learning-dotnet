﻿//See https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/projection-operations
//and https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/join-operations
//and https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/grouping-data

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

    /*
     * NOTE
     *
     * SelectMany operator can behave as a "CROSS JOIN" (as in SelectMany2), an "INNER JOIN" (as in SelectMany3),
     * or a "LEFT JOIN" (as in SelectMany4). And it is translated into one of the SQL joins by EF Core. See
     * https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators#selectmany
     */
    static void SelectMany3()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from student in Student.Collection
                from department in Department.Collection.Where(dep => dep.ID == student.DepartmentID)
                select new { student.Name, Department = department.Name };
        }
        else
        {
            query =
                Student.Collection.SelectMany(
                    student => Department.Collection.Where(dep => dep.ID == student.DepartmentID),
                    (student, department) => new { student.Name, Department = department.Name });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item);
        }
    }

    /*
     * NOTE
     *
     * The only difference between SelectMany3 and SelectMany4 is the call to "DefaultIfEmpty()".
     */
    static void SelectMany4()
    {
        dynamic query;

        if (!_useMethod)
        {
            query =
                from student in Student.Collection
                from department in Department.Collection.Where(dep => dep.ID == student.DepartmentID).DefaultIfEmpty()
                    /*
                     * NOTE
                     *
                     * The compiler can not deduce that department is nullable here, while it can do when using SelectMany
                     * method. So it seems the method way is better than clause in compiler checking.
                     */
                select new { student.Name, Department = department?.Name ?? "(null)" };
        }
        else
        {
            query =
                Student.Collection.SelectMany(
                    student => Department.Collection.Where(dep => dep.ID == student.DepartmentID).DefaultIfEmpty(),
                    (student, department) => new { student.Name, Department = department?.Name ?? "(null)" });
        }

        foreach (var item in query)
        {
            Console.WriteLine(item);
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
     * This is a pattern to generate "left join" SQL in EF. See more about it at
     * https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators#left-join
     */
    static void GroupJoin3()
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

    static void GroupBy()
    {
        IEnumerable<IGrouping<int, Student>> query;

        if (!_useMethod)
        {
            query =
                from student in Student.Collection
                group student by student.DepartmentID into sgroup
                orderby sgroup.Key
                select sgroup;
        }
        else
        {
            query = Student.Collection.GroupBy(student => student.DepartmentID).OrderBy(group => group.Key);
        }

        foreach (var group in query)
        {
            Console.WriteLine($"Department: {group.Key}");

            foreach (var student in group)
            {
                Console.WriteLine($"  {student}");
            }
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

        Console.WriteLine("\n---------- SelectMany2 (Cross Join) ----------");

        SelectMany2();

        Console.WriteLine("\n---------- SelectMany3 (Inner Join) ----------");

        SelectMany3();

        Console.WriteLine("\n---------- SelectMany4 (Left Join Pattern) ----------");

        SelectMany4();

        Console.WriteLine("\n---------- Join (Inner Join) ----------");

        Join();

        Console.WriteLine("\n---------- GroupJoin ----------");

        GroupJoin();

        Console.WriteLine("\n---------- GroupJoin2 ----------");

        GroupJoin2();

        Console.WriteLine("\n---------- GroupJoin3 (Left Join Pattern) ----------");

        GroupJoin3();

        Console.WriteLine("\n---------- GroupBy ----------");

        GroupBy();
    }
}
