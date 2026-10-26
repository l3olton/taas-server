using System.Text.Json;
using MQTTnet;

namespace TaaSServer.Mqtt.Adapters;

public abstract class BaseMqttAdapter
{
    public abstract string Topic { get; }

    public abstract Task HandleAsync(MqttApplicationMessageReceivedEventArgs args);
}

public abstract class BaseMqttAdapter<T> : BaseMqttAdapter
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
            throw new Exception($"MQTT adapter: JSON payload deserialization failed: {ex.Message}");
        }

        await HandleAsync(message);
    }

    protected abstract Task HandleAsync(T message);
}
