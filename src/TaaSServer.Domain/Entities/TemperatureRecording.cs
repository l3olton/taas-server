using TaaSServer.Domain.Ports;

namespace TaaSServer.Domain.Entities;

public record TemperatureRecording(
    DateTime Timestamp,
    string DeviceId,
    float Temperature);

public static class CreateTemperatureRecording
{
    public static TemperatureRecording FromSaveTemperatureRecordingCmd(SaveTemperatureRecordingCmd temperatureRecordingCmd)
    {
        var temperature = (temperatureRecordingCmd.Raw >>> 7) / 2.0f;
        
        return new TemperatureRecording(
            temperatureRecordingCmd.Timestamp,
            temperatureRecordingCmd.DeviceId,
            temperature);
    }
}
