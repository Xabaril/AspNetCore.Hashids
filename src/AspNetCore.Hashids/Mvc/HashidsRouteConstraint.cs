using HashidsNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Globalization;

namespace AspNetCore.Hashids.Mvc
{
    public class HashidsRouteConstraint : IRouteConstraint
    {
        private readonly IHashids hashids;

        public HashidsRouteConstraint(IHashids hashids)
        {
            this.hashids = hashids ?? throw new ArgumentNullException(nameof(hashids));
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (values.TryGetValue(routeKey, out var value))
            {
                var hashid = Convert.ToString(value, CultureInfo.InvariantCulture);
                var decode = hashids.Decode(hashid);

                return decode.Length > 0;
            }

            return false;
        }
    }
}
