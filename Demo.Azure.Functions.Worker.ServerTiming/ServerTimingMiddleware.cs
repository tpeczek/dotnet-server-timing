using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;

namespace Demo.Azure.Functions.Worker.ServerTiming
{
    internal class ServerTimingMiddleware : IFunctionsWorkerMiddleware
    {
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            // Pre-function execution

            await next(context);

            // Post-function execution
            InvocationResult invocationResult = context.GetInvocationResult();

            HttpResponseData? response = invocationResult.Value as HttpResponseData;
            if (response is not null)
            {
                response.Headers.Add("Server-Timing", "cache;dur=300;desc=\"Cache\",sql;dur=900;desc=\"Sql Server\",fs;dur=600;desc=\"FileSystem\",cpu;dur=1230;desc=\"Total CPU\"");
            }
        }
    }
}
