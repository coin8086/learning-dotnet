using System.Collections.Concurrent;

namespace UsingConcurrentBag;

class Program
{
    static void Main(string[] args)
    {
        var bag = new ConcurrentBag<int>();
        bag.Add(1);
        bag.Add(1);
        bag.Add(2);

        var numbers = bag.ToArray();
        for (var i = 0; i < numbers.Length; i++)
        {
            Console.WriteLine(numbers[i]);
        }
    }
}
