using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TaaSServer.Domain.Ports;
using TaaSServer.Domain.Services;
using TaaSServer.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly)
    .AddApplicationPart(typeof(HttpApi).Assembly);
builder.Services.AddSingleton<IHelloService, HelloService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// app.MapOpenApi();

app.Run();