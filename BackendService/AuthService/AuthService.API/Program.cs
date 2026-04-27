using AuthService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// passing the db and api config to authservice infrastructure to register the services and db context
ServiceRegistration.RegisterServices(builder.Services, builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
