//See more in https://xunit.net/docs/shared-context#collection-fixture

using Xunit;
using Xunit.Abstractions;

namespace TestInXUnit;

public class CollectionFixtureData : ClassFixtureData, IAsyncLifetime
{
    public CollectionFixtureData(IMessageSink messageSink) : base(messageSink)
    {
    }

    //public override Task DisposeAsync()
    //{
    //    return Task.CompletedTask;
    //}

    //public override Task InitializeAsync()
    //{
    //    return Task.CompletedTask;
    //}
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
