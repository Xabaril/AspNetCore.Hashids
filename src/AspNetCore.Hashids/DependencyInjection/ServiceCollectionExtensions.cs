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
        /// <summary>
        /// Adds default ASP.NET Core services for AspNetCore.Hashids
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/></param>
        /// <param name="setup">The action used to configure <see cref="Hashids"/></param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chanined.</returns>
        public static IServiceCollection AddHashids(this IServiceCollection services, Action<HashidsOptions> setup)
        {
            services.Configure(setup);
            services.AddHttpContextAccessor();
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<HashidsOptions>>().Value);
            services.ConfigureOptions<ConfigureJsonOptions>();
            services.AddSingleton<IHashids>(sp =>
            {
                var options = sp.GetRequiredService<HashidsOptions>();
                return new Hashids(options.Salt, options.MinHashLength, options.Alphabet, options.Steps);
            });

            services.PostConfigure<RouteOptions>(setup =>
            {
                setup.ConstraintMap.Add("hashids", typeof(HashidsRouteConstraint));
            });
            return services;
        }
    }
}
