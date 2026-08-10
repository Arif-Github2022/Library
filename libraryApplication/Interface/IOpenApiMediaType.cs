using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var mediaType = new OpenApiMediaType
{
    Schema = new OpenApiSchema { Type = "object" }
};

mediaType.Example = new OpenApiString("hello");