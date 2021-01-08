using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;


namespace DegreeMapping.Models
{
    public class APIKeyMessageHandler : DelegatingHandler
    {
        private const string APIKeyToCheck = "Th1sIsth3Way";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            bool validKey = false;

            IEnumerable<string> requestHeaders;
            var checkApiKey = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);
            if(checkApiKey)
            {
                if (requestHeaders.FirstOrDefault().Equals(APIKeyToCheck))
                {
                    validKey = true;
                }
            }

            if (!validKey)
            {
                return httpRequestMessage.CreateResponse(HttpStatusCode.Forbidden, "Invalid API Key");
            }

            var response = await base.SendAsync(httpRequestMessage, cancellationToken);
            return response;
        }
    }
    //https://www.youtube.com/watch?v=8V8-3ewm78k
}