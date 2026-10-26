namespace TaaSServer.Domain.Ports;

public record struct SaveTemperatureRecordingCmd(
    DateTime Timestamp,
    string DeviceId,
    UInt16 Raw);

public interface ITemperatureSensorService
{
    Task SaveTemperatureRecording(SaveTemperatureRecordingCmd saveCmd);
}