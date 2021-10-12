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

        private IEnumerable<string> GetTitles(int pageIndex, int pageSize, bool isDescending)
        {
            IEnumerable<string> source = isDescending ? _titles.Reverse() : _titles;
            return  source.Skip(pageIndex * pageSize).Take(pageSize);
        }

        public ExampleService(ILogger<ExampleService> logger)
        {
            _logger = logger;
        }

        public override Task<ExampleResponse> UnaryCall(ExampleRequest request, ServerCallContext context)
        {
            var response = new ExampleResponse() { Request = request };
            response.Titles.Add(GetTitles(request.PageIndex, request.PageSize, request.IsDescending));
            return Task.FromResult(response);
        }

        public override async Task StreamingFromServer(ExampleRequest request, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            var response = new ExampleResponse() { Request = request };
            response.Titles.Add(GetTitles(request.PageIndex, request.PageSize, request.IsDescending));

            //For demo purpose, the same response is sent repeatedly until client cancels it.
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await responseStream.WriteAsync(response);
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogWarning("Cancled by client.");
                }
            }
        }

        public override async Task<ExampleResponse> StreamingFromClient(IAsyncStreamReader<ExampleRequest> requestStream, ServerCallContext context)
        {
            var response = new ExampleResponse();
            try
            {
                //When the request is cancled by client, an IOException will be thrown.
                while (await requestStream.MoveNext(context.CancellationToken))
                {
                    var request = requestStream.Current;
                    response.Titles.Add(GetTitles(request.PageIndex, request.PageSize, request.IsDescending));
                }
            }
            catch (System.IO.IOException ex)
            {
                _logger.LogWarning(ex.ToString());
            }
            return response;
        }

        public override async Task StreamingBothWays(IAsyncStreamReader<ExampleRequest> requestStream, IServerStreamWriter<ExampleResponse> responseStream, ServerCallContext context)
        {
            try
            {
                //When the request is cancled by client, an IOException will be thrown.
                await foreach (var request in requestStream.ReadAllAsync(cancellationToken: context.CancellationToken))
                {
                    var response = new ExampleResponse() { Request = request };
                    response.Titles.Add(GetTitles(request.PageIndex, request.PageSize, request.IsDescending));
                    await responseStream.WriteAsync(response);
                }
            }
            catch (System.IO.IOException ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }
    }
}
