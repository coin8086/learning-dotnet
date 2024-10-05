namespace DiShare;

public interface IOperation
{
    string Id { get; }
}

public interface ITransientOperation : IOperation { }

public interface IScopedOperation : IOperation { }

public interface ISingletonOperation : IOperation { }
