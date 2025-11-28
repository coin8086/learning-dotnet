namespace ExtensionMethod;


interface IFoo
{
    void Bar();
}

class Foo : IFoo
{
    public void Bar()
    {
        Console.WriteLine("Foo.Bar");
    }

    public void Bar2()
    {
        Console.WriteLine("Foo.Bar2");
    }
}

static class IFooExtensions
{
    public static void Bar2(this IFoo foo)
    {
        Console.WriteLine("IFooExtensions.Bar2");
    }
}

//Complier error when defining an extension method with the same signature
//static class IFooExtensions2
//{
//    public static void Bar2(this IFoo foo)
//    {
//        Console.WriteLine("IFooExtensions2.Bar2");
//    }
//}

class Program
{
    static void Main(string[] args)
    {
        {
            IFoo foo = new Foo();
            foo.Bar();
            foo.Bar2();
        }

        Console.WriteLine("-----------------");

        {
            Foo foo = new Foo();
            foo.Bar();
            foo.Bar2();
        }
    }
}
