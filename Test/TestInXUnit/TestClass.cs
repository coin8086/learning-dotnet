//See more in https://xunit.net/docs/shared-context#constructor

using System;
using Xunit;
using Xunit.Abstractions;

namespace TestInXUnit;

public class TestClass : IDisposable
{
    //See https://xunit.net/docs/capturing-output
    private readonly ITestOutputHelper _output;

    private int _num = 0;

    public TestClass(ITestOutputHelper output)
    {
        //Test setup code here
        _output = output;
        _num++;
        _output.WriteLine("New TestClass");
    }

    public void Dispose()
    {
        //Test tear down code here
        //IDisposable is not required if no tear down code.
        _output.WriteLine("Dispose TestClass");
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
