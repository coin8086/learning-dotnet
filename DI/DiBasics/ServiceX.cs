namespace DiBasics;

interface IServiceX<T>
{
    void Log();
}

class ServiceX<T> : IServiceX<T>
{
    public void Log()
    {
        Console.WriteLine($"{ToString()}: {this.GetHashCode()}");
    }
}
