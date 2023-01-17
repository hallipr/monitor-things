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
    public IActionResult Get()
    {
        if(Request.Query.ContainsKey("error"))
        {
            var errorDescription = Request.Query["error_description"].FirstOrDefault();
            var error = Request.Query["error"].FirstOrDefault();

            _logger.LogError("Error: {Error}, Description {ErrorDescription}", error, errorDescription);
            return this.BadRequest($"{error}: {errorDescription}");
        }

        var code = Request.Query["code"].FirstOrDefault();
        _logger.LogInformation("Code: {Code}", code);
        return Ok();
    }
}
