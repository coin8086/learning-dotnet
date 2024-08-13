//See more in https://xunit.net/docs/shared-context#class-fixture

using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TestInXUnit;

public class ClassFixtureData : IAsyncLifetime
{
    //See https://xunit.net/docs/capturing-output
    private readonly IMessageSink _messageSink;

    private int _num = 0;

    public int Num => _num++;

    public ClassFixtureData(IMessageSink messageSink)
    {
        _messageSink = messageSink;
    }

    protected virtual void Output(string msg)
    {
        var diagMsg = new DiagnosticMessage($"[{this.GetType().Name}] {msg}");
        _messageSink.OnMessage(diagMsg);
    }

    public virtual Task DisposeAsync()
    {
        Output("Fixture cleanup");
        return Task.CompletedTask;
    }

    public virtual Task InitializeAsync()
    {
        Output("Fixture setup");
        return Task.CompletedTask;
    }
}

public class TestClassFixture : IClassFixture<ClassFixtureData>
{
    private ClassFixtureData _data;

    public TestClassFixture(ClassFixtureData data)
    {
        _data = data;
    }

    //One and only one between Test1 and Test2 should pass, and the other should fail, since
    //the data is shared by all instances of the test class. The run order decides which one passes.

    [Fact]
    public void Test1()
    {
        Assert.Equal(0, _data.Num);
    }

    [Fact]
    public void Test2()
    {
        Assert.Equal(0, _data.Num);
    }
}
