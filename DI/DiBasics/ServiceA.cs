namespace DiBasics;

interface IServiceA
{
    void Speak();
}

class ServiceA : IServiceA
{
    public void Speak()
    {
        Console.WriteLine($"{ToString()}: {this.GetHashCode()}");
    }
}
