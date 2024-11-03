//[REF]
//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-generic-modifier
//https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/

namespace Covariance;

class BaseClass
{
    public BaseClass() { }
}

class DerivedClass : BaseClass
{
    public DerivedClass() { }
}

//Covariant interface.
interface ICovariant<out T>
{
    void MethodA();

    //T can be return type.
    T MethodB();

    //T is disallowed for argument type.
    //void InvalidMethod(T t);
}

//Extending covariant interface.
//[NOTE] If no "out" for T here, then the following tests in main show the same results.
//Then why "out" is needed here?
interface IExtCovariant<out T> : ICovariant<T> { }

//Implementing covariant interface.
//[NOTE] Why "out" is not allowed for a type parameter of generic class?
class CovariantClass<T> : IExtCovariant<T> where T: new()
{
    public void MethodA() { }

    public T MethodB()
    {
        return new T();
    }
}

class Program
{
    static void Main()
    {
        ICovariant<BaseClass> iBase = new CovariantClass<BaseClass>();
        ICovariant<DerivedClass> iDerived = new CovariantClass<DerivedClass>();

        // You can assign iDerived to iBase because the ICovariant interface is covariant.
        iBase = iDerived;

        var covariantBase = typeof(ICovariant<BaseClass>);
        var covariantDerived = typeof(ICovariant<DerivedClass>);
        var assignable = covariantBase.IsAssignableFrom(covariantDerived);
        var isSubclass = covariantDerived.IsSubclassOf(covariantBase);
        Console.WriteLine(assignable); //True
        Console.WriteLine(isSubclass); //False
    }
}
