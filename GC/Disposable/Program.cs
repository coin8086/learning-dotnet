// See https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose

using System.Diagnostics;

namespace Disposable;

class Sample : IDisposable
{
    private bool _disposed;
    private Stopwatch _stopwatch;

    public Sample()
    {
        Console.WriteLine("Sample()");
        _stopwatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        Console.WriteLine($"Dispose(): Time elapsed: {_stopwatch.Elapsed}");

        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    // Derived class only needs to override this when it wants to "dispose" something, that is,
    // when it has a class member of IDisposable, or when it directly owns unmanaged resources.
    protected virtual void Dispose(bool disposing)
    {
        Console.WriteLine($"Dispose(bool disposing): Time elapsed: {_stopwatch.Elapsed}");

        if (!_disposed)
        {
            if (disposing)
            {
                Console.WriteLine("disposing = true");
                // TODO: dispose managed state (managed objects)
            }
            else
            {
                Console.WriteLine("disposing = false");
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposed = true;
        }
        else
        {
            Console.WriteLine("Disposed already!");
        }
    }

    // Override finalizer ONLY IF 'Dispose(bool disposing)' has code to free unmanaged resources
    // Here a finalizer is provided only as a way to explore GC.
    ~Sample()
    {
        try
        {
            // Here _stopwatch may already have been destroyed. So catch exception by it.
            Console.WriteLine($"~Sample(): Time elapsed: {_stopwatch.Elapsed}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"~Sample(): {ex}");
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
