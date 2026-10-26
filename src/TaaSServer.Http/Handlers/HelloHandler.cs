using Microsoft.AspNetCore.Mvc;
using TaaSServer.Domain.Ports;

namespace TaaSServer.Http.Handlers;

[ApiController]
[Route("/hello")]
public class HelloHandler([FromServices] IHelloService helloService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(helloService.SayHello());
    }
}