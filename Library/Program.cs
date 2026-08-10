using libraryApplication.Repository;
using libraryInfra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<Repo>();
builder.Services.AddDbContext<Library_Context>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Con")));
builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<Library_Context>().AddSignInManager();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    //options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    //options.AddPolicy("UserOnly", policy =>  policy.RequireRole("User"));
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
//app.MapGet("/event-producing", async (ProducerService producer, CancellationToken cancellationToken) =>
//{
//    await producer.ProduceAsync(cancellationToken);
//    return "Event Sent!";
//})
//.WithOpenApi();

app.UseHttpsRedirection();
app.UseAuthentication();   
app.UseAuthorization();
app.MapControllers();
app.Run();