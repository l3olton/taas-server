using Microsoft.AspNetCore.Builder;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaaSServer.App.SecretClient;
using TaaSServer.Domain.Ports;
using TaaSServer.Domain.Services;
using TaaSServer.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly)
    .AddApplicationPart(typeof(HttpApi).Assembly);
builder.Services.AddSingleton<IHelloService, HelloService>();

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = new BasicAWSCredentials(builder.Configuration["AWS:AccessKey"], builder.Configuration["AWS:SecretKey"]);
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonSecretsManager>();
builder.Services.AddSingleton<ISecretClient, SecretsManagerSecretClient>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// app.MapOpenApi();

app.Run();