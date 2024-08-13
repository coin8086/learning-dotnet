namespace TestInMSTest;

[TestClass]
public class TestSubclass1 : TestInitAndCleanup
{
    [TestMethod]
    public void TestMethod3()
    {
        Output("Test3");
        Assert.IsTrue(true, "This is true.");
    }
}

[TestClass]
public class TestSubclass2: TestInitAndCleanup
{
    //NOTE: The TestContext is different from what the base class gets. Compare the output between 
    //TestSubclass1 and TestSubclass2.
    [ClassInitialize]
    public static new void ClassInit(TestContext testContext)
    {
        _testContext = testContext;
        Output("Test class init in subclass");
    }

    [ClassCleanup]
    public static new void ClassCleanup()
    {
        Output("Test class cleanup in subclass");
    }

    //NOTE:
    //1) Even though the base method is "virtual", the "TestInitialize" attribute is still needed.
    //2) So the base "virtual" is optional for the "TestInitialize" method.
    [TestInitialize]
    public override void TestInit()
    {
        Output("Test init in subclass");
    }

    [TestCleanup]
    public override void TestCleanup()
    {
        Output("Test cleanup in subclass");
    }

    [TestMethod]
    public void TestMethod3()
    {
        Output("Test3");
        Assert.IsTrue(true, "This is true.");
    }
}
