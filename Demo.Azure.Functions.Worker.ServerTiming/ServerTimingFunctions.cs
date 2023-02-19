using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Demo.Azure.Functions.Worker.ServerTiming
{
    public class ServerTimingFunctions
    {
        private readonly ILogger _logger;

        public ServerTimingFunctions(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ServerTimingFunctions>();
        }

        [Function("basic")]
        public HttpResponseData Basic([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
        {

            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("-- Demo.Azure.Functions.Worker.ServerTiming --");

            return response;
        }
    }
}
