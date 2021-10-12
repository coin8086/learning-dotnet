using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestSample
{
    public class TestContext
    {
        private int _num = 0;

        public TestContext()
        {
            _num++;
        }

        //Both ShouldEqualToOne and ShouldStillEqualToOne should pass
        //See more in https://xunit.net/docs/shared-context#constructor

        [Fact]
        public void ShouldEqualToOne()
        {
            Assert.Equal(1, _num++);
        }

        [Fact]
        public void ShouldStillEqualToOne()
        {
            Assert.Equal(1, _num++);
        }
    }
}
