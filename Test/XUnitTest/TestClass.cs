using System;
using Xunit;

namespace XUnitTest;

//See more in https://xunit.net/docs/shared-context#constructor
public class TestClass : IDisposable
{
    private int _num = 0;

    public TestClass()
    {
        //Test setup code here
        _num++;
    }

    public void Dispose()
    {
        //Test tear down code here
        //IDisposable is not required if no tear down code.
    }

    //Both ShouldEqualToOne and ShouldStillEqualToOne should pass, since each test method
    //is called on a new instance of the test class. All test methods share the same setup
    //and tear down code.

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
