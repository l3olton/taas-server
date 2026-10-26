using Microsoft.AspNetCore.Builder;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaaSServer.App.SecretClient;
using TaaSServer.Db;
using TaaSServer.Domain.Ports;
using TaaSServer.Domain.Services;
using TaaSServer.Http;
using TaaSServer.Mqtt;
using TaaSServer.Mqtt.Adapters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Program).Assembly)
    .AddApplicationPart(typeof(HttpApi).Assembly);
builder.Services.AddSingleton<IHelloService, HelloService>();
builder.Services.AddSingleton<ITemperatureRepository, TemperatureRepository>();
builder.Services.AddSingleton<ITemperatureSensorService, TemperatureSensorService>();

var awsOptions = builder.Configuration.GetAWSOptions();
awsOptions.Credentials = new BasicAWSCredentials(builder.Configuration["AWS:AccessKey"], builder.Configuration["AWS:SecretKey"]);
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonSecretsManager>();
builder.Services.AddSingleton<ISecretClient, SecretsManagerSecretClient>();

builder.Services.Configure<MqttBrokerOptions>(builder.Configuration.GetSection(MqttBrokerOptions.SectionName));
builder.Services.AddSingleton<BaseMqttAdapter, TemperatureReadingAdapter>();
builder.Services.AddHostedService<MqttHostedService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
// app.MapOpenApi();

app.Run();