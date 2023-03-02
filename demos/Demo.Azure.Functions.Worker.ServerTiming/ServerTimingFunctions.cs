using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Lib.ServerTiming;
using Lib.ServerTiming.Http.Headers;

namespace Demo.Azure.Functions.Worker.ServerTiming
{
    public class ServerTimingFunctions
    {
        private readonly IServerTiming _serverTiming;

        public ServerTimingFunctions(IServerTiming serverTiming)
        {
            _serverTiming = serverTiming;
        }

        [Function("basic")]
        public HttpResponseData Basic([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
        {

            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            _serverTiming.Metrics.Add(new ServerTimingMetric("cache", 300, "Cache"));
            _serverTiming.Metrics.Add(new ServerTimingMetric("sql", 900, "Sql Server"));
            _serverTiming.Metrics.Add(new ServerTimingMetric("fs", 600, "FileSystem"));
            _serverTiming.Metrics.Add(new ServerTimingMetric("cpu", 1230, "Total CPU"));

            response.WriteString("-- Demo.Azure.Functions.Worker.ServerTiming --");

            return response;
        }
    }
}
