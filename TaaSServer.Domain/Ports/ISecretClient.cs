namespace TaaSServer.Domain.Ports;

public interface ISecretClient
{
    Task<string> GetSecret(string key);
}
