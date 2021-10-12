using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestSample
{
    [CollectionDefinition("Some Collection")]
    public class CollectionDefinition : ICollectionFixture<FixtureData>
    {
    }

    //One and only one Test in CollectionFixture and CollectionFixture2 should pass, and the other should fail. 
    //The run order decides which one should pass.
    //See more in https://xunit.net/docs/shared-context#collection-fixture

    [Collection("Some Collection")]
    public class CollectionFixture
    {
        private FixtureData _data;

        public CollectionFixture(FixtureData data)
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
        private FixtureData _data;

        public CollectionFixture2(FixtureData data)
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
