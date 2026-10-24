using TaaSServer.Domain.Ports;

namespace TaaSServer.Domain.Services;

public class HelloService : IHelloService
{
    public string SayHello()
    {
        return "Hello World!";
    }
}