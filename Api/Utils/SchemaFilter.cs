using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class DateOnlySchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(DateOnly))
        {
            schema.Type = "string";
            schema.Format = "date"; 
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("2025-07-28");
        }
        else if (context.Type == typeof(TimeOnly))
        {
            schema.Type = "string";
            schema.Format = "time";
            schema.Example = new Microsoft.OpenApi.Any.OpenApiString("14:30");
        }
    }
}
