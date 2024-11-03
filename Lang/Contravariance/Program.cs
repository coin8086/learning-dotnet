//[REF]
//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/out-generic-modifier
//https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/

namespace Contravariance;

class Program
{
    delegate void HandleEvent<in T>(T t);

    static void SetString(string s) { }

    static void SetObject(object o) { }

    static void Main(string[] args)
    {
        HandleEvent<string> hstr = SetString;
        HandleEvent<object> hobj = SetObject;
        hstr = hobj;

        var assignable = hstr.GetType().IsAssignableFrom(hobj.GetType());
        Console.WriteLine(assignable);
    }
}
