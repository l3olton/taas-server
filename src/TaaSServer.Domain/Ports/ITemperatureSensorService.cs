namespace TaaSServer.Domain.Ports;

public record struct SaveTemperatureReadingCmd(
    DateTime Timestamp,
    string DeviceId,
    ushort Raw);

public interface ITemperatureSensorService
{
    Task SaveTemperatureReading(SaveTemperatureReadingCmd saveCmd);
}