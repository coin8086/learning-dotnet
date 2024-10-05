namespace DiBasics;

interface IServiceX<T> : IChecker
{
}

class ServiceX<T> : Checker, IServiceX<T>
{
    public override void Check(int indent = 0)
    {
        var leading = new string(' ', indent);
        Console.WriteLine($"{leading}{GetType().Name}<{typeof(T).Name}>: {this.GetHashCode()}");
    }
}
