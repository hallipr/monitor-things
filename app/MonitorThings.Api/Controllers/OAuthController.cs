using Microsoft.AspNetCore.Mvc;

namespace MonitorThings.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OAuthController : ControllerBase
{
    private readonly ILogger<OAuthController> _logger;

    public OAuthController(ILogger<OAuthController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAuthCallback")]
    public string Get()
    {
        return "ok";
    }
}
