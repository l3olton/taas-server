using MQTTnet;
using MQTTnet.Packets;
using TaaSServer.Mqtt.Adapters;
using Xunit;

namespace TaaSServer.Mqtt.Tests.Adapters;

public class BaseMqttAdapterTests
{
    [Fact]
    public async Task HandleAsync_ValidJsonPayload_DeserializesAndPassesMessageToTypedAdapter()
    {
        var adapter = new TestAdapter();
        var args = CreateArgs("{\"Id\":42,\"Name\":\"sensor\"}");

        await adapter.HandleAsync(args);

        Assert.Single(adapter.ReceivedMessages);
        Assert.Equal(42, adapter.ReceivedMessages[0].Id);
        Assert.Equal("sensor", adapter.ReceivedMessages[0].Name);
    }

    [Fact]
    public async Task HandleAsync_InvalidJsonPayload_ThrowsDeserializationException()
    {
        var adapter = new TestAdapter();
        var args = CreateArgs("{not valid json");

        var exception = await Assert.ThrowsAsync<Exception>(() => adapter.HandleAsync(args));

        Assert.StartsWith("MQTT adapter: JSON payload deserialization failed", exception.Message);
        Assert.Empty(adapter.ReceivedMessages);
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

    private sealed class TestAdapter : BaseMqttAdapter<TestMessage>
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
