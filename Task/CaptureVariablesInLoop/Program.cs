namespace CaptureVariablesInLoop;

class Program
{
    static void Main(string[] args)
    {
        var tasks = new Task[3];
        var strs = new string[3] { "I'm", "OK", "Thanks!" };
        for (int i = 0; i < 3; i++)
        {
            var j = i;
            var x = strs[i];
            tasks[i] = Task.Run(() =>
            {
                Console.WriteLine($"{i}, {j}, {x}");
            });
        }
        Task.WaitAll(tasks);
    }
}
