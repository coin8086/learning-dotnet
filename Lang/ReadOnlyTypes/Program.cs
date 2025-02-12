using System.Text.Json;

namespace ReadOnlyTypes;

interface IReadOnlyEntity
{
    int Id { get; }
}

class Entity : IReadOnlyEntity
{
    public int Id { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

class Program
{
    static IReadOnlyList<IReadOnlyEntity> ConvertToReadOnlyList(List<Entity> list)
    {
        return list;
    }

    static IReadOnlyDictionary<int, IReadOnlyEntity> ConvertToReadOnlyDictionary(IEnumerable<KeyValuePair<int, Entity>> dict)
    {
        var dict2 = new Dictionary<int, IReadOnlyEntity>();
        foreach (var kvp in dict)
        {
            dict2.Add(kvp.Key, kvp.Value);
        }
        return dict2;
    }
    static IReadOnlyDictionary<int, IReadOnlyEntity> ConvertToReadOnlyDictionary2(IDictionary<int, Entity> dict)
    {
        return new ProxyReadOnlyDictionary<int, IReadOnlyEntity, Entity>(dict);
    }

    static void Main(string[] args)
    {
        var list = new List<Entity>();
        list.Add(new Entity { Id = 1 });
        list.Add(new Entity { Id = 2 });
        list.Add(new Entity { Id = 3 });

        var readOnlyList = ConvertToReadOnlyList(list);
        foreach (var entity in readOnlyList)
        {
            Console.WriteLine(entity.Id);
        }

        Console.WriteLine("------------------------");

        var dict = new Dictionary<int, Entity>();
        foreach (var entity in list)
        {
            dict.Add(entity.Id, entity);
        }

        var readOnlyDict = ConvertToReadOnlyDictionary(dict);
        foreach (var (kev, value) in readOnlyDict)
        {
            Console.WriteLine($"{kev}: {value}");
        }

        Console.WriteLine("------------------------");

        var readOnlyDict2 = ConvertToReadOnlyDictionary2(dict);
        foreach (var (kev, value) in readOnlyDict2)
        {
            Console.WriteLine($"{kev}: {value}");
        }

        foreach (var value in readOnlyDict2.Values)
        {
            Console.WriteLine(value);
        }
    }
}
