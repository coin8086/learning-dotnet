using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest;

public class AsyncLifetime : IAsyncLifetime
{
    public AsyncLifetime()
    {
        Console.WriteLine("[AsyncLifetime]: CTOR");
    }

    public Task DisposeAsync()
    {
        Console.WriteLine("[AsyncLifetime]: Cleaning up resource for test...");
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        Console.WriteLine("[AsyncLifetime]: Preparing resource for test...");
        return Task.CompletedTask;
    }

    [Fact]
    public void PassingTest()
    {
        Assert.Equal(4, Add(2, 2));
    }

    [Fact]
    public void FailingTest()
    {
        Assert.Equal(5, Add(2, 2));
    }

    int Add(int x, int y)
    {
        return x + y;
    }
}
