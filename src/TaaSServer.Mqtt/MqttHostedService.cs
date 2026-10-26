using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using TaaSServer.Domain.Ports;
using TaaSServer.Mqtt.Handlers;

namespace TaaSServer.Mqtt;

public class MqttHostedService(IOptions<MqttBrokerOptions> options, ISecretClient secretClient, IEnumerable<BaseMqttHandler> handlers, ILogger<MqttHostedService> logger) : IHostedService
{
    private readonly MqttBrokerOptions _options = options.Value;
    private IMqttClient _client;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new MqttClientFactory();
        _client = factory.CreateMqttClient();

        _client.ApplicationMessageReceivedAsync += async args =>
        {
            var topic = args.ApplicationMessage.Topic;
            foreach (var handler in handlers.Where(h => h.Topic == topic))
            {
                await handler.HandleAsync(args);
            }
        };

        logger.LogInformation("Connecting to MQTT broker at {Host}:{Port}...", _options.Host, _options.Port);
        _ = Task.Run(() => ConnectAndSubscribeAsync(factory));
        return Task.CompletedTask;
    }

    private async Task<MqttClientOptions> BuildClientOptionsAsync()
    {
        var secretJson = await secretClient.GetSecret(_options.CertSecretName);
        using var secret = JsonDocument.Parse(secretJson);
        var root = secret.RootElement;

        var rootCaPem = root.GetProperty("mqtt-root-ca").GetString();
        var certPem = root.GetProperty("mqtt-cert").GetString();
        var keyPem = root.GetProperty("mqtt-key").GetString();

        using var pemCertificate = X509Certificate2.CreateFromPem(certPem, keyPem);
        var clientCertificate = new X509Certificate2(pemCertificate.Export(X509ContentType.Pfx));
        var rootCa = X509Certificate2.CreateFromPem(rootCaPem);

        return new MqttClientOptionsBuilder()
            .WithTcpServer(_options.Host, _options.Port)
            .WithTlsOptions(tls => tls
                .UseTls()
                .WithTargetHost(_options.Host)
                .WithClientCertificates([clientCertificate])
                .WithTrustChain([rootCa]))
            .Build();
    }

    private async Task ConnectAndSubscribeAsync(MqttClientFactory factory)
    {
        try
        {
            var clientOptions = await BuildClientOptionsAsync();
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await _client.ConnectAsync(clientOptions, timeout.Token);
            logger.LogInformation("Connected to MQTT broker at {Host}:{Port}.", _options.Host, _options.Port);

            foreach (var topic in handlers.Select(h => h.Topic).Distinct())
            {
                var subscribeOptions = factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(topic)
                    .Build();
                await _client.SubscribeAsync(subscribeOptions, timeout.Token);
                logger.LogInformation("Subscribed to MQTT topic {Topic}.", topic);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "MQTT connection to {Host}:{Port} failed: {Message}", _options.Host, _options.Port, ex.Message);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_client is { IsConnected: true })
        {
            await _client.DisconnectAsync(cancellationToken: cancellationToken);
        }
        _client?.Dispose();
    }
}
