using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;
using Xunit;

namespace TaaSServer.Domain.Tests.Entities;

public class CreateTemperatureRecordingTests
{
    [Fact]
    public void FromSaveTemperatureRecordingCmd_MapsTimestampAndDeviceId()
    {
        var timestamp = new DateTime(2026, 10, 26, 15, 42, 0, DateTimeKind.Utc);
        var cmd = new SaveTemperatureRecordingCmd(timestamp, "device-1", 256);

        var recording = CreateTemperatureRecording.FromSaveTemperatureRecordingCmd(cmd);

        Assert.Equal(timestamp, recording.Timestamp);
        Assert.Equal("device-1", recording.DeviceId);
    }

    [Theory]
    [InlineData((ushort)0, 0.0f)]
    [InlineData((ushort)127, 0.0f)]
    [InlineData((ushort)128, 0.5f)]
    [InlineData((ushort)256, 1.0f)]
    [InlineData((ushort)1000, 3.5f)]
    [InlineData((ushort)65535, 255.5f)]
    public void FromSaveTemperatureRecordingCmd_ConvertsRawValueToFloatTemperature(ushort raw, float expected)
    {
        var cmd = new SaveTemperatureRecordingCmd(DateTime.UtcNow, "device-1", raw);

        var recording = CreateTemperatureRecording.FromSaveTemperatureRecordingCmd(cmd);

        Assert.Equal(expected, recording.Temperature);
    }
}
