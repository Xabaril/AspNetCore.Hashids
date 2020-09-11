using AspNetCore.Hashids.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace AspNetCore.Hashids.Options
{
    public class ConfigureJsonOptions : IConfigureOptions<JsonOptions>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IServiceProvider serviceProvider;

        public ConfigureJsonOptions(
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.serviceProvider = serviceProvider;
        }

        public void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(
                new DependencyInjectionJsonConverter(
                    httpContextAccessor,
                    serviceProvider));
        }
    }
}
