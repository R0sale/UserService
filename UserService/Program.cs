using Microsoft.AspNetCore.Mvc;
using UserService.ActionFilters;
using UserService.Extensions;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRepository();
builder.Services.ConfigureService();
builder.Services.ConfigureServiceManager();
builder.Services.AddAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureValidationFilter();
builder.Services.AddControllers()
    .AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseRouting();

app.ConfigureExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
