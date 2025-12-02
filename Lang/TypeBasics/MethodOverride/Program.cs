namespace MethodOverride;

#region Interface method override

interface IHello
{
    void Say();
}

class Foo : IHello
{
    public virtual void Say()
    {
        Console.WriteLine("Hello!");
    }
}

class Foo2 : Foo
{
    /*
     * Try remove "override" and see the differences
     */
    public override void Say()
    {
        Console.WriteLine("Hi!");
    }
}

#endregion

#region Abstract method override

abstract class AbstractHello
{
    public abstract void Say();
}

class Bar : AbstractHello
{
    /*
     * Here "override" is required, meaning "abstract" is like "virtual" in base class.
     */
    public override void Say()
    {
        throw new NotImplementedException();
    }
}

#endregion

class Program
{
    static void Main(string[] args)
    {
        IHello hello = new Foo2();
        hello.Say();

        Foo2 foo = new Foo2();
        foo.Say();
    }
}
