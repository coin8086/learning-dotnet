using System.Runtime.CompilerServices;

namespace AsyncEnumeration;

class MyGuids : IAsyncEnumerable<string>
{
    class Enumberator : IAsyncEnumerator<string>
    {
        private string[] _guids;
        private int _index = -1;
        private CancellationToken _cancel;

        public Enumberator(string[] guids, CancellationToken cancellationToken = default) 
        {
            _guids = guids;
            _cancel = cancellationToken;
        }

        public string Current => _guids[_index];

        public async ValueTask<bool> MoveNextAsync()
        {
            //Simulate some async opeartion
            await Task.Delay(100, _cancel);

            _index++;
            if (_index >= _guids.Length)
            {
                return false;
            }
            return true;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
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

    public IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new Enumberator(_guids, cancellationToken);
    }
}

class MyGuids2 : IAsyncEnumerable<string>
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

    //NOTE: The return type is not Task, Task<T>, or void, while "async" modifier is used.
    public async IAsyncEnumerator<string> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        foreach (var guid in _guids)
        {
            //Simulate some async opeartion
            await Task.Delay(100, cancellationToken);

            yield return guid;
        }
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

    //NOTE the EnumeratorCancellation attribute on the CancellationToken parameter!
    public async IAsyncEnumerable<string> GetGuids([EnumeratorCancellation]CancellationToken cancellationToken = default)
    {
        foreach (var guid in _guids)
        {
            //Simulate some async opeartion
            await Task.Delay(100, cancellationToken);

            yield return guid;
        }
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        var cancelSource = new CancellationTokenSource();
        var myguids = new MyGuids(3).WithCancellation(cancelSource.Token);
        await foreach (var item in myguids)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("---------------------------------");

        await foreach (var item in myguids)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("---------------------------------");

        await foreach (var item in new MyGuids2(3).WithCancellation(cancelSource.Token))
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("---------------------------------");

        await foreach (var item in new MyGuids3(3).GetGuids().WithCancellation(cancelSource.Token))
        {
            Console.WriteLine(item);
        }
    }
}
