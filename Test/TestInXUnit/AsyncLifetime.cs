using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestInXUnit;

public class AsyncLifetime : IAsyncLifetime
{
    //See https://xunit.net/docs/capturing-output
    private readonly ITestOutputHelper _output;

    public AsyncLifetime(ITestOutputHelper output)
    {
        _output = output;
        _output.WriteLine("[AsyncLifetime]: CTOR");
    }

    private void Output(string msg)
    {
        _output.WriteLine($"[{nameof(AsyncLifetime)}]: {msg}");
    }

    public Task DisposeAsync()
    {
        Output("Clean up test");
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        Output("Prepare for test");
        return Task.CompletedTask;
    }

    [Fact]
    public void Test1()
    {
        Output("Test1");
        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        Output("Test2");
        Assert.True(true);
    }
}
