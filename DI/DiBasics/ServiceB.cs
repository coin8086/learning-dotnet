namespace DiBasics;

interface IServiceB : IChecker
{
}

class ServiceB : Checker, IServiceB
{
    IServiceA _sa;
    IServiceX<ServiceB> _sx;

    public ServiceB(IServiceA serviceA, IServiceX<ServiceB> sx)
    {
        _sa = serviceA;
        _sx = sx;
    }

    public override void Check(int indent = 0)
    {
        base.Check(indent);
        _sa.Check(2);
        _sx.Check(2);
    }
}
