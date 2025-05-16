using UserService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureRepository();
builder.Services.ConfigureService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();


var app = builder.Build();

app.MapControllers();

app.Run();
