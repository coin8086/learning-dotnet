namespace TestInMSTest;

[TestClass]
public class TestInitAndCleanup
{
    private static TestContext _testContext;

    public TestInitAndCleanup()
    {
        _testContext.WriteLine("CTOR");
    }

    [ClassInitialize]
    //Or: public static async Task TestInit(TestContext testContext)
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
        _testContext.WriteLine("Test class init");
    }

    [ClassCleanup]
    //Or: public static async Task TestCleanup()
    public static void ClassCleanup()
    {
        _testContext.WriteLine("Test class cleanup");
    }

    [TestInitialize]
    //Or: public async Task TestInit()
    public void TestInit()
    {
        _testContext.WriteLine("Test init");
    }

    [TestCleanup]
    //Or: public async Task TestCleanup()
    public void TestCleanup()
    {
        _testContext.WriteLine("Test cleanup");
    }

    [TestMethod]
    public void TestMethod1()
    {
        Assert.IsTrue(true, "This is true.");
    }

    [TestMethod]
    public void TestMethod2()
    {
        Assert.IsTrue(true, "This is also true.");
    }

}
