using MQTTnet;
using MQTTnet.Packets;
using TaaSServer.Mqtt.Handlers;
using Xunit;

namespace TaaSServer.Mqtt.Tests.Handlers;

public class BaseMqttHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidJsonPayload_DeserializesAndPassesMessageToTypedHandler()
    {
        var handler = new TestHandler();
        var args = CreateArgs("{\"Id\":42,\"Name\":\"sensor\"}");

        await handler.HandleAsync(args);

        Assert.Single(handler.ReceivedMessages);
        Assert.Equal(42, handler.ReceivedMessages[0].Id);
        Assert.Equal("sensor", handler.ReceivedMessages[0].Name);
    }

    [Fact]
    public async Task HandleAsync_InvalidJsonPayload_ThrowsDeserializationException()
    {
        var handler = new TestHandler();
        var args = CreateArgs("{not valid json");

        var exception = await Assert.ThrowsAsync<Exception>(() => handler.HandleAsync(args));

        Assert.StartsWith("MQTT handler: JSON payload deserialization failed", exception.Message);
        Assert.Empty(handler.ReceivedMessages);
    }

    private static MqttApplicationMessageReceivedEventArgs CreateArgs(string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("test/topic")
            .WithPayload(payload)
            .Build();

        return new MqttApplicationMessageReceivedEventArgs(
            "test-client",
            message,
            new MqttPublishPacket(),
            (_, _) => Task.CompletedTask);
    }

    private sealed class TestHandler : BaseMqttHandler<TestMessage>
    {
        public override string Topic => "test/topic";

        public List<TestMessage> ReceivedMessages { get; } = [];

        protected override Task HandleAsync(TestMessage message)
        {
            ReceivedMessages.Add(message);
            return Task.CompletedTask;
        }
    }

    public class TestMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
