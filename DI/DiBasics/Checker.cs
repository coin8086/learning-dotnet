namespace DiBasics;

class Checker : IChecker
{
    public virtual void Check(int indent = 0)
    {
        var leading = new string(' ', indent);
        Console.WriteLine($"{leading}{GetType().Name}: {this.GetHashCode()}");
    }
}
