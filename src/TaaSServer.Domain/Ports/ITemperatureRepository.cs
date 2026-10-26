using TaaSServer.Domain.Entities;

namespace TaaSServer.Domain.Ports;

public interface ITemperatureRepository
{
    Task<TemperatureRecording> GetLatestAsync();
    Task SaveAsync(TemperatureRecording temperatureRecording);
}