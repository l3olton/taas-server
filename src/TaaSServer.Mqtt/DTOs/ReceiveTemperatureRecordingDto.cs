namespace TaaSServer.Mqtt.DTOs;

public record ReceiveTemperatureRecordingDto(string Timestamp, string DeviceId, ushort Raw);
