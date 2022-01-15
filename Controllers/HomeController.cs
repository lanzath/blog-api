using Blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    // API Health Check (200 or 500 status code response)
    [HttpGet("")]
    [ApiKey] // Exemplo de custom attribute.
    public IActionResult Get()
        => Ok();
}
