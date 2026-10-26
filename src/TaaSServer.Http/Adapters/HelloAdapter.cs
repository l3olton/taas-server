using Microsoft.AspNetCore.Mvc;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Http.Adapters;

[ApiController]
[Route("/hello")]
public class HelloAdapter([FromServices] IHelloService helloService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(helloService.SayHello());
    }
}