using AspNetCore.Hashids.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace WebApi
{
    public class HashidsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hashids = context
                .ApiDescription
                .ParameterDescriptions
                .Where(x => x.ModelMetadata.BinderType == typeof(HashidsModelBinder))
                .ToDictionary(d => d.Name, d => d);

            foreach (var parameter in operation.Parameters)
            {
                if (hashids.TryGetValue(parameter.Name, out var apiParameter))
                {
                    parameter.Schema.Format = string.Empty;
                    parameter.Schema.Type = "string";
                }
            }

            foreach (var schema in context.SchemaRepository.Schemas.Values)
            {
                foreach (var property in schema.Properties)
                {
                    if (hashids.TryGetValue(property.Key, out var apiParameter))
                    {
                        property.Value.Format = string.Empty;
                        property.Value.Type = "string";
                    }
                }
            }
        }
    }
}
