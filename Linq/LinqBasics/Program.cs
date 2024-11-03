//See https://learn.microsoft.com/en-us/dotnet/csharp/linq/

namespace LinqBasics;

class Program
{
    static readonly IEnumerable<int> Numbers = Enumerable.Range(1, 10);

    static readonly char[] Chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

    static string RandomName(int length)
    {
        var chars = Random.Shared.GetItems(Chars, length);
        chars[0] = Char.ToUpper(chars[0]);
        return new string(chars);
    }

    static char ScoreClass(int score)
    {
        if (score >= 90)
            return 'A';
        if (score >= 80)
            return 'B';
        if (score >= 60)
            return 'C';
        return 'D';
    }

    static void UseQueryExpressions()
    {
        var students =
            from number in Numbers
            let score = new Random(number).Next(100)
            select new
            {
                ID = number,
                Name = RandomName(6),
                Score = score
            };

        foreach (var student in students)
        {
            Console.WriteLine(student);
        }

        Console.WriteLine("----------------");

        var qualifiedStudents =
            from student in students
            where student.Score >= 60
            orderby student.Score descending
            select student;

        foreach (var student in qualifiedStudents)
        {
            Console.WriteLine(student);
        }

        Console.WriteLine("----------------");

        var groups =
            from student in students
            group student by ScoreClass(student.Score) into Group
            orderby Group.Key ascending
            select Group;

        foreach (var group in groups)
        {
            Console.WriteLine(group.Key);
            foreach (var student in group)
            {
                Console.WriteLine($"  {student}");
            }
        }
    }

    static void UseQueryMethods()
    {
        var students = Numbers.Select(number => new
        {
            ID = number,
            Name = RandomName(6),
            Score = new Random(number).Next(100)
        });

        foreach (var student in students)
        {
            Console.WriteLine(student);
        }

        Console.WriteLine("----------------");

        var qualifiedStudents = students.Where(student => student.Score >= 60).OrderByDescending(student => student.Score);

        foreach (var student in qualifiedStudents)
        {
            Console.WriteLine(student);
        }

        Console.WriteLine("----------------");

        var groups = students.GroupBy(student => ScoreClass(student.Score)).OrderBy(group => group.Key);

        foreach (var group in groups)
        {
            Console.WriteLine(group.Key);
            foreach (var student in group)
            {
                Console.WriteLine($"  {student}");
            }
        }
    }

    static void Main(string[] args)
    {
        UseQueryExpressions();

        Console.WriteLine("========================================");

        UseQueryMethods();
    }
}
