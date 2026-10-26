using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Mqtt.Adapters;

public record ReceiveTemperatureRecordingDto(string Timestamp, string DeviceId, ushort Raw);

public class TemperatureRecordingAdapter(
    [FromServices] ITemperatureSensorService temperatureSensorService,
    ILogger<TemperatureRecordingAdapter> logger) : BaseMqttAdapter<ReceiveTemperatureRecordingDto>
{
    public override string Topic => "temperature/recording";

    protected override async Task HandleAsync(ReceiveTemperatureRecordingDto receiveTemperatureRecordingDto)
    {
        try
        {
            await temperatureSensorService.SaveTemperatureRecording(new SaveTemperatureRecordingCmd(
                DateTime.Parse(receiveTemperatureRecordingDto.Timestamp),
                receiveTemperatureRecordingDto.DeviceId,
                receiveTemperatureRecordingDto.Raw));
        } 
        catch (Exception ex)
        {
            throw new Exception($"MQTT adapter: Error saving temperature recording: {ex.Message}");
        }

        logger.LogInformation(
            $"MQTT adapter: {receiveTemperatureRecordingDto.DeviceId} {receiveTemperatureRecordingDto.Raw} {receiveTemperatureRecordingDto.Timestamp}");
    }
}
