//See https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/
//and https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-8.0#thread-safety
//and https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutabledictionary-2?view=net-8.0#thread-safety
//and https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-8.0#thread-safety

using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace DictionaryTypes;

class Program
{
    static void Main(string[] args)
    {
        //non-thread-safe
        var dict = new Dictionary<int, string>();
        dict[1] = "abc";
        dict[2] = "xyz";
        foreach (var (key, value) in dict)
        {
            Console.WriteLine($"{key}={value}");
        }

        Console.WriteLine("-----------------");

        //thread-safe, read-only
        var readonlyDict = (IReadOnlyDictionary<int, string>)dict;
        foreach (var (key, value) in readonlyDict)
        {
            Console.WriteLine($"{key}={value}");
        }

        Console.WriteLine("-----------------");

        //thread-safe, read-write
        var concurrentDict = new ConcurrentDictionary<int, string>(dict);
        foreach (var (key, value) in concurrentDict)
        {
            Console.WriteLine($"{key}={value}");
        }

        Console.WriteLine("-----------------");

        //thread-safe, read-write (return new instance on write, while keeping the original one unchanged.
        //That's what "immutable" means.)
        var immutableDict = concurrentDict.ToImmutableDictionary();
        var immutableDict2 = ImmutableDictionary.CreateRange(dict);
        foreach (var (key, value) in immutableDict2)
        {
            Console.WriteLine($"{key}={value}");
        }
    }
}
