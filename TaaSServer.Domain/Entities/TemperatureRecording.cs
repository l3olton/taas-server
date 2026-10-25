namespace TaaSServer.Domain.Entities;

public record TemperatureRecording(
    DateTime Timestamp,
    string DeviceId,
    float Temperature);