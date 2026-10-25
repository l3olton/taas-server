using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MQTTnet;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Mqtt.Handlers;

public record TemperatureRecording(string Timestamp, string DeviceId, float Temperature);

public class TemperatureRecordingHandler(
    [FromServices] ITemperatureSensorService temperatureSensorService,
    ILogger<TemperatureRecordingHandler> logger) : BaseMqttHandler
{
    public override string Topic => "temperature/recording";

    public override async Task HandleAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        var payload = args.ApplicationMessage.ConvertPayloadToString();
        logger.LogInformation($"Payload string: {payload}");
        
        TemperatureRecording temperatureRecording;
        try
        {
            temperatureRecording = JsonSerializer.Deserialize<TemperatureRecording>(payload);
        }
        catch (Exception ex)
        {
            throw new Exception($"MQTT handler: JSON payload deserialization failed: {ex.Message}");
        }
        logger.LogInformation("MQTT handler: JSON payload deserialized");

        try
        {
            await temperatureSensorService.SaveTemperatureRecording(new TemperatureRecordingCmd(
                DateTime.Parse(temperatureRecording.Timestamp),
                temperatureRecording.DeviceId,
                temperatureRecording.Temperature));
        } 
        catch (Exception ex)
        {
            throw new Exception($"MQTT Handler: Error saving temperature recording: {ex.Message}");
        }

        logger.LogInformation(
            $"MQTT Handler: {temperatureRecording.DeviceId} {temperatureRecording.Temperature} {temperatureRecording.Timestamp}");
    }
}
