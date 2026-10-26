namespace TaaSServer.Domain.Ports;

public record struct SaveTemperatureRecordingCmd(
    DateTime Timestamp,
    string DeviceId,
    ushort Raw);

public interface ITemperatureSensorService
{
    Task SaveTemperatureRecording(SaveTemperatureRecordingCmd saveCmd);
}