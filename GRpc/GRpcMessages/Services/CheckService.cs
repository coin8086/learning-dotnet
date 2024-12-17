using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace GRpcMessages.Services;

public class CheckService : Check.CheckBase
{
    ILogger _logger;

    public CheckService(ILogger<CheckService> logger)
    {
        _logger = logger;
    }

    #region Person
    public override Task<Person> Echo(Person person, ServerCallContext context)
    {
        Console.WriteLine(nameof(Echo));
        ShowPerson(person);
        return Task.FromResult(person);
    }

    public override Task<Person> Get(Empty _, ServerCallContext context)
    {
        var person = new Person()
        {
            //NOTE: It seems Id can be absent here, though it's not nullable in the proto definition.
            Id = 1,
            FirstName = "Rob",
            LastName = "X",
            Birthday = Timestamp.FromDateTimeOffset(DateTimeOffset.Parse("1990-10-10T20:02:00Z")),
            //Age filed is nullable. Let's omit it here.
        };
        person.Roles.Add("Admin");
        person.Roles.Add("User");
        person.Attributes["modified_at"] = DateTimeOffset.UtcNow.ToString("u");
        person.Attributes["modified_by"] = nameof(CheckService);

        Console.WriteLine(nameof(Get));
        ShowPerson(person);
        return Task.FromResult(person);
    }

    private void ShowPerson(Person person)
    {
        Console.WriteLine($"Person={person}");
        Console.WriteLine($"In which,");
        Console.WriteLine($"\tBirthday=\"{person.Birthday?.ToDateTimeOffset():u}\"");

        var roles = string.Join(", ", person.Roles);
        Console.WriteLine($"\tRoles=\"{roles}\"");

        var entries = person.Attributes.Select((pair) => $"{pair.Key}={pair.Value}");
        var attrs = string.Join(", ", entries);
        Console.WriteLine($"\tAttributes=\"{attrs}\"");
    }
    #endregion

    #region Status
    public override Task<Status> EchoStatus(Status status, ServerCallContext context)
    {
        Console.WriteLine(nameof(EchoStatus));
        ShowStatus(status);
        return Task.FromResult(status);
    }

    public override Task<Status> GetStatus(Empty _, ServerCallContext context)
    {
        var data = @"{
""enabled"": true,
""metadata"": [ ""value1"", ""value2"" ]
}";

        var status = new Status()
        {
            Message = "Hello",
            Detail = Any.Pack(new Person() { FirstName = "Pack" }),
            Data = Value.Parser.ParseJson(data),
        };

        Console.WriteLine(nameof(GetStatus));
        ShowStatus(status);
        return Task.FromResult(status);
    }

    private void ShowStatus(Status status)
    {
        Console.WriteLine($"Status={status}");
        if (status.Detail.Is(Person.Descriptor))
        {
            var person = status.Detail.Unpack<Person>();
            ShowPerson(person);
        }
        var json = JsonFormatter.Default.Format(status.Data);
        Console.WriteLine($"Data={json}");
    }
    #endregion

    #region Result
    public override Task<Result> EchoResult(Result result, ServerCallContext context)
    {
        Console.WriteLine(nameof(EchoResult));
        ShowResult(result);
        return Task.FromResult(result);
    }

    public override Task<Result> GetResult(Empty _, ServerCallContext context)
    {
        var result = new Result()
        {
            Code = 100,
            Person = new Person() { LastName = "X" },
        };
        Console.WriteLine(nameof(GetResult));
        ShowResult(result);
        return Task.FromResult(result);
    }

    private void ShowResult(Result result)
    {
        Console.WriteLine($"Result={result}");
        switch (result.ResultCase)
        {
            case Result.ResultOneofCase.Person:
                ShowPerson(result.Person);
                break;
            case Result.ResultOneofCase.Status:
                ShowStatus(result.Status);
                break;
            default:
                throw new ArgumentException("Unexpected result!");
        }
    }
    #endregion
}
