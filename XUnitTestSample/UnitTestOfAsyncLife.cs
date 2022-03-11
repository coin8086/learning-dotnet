using System;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestSample
{
    public class UnitTestOfAsyncLife : IAsyncLifetime
    {
        public UnitTestOfAsyncLife()
        {
            Console.WriteLine("[UnitTestOfAsyncLife]: CTOR");
        }

        public Task DisposeAsync()
        {
            Console.WriteLine("[UnitTestOfAsyncLife]: Cleaning up resource for test...");
            return Task.CompletedTask;
        }

        public Task InitializeAsync()
        {
            Console.WriteLine("[UnitTestOfAsyncLife]: Preparing resource for test...");
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
}
