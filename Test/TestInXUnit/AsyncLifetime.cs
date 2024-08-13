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
        Output("CTOR");
    }

    private void Output(string msg)
    {
        _output.WriteLine($"[{nameof(AsyncLifetime)}] {msg}");
    }

    public Task DisposeAsync()
    {
        Output("Test cleanup");
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        Output("Test setup");
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
