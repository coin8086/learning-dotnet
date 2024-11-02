//See https://learn.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim.-ctor?view=net-8.0#system-threading-readerwriterlockslim-ctor
//and https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-threading-readerwriterlockslim

namespace ReaderWriterLockBasics;

public class SynchronizedCache : IDisposable
{
    public enum AddOrUpdateStatus
    {
        Added,
        Updated,
        Unchanged
    };

    private ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
    private Dictionary<int, string> _innerCache = new Dictionary<int, string>();

    //TODO/Q: Doesn't it require a reader lock?
    public int Count => _innerCache.Count;

    public string Read(int key)
    {
        _cacheLock.EnterReadLock();
        try
        {
            return _innerCache[key];
        }
        finally
        {
            _cacheLock.ExitReadLock();
        }
    }

    public void Add(int key, string value)
    {
        _cacheLock.EnterWriteLock();
        try
        {
            _innerCache.Add(key, value);
        }
        finally
        {
            _cacheLock.ExitWriteLock();
        }
    }

    public bool AddWithTimeout(int key, string value, int timeout)
    {
        if (_cacheLock.TryEnterWriteLock(timeout))
        {
            try
            {
                _innerCache.Add(key, value);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public AddOrUpdateStatus AddOrUpdate(int key, string value)
    {
        _cacheLock.EnterUpgradeableReadLock();
        try
        {
            string? result = null;
            if (_innerCache.TryGetValue(key, out result))
            {
                if (result == value)
                {
                    return AddOrUpdateStatus.Unchanged;
                }
                else
                {
                    _cacheLock.EnterWriteLock();
                    try
                    {
                        _innerCache[key] = value;
                    }
                    finally
                    {
                        _cacheLock.ExitWriteLock();
                    }
                    return AddOrUpdateStatus.Updated;
                }
            }
            else
            {
                _cacheLock.EnterWriteLock();
                try
                {
                    _innerCache.Add(key, value);
                }
                finally
                {
                    _cacheLock.ExitWriteLock();
                }
                return AddOrUpdateStatus.Added;
            }
        }
        finally
        {
            _cacheLock.ExitUpgradeableReadLock();
        }
    }

    public void Delete(int key)
    {
        _cacheLock.EnterWriteLock();
        try
        {
            _innerCache.Remove(key);
        }
        finally
        {
            _cacheLock.ExitWriteLock();
        }
    }

    public void Dispose()
    {
        _cacheLock.Dispose();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
