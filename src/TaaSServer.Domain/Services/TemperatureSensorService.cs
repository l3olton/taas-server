using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Domain.Services;

public class TemperatureSensorService(ITemperatureRepository temperatureRepository, ILogger<TemperatureSensorService> logger) : ITemperatureSensorService
{

    public async Task SaveTemperatureReading(SaveTemperatureReadingCmd saveCmd)
    {
        var temperatureReading = CreateTemperatureReading.FromSaveTemperatureReadingCmd(saveCmd);
        
        try
        {
            await temperatureRepository.SaveAsync(temperatureReading);
        }
        catch (Exception ex)
        {
            throw new Exception($"Domain Service: Error saving temperature reading: {ex.Message}");
        }

        logger.LogInformation(
            $"Domain Service processed save command: DeviceId: {temperatureReading.DeviceId} Temperature: {temperatureReading.Temperature} Timestamp: {temperatureReading.Timestamp}");
    }
}