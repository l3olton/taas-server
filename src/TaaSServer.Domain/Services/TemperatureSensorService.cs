using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Domain.Services;

public class TemperatureSensorService(ITemperatureRepository temperatureRepository, ILogger<TemperatureSensorService> logger) : ITemperatureSensorService
{

    public async Task SaveTemperatureRecording(SaveTemperatureRecordingCmd saveCmd)
    {
        var temperatureRecording = CreateTemperatureRecording.FromSaveTemperatureRecordingCmd(saveCmd);
        
        try
        {
            await temperatureRepository.SaveAsync(temperatureRecording);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Domain Service: Error saving temperature reading");
        }

        logger.LogInformation(
            $"Domain Service: {temperatureRecording.DeviceId} {temperatureRecording.Temperature} {temperatureRecording.Timestamp}");
    }
}