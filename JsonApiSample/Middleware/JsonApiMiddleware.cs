using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JsonApiSample.Middleware
{
    public class JsonApiMiddleware
    {
        private readonly RequestDelegate _next;
        private const string contentType = "Content-Type";
        private const string jsonApiContentType = "application/vnd.api+json";
        public JsonApiMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /*
         * Configures the response to JsonApi standards
        */
        public async Task Invoke(HttpContext httpContext)
        {
            var response = httpContext.Response;
            var acceptHeaders = httpContext.Request.GetTypedHeaders().Accept;
            if (acceptHeaders?.Count() > 0)
            {
                var jsonApiAcceptHeader = acceptHeaders.Where(h => h.MediaType.Equals(jsonApiContentType)).SingleOrDefault();
                if (jsonApiAcceptHeader?.Parameters?.Count() > 0)
                {
                    response.Body = Stream.Null;
                    response.StatusCode = 415;
                }
            }

            response.OnStarting((state) =>
            {
                var contentTypeHeader = response.GetTypedHeaders().ContentType;
                if (contentTypeHeader != null && contentTypeHeader.MediaType.Equals(jsonApiContentType))
                {
                    response.Headers.Remove(jsonApiContentType);
                    response.Headers.Append(contentType, jsonApiContentType);
                }

                return Task.FromResult(0);
            }, null);


            await _next.Invoke(httpContext);
        }
    }
}