namespace TestInMSTest;

[TestClass]
public class TestClassAndTestMethod
{
    [TestMethod]
    public void TestMethod1()
    {
        Assert.IsTrue(true, "This is true.");
    }
}