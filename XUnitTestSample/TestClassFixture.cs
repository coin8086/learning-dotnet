using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTest;

//See more in https://xunit.net/docs/shared-context#class-fixture
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

public class ClassFixtureData : IAsyncLifetime
{
    private int _num = 0;

    public int Num
    {
        get { return _num++; }
    }

    public Task DisposeAsync()
    {
        Console.WriteLine("[ClassFixtureData]: Cleaning up resource for test...");
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        Console.WriteLine("[ClassFixtureData]: Preparing resource for test...");
        return Task.CompletedTask;
    }
}
