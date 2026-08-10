using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace libraryApplication.Interface
{
    public static class OpenApiMediaTypeFactory
    {
        public static OpenApiMediaType Create()
        {
            return new OpenApiMediaType
            {
                Schema = new OpenApiSchema { Type = "object" },
                Example = new OpenApiString("hello")
            };
        }
    }
}