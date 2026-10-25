using MQTTnet;

namespace TaaSServer.Mqtt.Handlers;

public abstract class BaseMqttHandler
{
    public abstract string Topic { get; }

    public abstract Task HandleAsync(MqttApplicationMessageReceivedEventArgs args);
}
