namespace DiShare;

public class Operation : ITransientOperation, IScopedOperation, ISingletonOperation
{
    public Operation()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; }
}
