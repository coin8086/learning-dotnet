namespace TestInMSTest;

[TestClass]
public class TestInitAndCleanup
{
    protected static TestContext? _testContext;

    protected static void Output(string msg)
    {
        _testContext?.WriteLine($"[{_testContext?.FullyQualifiedTestClassName}] {msg}");
    }

    public TestInitAndCleanup()
    {
        Output("CTOR");
    }

    [ClassInitialize]
    //Or: public static async Task TestInit(TestContext testContext)
    public static void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
        Output("Test class init");
    }

    [ClassCleanup]
    //Or: public static async Task TestCleanup()
    public static void ClassCleanup()
    {
        Output("Test class cleanup");
    }

    [TestInitialize]
    //Or: public async Task TestInit()
    public virtual void TestInit()
    {
        Output("Test init");
    }

    [TestCleanup]
    //Or: public async Task TestCleanup()
    public virtual void TestCleanup()
    {
        Output("Test cleanup");
    }

    [TestMethod]
    public void TestMethod1()
    {
        Output("Test1");
        Assert.IsTrue(true, "This is true.");
    }

    [TestMethod]
    public void TestMethod2()
    {
        Output("Test2");
        Assert.IsTrue(true, "This is also true.");
    }

}
