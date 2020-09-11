using AspNetCore.Hashids.Mvc;
using AspNetCore.Hashids.Options;
using HashidsNet;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        internal static IHashids Hashids;

        public static IServiceCollection AddHashids(this IServiceCollection services, Action<HashidsOptions> setup)
        {
            services.Configure(setup);
            services.AddHttpContextAccessor();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<HashidsOptions>>().Value);
            services.ConfigureOptions<ConfigureJsonOptions>();
            services.AddSingleton(sp =>
            {
                var options = sp.GetRequiredService<HashidsOptions>();
                Hashids = new Hashids(options.Salt, options.MinHashLength, options.Alphabet, options.Steps);
                return Hashids;
            });

            services.PostConfigure<RouteOptions>(setup =>
            {
                setup.ConstraintMap.Add("hashids", typeof(HashidsRouteConstraint));
            });
            return services;
        }
    }
}
