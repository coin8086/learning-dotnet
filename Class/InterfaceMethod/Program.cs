namespace InterfaceMethod;

interface IFoo
{
    //Not like a C++ interface method, the method Func is not "virtual".
    void Func();
}

class Foo : IFoo
{
    //So here the "virtual" modifier must be specified when a subclass needs to override it.
    public virtual void Func()
    {
        Console.WriteLine("Foo");
    }
}

class Bar : Foo
{
    public override void Func()
    {
        Console.WriteLine("Bar");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var bar = new Bar();
        bar.Func();
    }
}
