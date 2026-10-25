using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Db;

public class TemperatureRepository(ILogger<TemperatureRepository> logger) : ITemperatureRepository
{
    public Task<TemperatureRecording> GetLatestAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(TemperatureRecording temperatureRecording)
    {
        logger.LogInformation("SaveAsync called: {DeviceId} {Temperature} {Timestamp}", temperatureRecording.DeviceId, temperatureRecording.Temperature, temperatureRecording.Timestamp);
        return Task.CompletedTask;
    }
}
