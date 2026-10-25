using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using TaaSServer.Domain.Ports;

namespace TaaSServer.App.SecretClient;

public class SecretsManagerSecretClient : ISecretClient
{
    private readonly IAmazonSecretsManager _client;

    public SecretsManagerSecretClient(IAmazonSecretsManager client)
    {
        _client = client;
    }

    public async Task<string> GetSecret(string key)
    {
        var request = new GetSecretValueRequest
        {
            SecretId = key
        };

        var response = await _client.GetSecretValueAsync(request);
        return response.SecretString;
    }
}
