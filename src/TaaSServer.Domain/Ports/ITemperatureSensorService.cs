namespace TaaSServer.Domain.Ports;

public record struct TemperatureRecordingCmd(
    DateTime Timestamp,
    string DeviceId,
    float Temperature);

public interface ITemperatureSensorService
{
    Task SaveTemperatureRecording(TemperatureRecordingCmd temperatureRecordingCmd);
}