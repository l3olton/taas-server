using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Domain.Services;

public class TemperatureSensorService(ITemperatureRepository temperatureRepository, ILogger<TemperatureSensorService> logger) : ITemperatureSensorService
{

    public async Task SaveTemperatureRecording(TemperatureRecordingCmd temperatureRecordingCmd)
    {
        var temperatureRecording = new TemperatureRecording(
            temperatureRecordingCmd.Timestamp,
            temperatureRecordingCmd.DeviceId,
            temperatureRecordingCmd.Temperature);

        try
        {
            await temperatureRepository.SaveAsync(temperatureRecording);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Domain Service: Error recording temperature reading");
        }
        
        logger.LogInformation("Domain Service: {DeviceId} {Temperature}", temperatureRecording.DeviceId, temperatureRecording.Temperature);
    }
}