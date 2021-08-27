using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GRpcSample
{
    public class ExampleService : Example.ExampleBase
    {
        private readonly ILogger<ExampleService> _logger;

        private static string[] _titles = new string[]
        {
            "C++",
            "Java",
            "Kotlin",
            "Python",
            "Go",
            "TypeScript",
            "Ruby",
            "C#",
            "PHP",
            "Dart",
        };

        public ExampleService(ILogger<ExampleService> logger)
        {
            _logger = logger;
        }

        public override Task<ExampleResponse> UnaryCall(ExampleRequest request, ServerCallContext context)
        {
            IEnumerable<string> source = request.IsDescending ?  _titles.Reverse() : _titles;
            var result = source.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
            var response = new ExampleResponse() { Request = request };
            response.Titles.Add(result);
            return Task.FromResult(response);
        }

        public override Task StreamingFromServer(ExampleRequest request, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            return base.StreamingFromServer(request, responseStream, context);
        }

        public override Task<ExampleResponse> StreamingFromClient(IAsyncStreamReader<ExampleRequest> requestStream, ServerCallContext context)
        {
            return base.StreamingFromClient(requestStream, context);
        }

        public override Task StreamingBothWays(IAsyncStreamReader<ExampleRequest> requestStream, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            return base.StreamingBothWays(requestStream, responseStream, context);
        }
    }
}
