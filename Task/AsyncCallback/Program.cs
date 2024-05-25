namespace AsyncCallback;

class Program
{
    static void CallCallback(Action callback)
    {
        Console.Error.WriteLine("CallCallback Begin");
        callback();
        Console.Error.WriteLine("CallCallback End");
    }

    static void WaitCallback(Func<Task> callback)
    {
        Console.Error.WriteLine("WaitCallback Begin");
        callback().Wait();
        Console.Error.WriteLine("WaitCallback End");
    }

    static void Main(string[] args)
    {
        Action cb = async () =>
        {
            Console.Error.WriteLine("Callback Begin");
            await Task.Delay(2000);
            Console.Error.WriteLine("Callback End");
        };
        CallCallback(cb);

        Console.Error.WriteLine("-------------------------------");

        Func<Task> cb2 = async () =>
        {
            Console.Error.WriteLine("Callback 2 Begin");
            await Task.Delay(1000);
            Console.Error.WriteLine("Callback 2 End");
        };
        WaitCallback(cb2);

        Console.Error.WriteLine("-------------------------------");
        Console.Error.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }
}
