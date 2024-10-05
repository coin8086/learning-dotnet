namespace DiBasics;

interface IServiceA : IChecker
{
}

class ServiceA : Checker, IServiceA
{
}
