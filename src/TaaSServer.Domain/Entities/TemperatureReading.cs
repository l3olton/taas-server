using TaaSServer.Domain.Ports;

namespace TaaSServer.Domain.Entities;

public record TemperatureReading(
    DateTime Timestamp,
    string DeviceId,
    float Temperature);

public static class CreateTemperatureReading
{
    public static TemperatureReading FromSaveTemperatureReadingCmd(SaveTemperatureReadingCmd temperatureReadingCmd)
    {
        var temperature = (temperatureReadingCmd.Raw >>> 7) / 2.0f;
        
        return new TemperatureReading(
            temperatureReadingCmd.Timestamp,
            temperatureReadingCmd.DeviceId,
            temperature);
    }
}
