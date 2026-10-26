using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Mqtt.Adapters;

public record TemperatureRecording(string Timestamp, string DeviceId, UInt16 Raw);

public class TemperatureRecordingAdapter(
    [FromServices] ITemperatureSensorService temperatureSensorService,
    ILogger<TemperatureRecordingAdapter> logger) : BaseMqttAdapter<TemperatureRecording>
{
    public override string Topic => "temperature/recording";

    protected override async Task HandleAsync(TemperatureRecording temperatureRecording)
    {
        try
        {
            await temperatureSensorService.SaveTemperatureRecording(new SaveTemperatureRecordingCmd(
                DateTime.Parse(temperatureRecording.Timestamp),
                temperatureRecording.DeviceId,
                temperatureRecording.Raw));
        } 
        catch (Exception ex)
        {
            throw new Exception($"MQTT adapter: Error saving temperature recording: {ex.Message}");
        }

        logger.LogInformation(
            $"MQTT adapter: {temperatureRecording.DeviceId} {temperatureRecording.Raw} {temperatureRecording.Timestamp}");
    }
}
