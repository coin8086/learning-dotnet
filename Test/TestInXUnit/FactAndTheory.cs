using System.Collections.Generic;
using Xunit;

namespace TestInXUnit;

public class FactAndTheory
{
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

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(6)]
    public void MyTheory(int value)
    {
        Assert.True(IsOdd(value));
    }

    bool IsOdd(int value)
    {
        return value % 2 == 1;
    }

    [Theory]
    [MemberData(nameof(MemberData))]
    public void MyTheoryOfMemberData(int x, int y, int expected)
    {
        Assert.Equal(expected, Add(x, y));
    }

    //The MemberData must be static and must be of type IEnumerable<object[]>.
    public static IEnumerable<object[]> MemberData => new List<object[]>
    {
        new object[] { 1, 2, 3 },
        new object[] { 1, 1, 2 },
    };
}
