using System.Linq;
using Microsoft.AspNetCore.Http;
using Lib.AspNetCore.ServerTiming.Http.Headers;

namespace Lib.AspNetCore.ServerTiming.Http.Extensions
{
    internal static class HttpRequestHeadersExtensions
    {
        #region Fields
        private const string ACCEPT_TRAILERS = "trailers";
        #endregion

        #region Methods
        public static bool AllowsTrailers(this HttpRequest request)
        {
            return request.Headers.ContainsKey(HeaderNames.AcceptTransferEncoding)
                && request.Headers[HeaderNames.AcceptTransferEncoding].Contains(ACCEPT_TRAILERS);
        }
        #endregion
    }
}
