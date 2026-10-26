using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Ports;
using TaaSServer.Mqtt.DTOs;

namespace TaaSServer.Mqtt.Adapters;

public class TemperatureReadingAdapter(
    [FromServices] ITemperatureSensorService temperatureSensorService,
    ILogger<TemperatureReadingAdapter> logger) : BaseMqttAdapter<ReceiveTemperatureRecordingDto>
{
    public override string Topic => "temperature/recording";

    protected override async Task HandleAsync(ReceiveTemperatureRecordingDto receiveTemperatureRecordingDto)
    {
        try
        {
            var saveCmd = CreateSaveTemperatureRecordingCmd(receiveTemperatureRecordingDto);
            await temperatureSensorService.SaveTemperatureReading(saveCmd);
        } 
        catch (Exception ex)
        {
            throw new Exception($"MQTT adapter: Error saving temperature recording: {ex.Message}");
        }

        logger.LogInformation(
            $"MQTT adapter: {receiveTemperatureRecordingDto.DeviceId} {receiveTemperatureRecordingDto.Raw} {receiveTemperatureRecordingDto.Timestamp}");
    }
    
    private SaveTemperatureReadingCmd CreateSaveTemperatureRecordingCmd(ReceiveTemperatureRecordingDto receiveTemperatureRecordingDto)
    {
        return new SaveTemperatureReadingCmd(
            DateTime.Parse(receiveTemperatureRecordingDto.Timestamp),
            receiveTemperatureRecordingDto.DeviceId,
            receiveTemperatureRecordingDto.Raw);
    }
}
