using TaaSServer.Domain.Entities;

namespace TaaSServer.Domain.Ports;

public interface ITemperatureRepository
{
    Task<TemperatureReading> GetLatestAsync();
    Task SaveAsync(TemperatureReading temperatureReading);
}