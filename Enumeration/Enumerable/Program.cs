using System.Collections;

namespace Enumerable;

class MyGuids : IEnumerable<string>
{
    class Enumerator : IEnumerator<string>
    {
        private string[] _guids;
        private int _index = -1;

        public Enumerator(string[] guids)
        {
            _guids = guids;
        }

        public string Current => _guids[_index];

        object IEnumerator.Current => _guids[_index];

        public bool MoveNext()
        {
            _index++;
            if (_index >= _guids.Length)
            {
                return false;
            }
            return true;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose()
        {
        }
    }

    private string[] _guids;

    public MyGuids(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        _guids = new string[count];
        for (var i = 0; i < count; i++)
        {
            _guids[i] = Guid.NewGuid().ToString();
        }
    }

    public IEnumerator<string> GetEnumerator()
    {
        return new Enumerator(_guids);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class MyGuids2 : IEnumerable<string>
{
    private string[] _guids;

    public MyGuids2(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        _guids = new string[count];
        for (var i = 0; i < count; i++)
        {
            _guids[i] = Guid.NewGuid().ToString();
        }
    }

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var item in _guids)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class MyGuids3
{
    private string[] _guids;

    public MyGuids3(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException("count");
        }

        _guids = new string[count];
        for (var i = 0; i < count; i++)
        {
            _guids[i] = Guid.NewGuid().ToString();
        }
    }

    public string[] Guids => _guids;

    public IEnumerable<string> GetGuids()
    {
        foreach (var item in _guids)
        {
            yield return item;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var myGuids = new MyGuids(3);
        foreach (var item in myGuids)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("---------------------------------");

        foreach (var item in myGuids)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("---------------------------------");

        foreach (var item in new MyGuids2(3))
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("---------------------------------");

        foreach (var item in new MyGuids3(3).GetGuids())
        {
            Console.WriteLine(item);
        }
    }
}
