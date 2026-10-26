using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Db;

public class TemperatureRepository(ILogger<TemperatureRepository> logger) : ITemperatureRepository
{
    public Task<TemperatureReading> GetLatestAsync()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(TemperatureReading temperatureReading)
    {
        logger.LogInformation(
            $"SaveAsync called: {temperatureReading.DeviceId} {temperatureReading.Temperature} {temperatureReading.Timestamp}");
        return Task.CompletedTask;
    }
}
