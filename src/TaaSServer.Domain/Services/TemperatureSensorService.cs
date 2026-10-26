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
            logger.LogError(ex, "Domain Service: Error saving temperature reading");
        }

        logger.LogInformation(
            $"Domain Service: {temperatureReading.DeviceId} {temperatureReading.Temperature} {temperatureReading.Timestamp}");
    }
}