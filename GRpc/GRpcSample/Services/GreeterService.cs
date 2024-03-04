using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GRpcSample
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var headers = context.RequestHeaders.Select((h) => h.ToString());
            _logger.LogInformation("Headers:\n{headers}", string.Join('\n', headers));

            var rand = new Random();
            context.ResponseTrailers.Add("Lucky-Number", rand.Next(10000).ToString());

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
    }
}
