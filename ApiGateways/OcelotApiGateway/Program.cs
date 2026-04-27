/*using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();*/


using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json");

// Register Ocelot API Gateway services in the dependency injection container.
// This configures the Ocelot middleware to handle API routing, authentication, rate limiting, and request/response transformation.
builder.Services.AddOcelot();

var app = builder.Build();


app.MapGet("/", () => "Hello World!");
app.UseOcelot().Wait();

app.Run();
