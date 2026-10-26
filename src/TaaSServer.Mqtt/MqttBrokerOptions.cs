namespace TaaSServer.Mqtt;

public class MqttBrokerOptions
{
    public const string SectionName = "MqttBroker";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 8883;
    public string CertSecretName { get; set; } = "mqtt_cert";
}
