using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Ports;
using TaaSServer.Mqtt.DTOs;

namespace TaaSServer.Mqtt.Adapters;

public class TemperatureRecordingAdapter(
    [FromServices] ITemperatureSensorService temperatureSensorService,
    ILogger<TemperatureRecordingAdapter> logger) : BaseMqttAdapter<ReceiveTemperatureRecordingDto>
{
    public override string Topic => "temperature/recording";

    protected override async Task HandleAsync(ReceiveTemperatureRecordingDto receiveTemperatureRecordingDto)
    {
        try
        {
            var saveCmd = CreateSaveTemperatureRecordingCmd(receiveTemperatureRecordingDto);
            await temperatureSensorService.SaveTemperatureRecording(saveCmd);
        } 
        catch (Exception ex)
        {
            throw new Exception($"MQTT adapter: Error saving temperature recording: {ex.Message}");
        }

        logger.LogInformation(
            $"MQTT adapter: {receiveTemperatureRecordingDto.DeviceId} {receiveTemperatureRecordingDto.Raw} {receiveTemperatureRecordingDto.Timestamp}");
    }
    
    private SaveTemperatureRecordingCmd CreateSaveTemperatureRecordingCmd(ReceiveTemperatureRecordingDto receiveTemperatureRecordingDto)
    {
        return new SaveTemperatureRecordingCmd(
            DateTime.Parse(receiveTemperatureRecordingDto.Timestamp),
            receiveTemperatureRecordingDto.DeviceId,
            receiveTemperatureRecordingDto.Raw);
    }
}
