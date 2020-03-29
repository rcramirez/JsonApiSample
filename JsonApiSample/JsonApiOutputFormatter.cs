using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace JsonApiSample
{
    public class JsonApiOutputFormatter : SystemTextJsonOutputFormatter
    {
        public JsonApiOutputFormatter(JsonSerializerOptions options) : base(options)
        {
            const string jsonApiMediaTypeName = "application/vnd.api+json";
            SupportedMediaTypes.Clear();
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(jsonApiMediaTypeName));
        }
    }
}