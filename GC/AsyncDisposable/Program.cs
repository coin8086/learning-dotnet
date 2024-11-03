using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AsyncDisposable;

class SomeAsyncDisposable : IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] SomeAsyncDisposable.DisposeManagedAsync()");
        return ValueTask.CompletedTask;
    }
}

class Sample : IAsyncDisposable
{
    private SomeAsyncDisposable? _obj;
    private SafeHandle? _handle;

    public Sample()
    {
        _obj = new SomeAsyncDisposable();
        _handle = new SafeFileHandle(IntPtr.Zero, true);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeManagedAsync().ConfigureAwait(false);
        DisposeUnmanaged();
        GC.SuppressFinalize(this);
    }

    // If a derived class has any managed resources to dispose, just overload this method.
    protected virtual async ValueTask DisposeManagedAsync()
    {
        if (_obj is not null)
        {
            await _obj.DisposeAsync().ConfigureAwait(false);
            _obj = null;
        }
        if (_handle is not null) 
        { 
            if (_handle is IAsyncDisposable disposable)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                _handle.Dispose();
            }
            _handle = null;
        }
    }

    // If a derived class has any unmanaged resources to dispose, just overload this method.
    protected virtual void DisposeUnmanaged() {}


    // NOTE: Have a finalizer only when it directly owns any unmanaged resources.
    ~Sample()
    {
        DisposeUnmanaged();
    }
}

class Sample2 : IAsyncDisposable, IDisposable
{
    private Stopwatch _stopwatch;
    private bool _disposed = false;
    private SomeAsyncDisposable? _obj;
    private SafeHandle? _handle;

    public Sample2()
    {
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Sample2()");

        _stopwatch = Stopwatch.StartNew();
        _obj = new SomeAsyncDisposable();
        _handle = new SafeFileHandle(IntPtr.Zero, true);
    }

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] DisposeAsync(): Time elapsed: {_stopwatch.Elapsed}");

        await DisposeManagedAsync().ConfigureAwait(false);
        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeManagedAsync()
    {
        if (_obj is not null)
        {
            await _obj.DisposeAsync().ConfigureAwait(false);
            _obj = null;
        }
        if (_handle is not null)
        {
            if (_handle is IAsyncDisposable disposable)
            {
                await disposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                _handle.Dispose();
            }
            _handle = null;
        }
    }

    public void Dispose()
    {
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Dispose(): Time elapsed: {_stopwatch.Elapsed}");

        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)

#if REUSE_ASYNC_DISPOSABLE
                // NOTE: This is to align the behavior of synchronous dispose to the one of asynchronous, by using 
                // IAsyncDisposable as much as possible. 
                DisposeManagedAsync().AsTask().Wait();
#else
                // Or you can align to the behaviour of synchronous dispose by using IDisposable as much as possible,
                // like
                if (_handle is not null)
                {
                    _handle.Dispose();
                    _handle = null;
                }
                if (_obj is not null)
                {
                    if (_obj is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    else
                    {
                        _obj.DisposeAsync().AsTask().Wait();
                    }
                    _obj = null;
                }
#endif
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposed = true;
        }
    }

    // NOTE: Override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // Here a finalizer is provided only as a way to explore GC.
    ~Sample2()
    {
        Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] ~Sample2(): Time elapsed: {_stopwatch.Elapsed}");

        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

}

class Program
{
    static async Task Main(string[] args)
    {
        //await using var obj1 = new Sample();
        await using var obj2 = new Sample2();
    }
}
