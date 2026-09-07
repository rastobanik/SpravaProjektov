using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Text.Json;

namespace SpravaProjektovAPI.Infrastructure
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiDescription = context.ApiDescription;
                       
            operation.Deprecated |= apiDescription.IsDeprecated;

            if (operation.Parameters == null) return;

            foreach (var parameter in operation.Parameters)
            {
                var description = apiDescription.ParameterDescriptions
                    .FirstOrDefault(p => p.Name == parameter.Name);

                if (description == null) continue;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default == null && description.DefaultValue != null)
                {
                    // Ak je hodnota DBNull (čo sa stáva pri niektorých routach), ignorujeme ju  
                    if (description.DefaultValue is not System.DBNull && description.DefaultValue != null && description.ModelMetadata != null)
                    {
                        var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                        parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
                    }
                }


                parameter.Required |= description.IsRequired;
            }
        }

    }
}
