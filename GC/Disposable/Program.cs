// See https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Disposable;

//TODO: Add some unmanaged resources to this class for demo
class Sample : IDisposable
{
    private bool _disposed = false;
    private Stopwatch _stopwatch;
    private SafeHandle? _handle;

    public Sample()
    {
        Console.Error.WriteLine("Sample()");
        _stopwatch = Stopwatch.StartNew();
        _handle = new SafeFileHandle(IntPtr.Zero, true);
    }

    public void Dispose()
    {
        Console.Error.WriteLine($"Dispose(): Time elapsed: {_stopwatch.Elapsed}");

        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // Derived class only needs to override this when it wants to "dispose" something, that is,
    // when it has a class member of IDisposable, or when it directly owns unmanaged resources.
    protected virtual void Dispose(bool disposing)
    {
        Console.Error.WriteLine($"Dispose(bool disposing): Time elapsed: {_stopwatch.Elapsed}");

        if (!_disposed)
        {
            if (disposing)
            {
                Console.Error.WriteLine("disposing = true");
                // Dispose managed state (managed objects)
                _handle?.Dispose();
                _handle = null;
            }
            else
            {
                Console.Error.WriteLine("disposing = false");
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposed = true;
        }
        else
        {
            Console.Error.WriteLine("Disposed already!");
        }
    }

    // NOTE: Override finalizer ONLY IF 'Dispose(bool disposing)' has code to free unmanaged resources
    // Here a finalizer is provided only as a way to explore GC.
    ~Sample()
    {
        try
        {
            // Here _stopwatch may already have been destroyed. So catch exception by it.
            Console.Error.WriteLine($"~Sample(): Time elapsed: {_stopwatch.Elapsed}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"~Sample(): {ex}");
        }

        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }
}


class Program
{
    static void Main(string[] args)
    {
        using var obj = new Sample();
    }
}
