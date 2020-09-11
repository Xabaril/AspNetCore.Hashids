using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCore.Hashids.Json
{
    // https://thomaslevesque.com/2020/08/31/inject-service-into-system-text-json-converter/
    public class DependencyInjectionJsonConverter : JsonConverter<object>, IServiceProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IServiceProvider serviceProvider;

        public DependencyInjectionJsonConverter(
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.serviceProvider = serviceProvider;
        }

        public override bool CanConvert(Type typeToConvert) => false;

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => 
            throw new NotImplementedException("Workaround to support DI in HashidsJsonConverter");

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
            throw new NotImplementedException("Workaround to support DI in HashidsJsonConverter");

        public object GetService(Type serviceType)
        {
            //Use the request services to resolve scope dependencies, if not, user the root service provider
            var services = httpContextAccessor.HttpContext?.RequestServices
                ?? serviceProvider;

            return services.GetService(serviceType);
        }
    }
}
