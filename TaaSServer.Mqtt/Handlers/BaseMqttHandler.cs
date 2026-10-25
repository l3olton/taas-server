using System.Text.Json;
using MQTTnet;

namespace TaaSServer.Mqtt.Handlers;

public abstract class BaseMqttHandler
{
    public abstract string Topic { get; }

    public abstract Task HandleAsync(MqttApplicationMessageReceivedEventArgs args);
}

public abstract class BaseMqttHandler<T> : BaseMqttHandler
{
    public override async Task HandleAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        var payload = args.ApplicationMessage.ConvertPayloadToString();

        T message;
        try
        {
            message = JsonSerializer.Deserialize<T>(payload);
        }
        catch (Exception ex)
        {
            throw new Exception($"MQTT handler: JSON payload deserialization failed: {ex.Message}");
        }

        await HandleAsync(message);
    }

    public abstract Task HandleAsync(T message);
}
