using Microsoft.EntityFrameworkCore;
using TaaSServer.Domain.Entities;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Db;

public class TemperatureRepository(IDbContextFactory<TemperatureReadingContext> contextFactory) : ITemperatureRepository
{
    public Task<TemperatureReading> GetLatestAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SaveAsync(TemperatureReading temperatureReading)
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
}
