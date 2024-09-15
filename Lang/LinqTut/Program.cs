//See https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/walkthrough-writing-queries-linq

using System.Net.Http.Headers;

namespace LinqTut;

public record Student(string First, string Last, int ID, int[] Scores);

class Program
{
    static void Main(string[] args)
    {
        IEnumerable<Student> students =
        [
            new Student(First: "Svetlana", Last: "Omelchenko", ID: 111, Scores: [97, 92, 81, 60]),
            new Student(First: "Claire",   Last: "O'Donnell",  ID: 112, Scores: [75, 84, 91, 39]),
            new Student(First: "Sven",     Last: "Mortensen",  ID: 113, Scores: [88, 94, 65, 91]),
            new Student(First: "Cesar",    Last: "Garcia",     ID: 114, Scores: [97, 89, 85, 82]),
            new Student(First: "Debra",    Last: "Garcia",     ID: 115, Scores: [35, 72, 91, 70]),
            new Student(First: "Fadi",     Last: "Fakhouri",   ID: 116, Scores: [99, 86, 90, 94]),
            new Student(First: "Hanying",  Last: "Feng",       ID: 117, Scores: [93, 92, 80, 87]),
            new Student(First: "Hugo",     Last: "Garcia",     ID: 118, Scores: [92, 90, 83, 78]),

            new Student("Lance",   "Tucker",      119, [68, 79, 88, 92]),
            new Student("Terry",   "Adams",       120, [99, 82, 81, 79]),
            new Student("Eugene",  "Zabokritski", 121, [96, 85, 91, 60]),
            new Student("Michael", "Tucker",      122, [94, 92, 91, 91])
        ];

        //IEnumerable<Student> studentQuery =
        //    from student in students
        //    where student.Scores[0] > 90
        //    orderby student.Scores[0] descending
        //    select student;

        //foreach (Student student in studentQuery)
        //{
        //    Console.WriteLine($"{student.Last}, {student.First} {student.Scores[0]}");
        //}

        //IEnumerable<IGrouping<char, Student>> studentQuery =
        //    from student in students
        //    group student by student.Last[0] into studentGroup
        //    orderby studentGroup.Key
        //    select studentGroup;

        //foreach (var studentGroup in studentQuery)
        //{
        //    Console.WriteLine(studentGroup.Key);
        //    foreach (Student student in studentGroup)
        //    {
        //        Console.WriteLine($"   {student.Last}, {student.First}");
        //    }
        //}

        var studentQuery5 =
            from student in students
            let avg = (decimal)student.Scores.Average()
            where avg < student.Scores[0]
            //select $"{student.Last}, {student.First}, {avg}, {student.Scores[0]}";
            select new { student.ID, Avg = avg };

        foreach (var s in studentQuery5)
        {
            Console.WriteLine(s);
        }
    }
}
