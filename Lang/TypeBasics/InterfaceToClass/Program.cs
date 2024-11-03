namespace InterfaceToClass;

interface A
{
    string Name { get; set; }
}

interface B
{
    int Id { get; set; }
}

class C : A, B
{
    public string Name { get; set; } = "Rob";

    public int Id { get; set; } = 100;
}

class Program
{
    static void Main(string[] args)
    {
        C c = new C();
        A a = c;

        //NOTE: Here a of type A can be explicitly converted to b of type B,
        //while type A and B have no relationship but C implements both A and B.
        B b = (B)a;

        Console.WriteLine(b.Id);
    }
}
