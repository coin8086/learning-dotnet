using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestSample
{
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

    //One and only one Test in CollectionFixture and CollectionFixture2 should pass, and the other should fail. 
    //The run order decides which one should pass.
    //See more in https://xunit.net/docs/shared-context#collection-fixture

    [Collection("Some Collection")]
    public class CollectionFixture
    {
        private ClassFixtureData _data;

        public CollectionFixture(CollectionFixtureData data)
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
    public class CollectionFixture2
    {
        private ClassFixtureData _data;

        public CollectionFixture2(CollectionFixtureData data)
        {
            _data = data;
        }

        [Fact]
        public void Test()
        {
            Assert.Equal(0, _data.Num);
        }
    }
}
