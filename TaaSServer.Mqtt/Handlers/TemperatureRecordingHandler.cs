using MQTTnet;

namespace TaaSServer.Mqtt.Handlers;

public class TemperatureRecordingHandler : BaseMqttHandler
{
    public override string Topic => "temperature/recording";

    public override Task HandleAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        Console.WriteLine("Temperature recording received");
        return Task.CompletedTask;
    }
}
