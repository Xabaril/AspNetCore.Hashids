using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.TestHost
{
    [ExcludeFromCodeCoverage]
    public static class RequestBuilderExtensions
    {
        public static RequestBuilder WithJsonBody<TEntity>(this RequestBuilder builder, TEntity entity) where TEntity : class
        {
            return builder.And(configure =>
            {
                var json = JsonConvert.SerializeObject(entity);
                configure.Content = new StringContent(json, Encoding.UTF8, "application/json");
            });
        }

        public static Task<HttpResponseMessage> PutAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Put);
        }

        public static Task<HttpResponseMessage> DeleteAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Delete);
        }

        public static Task<HttpResponseMessage> PatchAsync(this RequestBuilder requestBuilder)
        {
            return requestBuilder.SendAsync(HttpMethods.Patch);
        }
    }
}
