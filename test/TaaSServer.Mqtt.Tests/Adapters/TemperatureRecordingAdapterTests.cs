using Microsoft.Extensions.Logging.Abstractions;
using MQTTnet;
using MQTTnet.Packets;
using TaaSServer.Domain.Ports;
using TaaSServer.Mqtt.Adapters;
using Xunit;

namespace TaaSServer.Mqtt.Tests.Adapters;

public class TemperatureRecordingAdapterTests
{
    [Fact]
    public async Task HandleAsync_ValidPayload_SavesCommandWithMappedFields()
    {
        var sensorService = new FakeTemperatureSensorService();
        var adapter = new TemperatureRecordingAdapter(sensorService, NullLogger<TemperatureRecordingAdapter>.Instance);
        var args = CreateArgs("{\"Timestamp\":\"2026-10-26T15:42:00\",\"DeviceId\":\"device-1\",\"Raw\":256}");

        await adapter.HandleAsync(args);

        Assert.Single(sensorService.SavedCommands);
        var cmd = sensorService.SavedCommands[0];
        Assert.Equal(DateTime.Parse("2026-10-26T15:42:00"), cmd.Timestamp);
        Assert.Equal("device-1", cmd.DeviceId);
        Assert.Equal((ushort)256, cmd.Raw);
    }

    [Fact]
    public async Task HandleAsync_InvalidTimestamp_ThrowsAndDoesNotSave()
    {
        var sensorService = new FakeTemperatureSensorService();
        var adapter = new TemperatureRecordingAdapter(sensorService, NullLogger<TemperatureRecordingAdapter>.Instance);
        var args = CreateArgs("{\"Timestamp\":\"not-a-date\",\"DeviceId\":\"device-1\",\"Raw\":256}");

        var exception = await Assert.ThrowsAsync<Exception>(() => adapter.HandleAsync(args));

        Assert.StartsWith("MQTT adapter: Error saving temperature recording", exception.Message);
        Assert.Empty(sensorService.SavedCommands);
    }

    private static MqttApplicationMessageReceivedEventArgs CreateArgs(string payload)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic("temperature/recording")
            .WithPayload(payload)
            .Build();

        return new MqttApplicationMessageReceivedEventArgs(
            "test-client",
            message,
            new MqttPublishPacket(),
            (_, _) => Task.CompletedTask);
    }

    private sealed class FakeTemperatureSensorService : ITemperatureSensorService
    {
        public List<SaveTemperatureRecordingCmd> SavedCommands { get; } = [];

        public Task SaveTemperatureRecording(SaveTemperatureRecordingCmd saveCmd)
        {
            SavedCommands.Add(saveCmd);
            return Task.CompletedTask;
        }
    }
}
