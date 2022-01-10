using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    // API Health Check (200 or 500 status code response)
    [HttpGet("")]
    public IActionResult Get()
        => Ok();
}
