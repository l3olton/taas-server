using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using TaaSServer.Domain.Ports;

namespace TaaSServer.App.SecretClient;

public class SecretsManagerSecretClient(IAmazonSecretsManager client) : ISecretClient
{
    public async Task<string> GetSecret(string key)
    {
        var request = new GetSecretValueRequest
        {
            SecretId = key
        };

        var response = await client.GetSecretValueAsync(request);
        return response.SecretString;
    }
}
