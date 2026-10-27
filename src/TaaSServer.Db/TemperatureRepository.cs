using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Db;

public class TemperatureRepository(
    IDbContextFactory<TemperatureReadingContext> contextFactory,
    ILogger<TemperatureRepository> logger) : ITemperatureRepository
{
    public Task<TemperatureReading> GetLatestAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(TemperatureReading temperatureReading)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            context.TemperatureReadings.Add(new Entities.TemperatureReading
            {
                DeviceId = temperatureReading.DeviceId,
                Temperature = temperatureReading.Temperature,
                Timestamp = temperatureReading.Timestamp.ToUniversalTime()
            });

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Temperature Repository: Error saving temperature reading {ex.Message}");
        }
        logger.LogInformation($"Temperature Repository: Saved temperature reading. DeviceId: {temperatureReading.DeviceId} Temperature: {temperatureReading.Temperature} Timestamp: {temperatureReading.Timestamp}");
    }
}
