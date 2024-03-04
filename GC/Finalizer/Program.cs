//See https://learn.microsoft.com/en-us/dotnet/api/system.object.finalize
//and https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-object-finalize

using System.Diagnostics;

public class ExampleClass
{
    Stopwatch sw;

    public ExampleClass()
    {
        sw = Stopwatch.StartNew();
        Console.WriteLine("Instantiated object");
    }

    public void ShowDuration()
    {
        Console.WriteLine("This instance of {0} has been in existence for {1}",
                          this, sw.Elapsed);
    }

    //The finalizer doesn't seem to get called in a console app in .Net 8?
    ~ExampleClass()
    {
        Console.WriteLine("Destructing object");
        sw.Stop();
        Console.WriteLine("This instance of {0} has been in existence for {1}",
                          this, sw.Elapsed);
    }

    //The compiler doesn't allow such a method:
    //protected override void Finalize()
    //{
    //    Console.WriteLine("Finalizing object");
    //    sw.Stop();
    //    Console.WriteLine("This instance of {0} has been in existence for {1}",
    //                      this, sw.Elapsed);
    //}
}

public class Demo
{
    public static void Main()
    {
        ExampleClass ex = new ExampleClass();
        ex.ShowDuration();
    }
}
