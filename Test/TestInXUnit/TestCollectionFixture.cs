using System;
using System.Threading.Tasks;
using Xunit;

namespace TestInXUnit;

public class CollectionFixtureData : ClassFixtureData, IAsyncLifetime
{
    public new Task DisposeAsync()
    {
        Console.WriteLine("[CollectionFixtureData]: Cleaning up resource for test...");
        return Task.CompletedTask;
    }

    public new Task InitializeAsync()
    {
        Console.WriteLine("[CollectionFixtureData]: Preparing resource for test...");
        return Task.CompletedTask;
    }
}

[CollectionDefinition("Some Collection")]
public class CollectionDefinition : ICollectionFixture<CollectionFixtureData>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

//One and only one Test in TestCollectionFixture and TestCollectionFixture2 should pass, and the other should fail. 
//The run order decides which one should pass.
//See more in https://xunit.net/docs/shared-context#collection-fixture

[Collection("Some Collection")]
public class TestCollectionFixture
{
    private ClassFixtureData _data;

    public TestCollectionFixture(CollectionFixtureData data)
    {
        _data = data;
    }

    [Fact]
    public void Test()
    {
        Assert.Equal(0, _data.Num);
    }
}

[Collection("Some Collection")]
public class TestCollectionFixture2
{
    private ClassFixtureData _data;

    public TestCollectionFixture2(CollectionFixtureData data)
    {
        _data = data;
    }

    [Fact]
    public void Test()
    {
        Assert.Equal(0, _data.Num);
    }
}
