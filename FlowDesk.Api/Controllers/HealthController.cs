using Microsoft.AspNetCore.Mvc;

namespace FlowDesk.Api.Controllers;

[ApiController]
[Route("api")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> Index()
    {
        return Ok(new
        {
            service = "FlowDesk Task Board API",
            version = "1.0.0",
            status = "running",
            timestamp = DateTime.UtcNow,
            endpoints = new
            {
                tasks = "/api/tasks",
                projects = "/api/projects",
                users = "/api/users",
                documentation = "/openapi/v1.json"
            }
        });
    }

    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> Health()
    {
        _logger.LogInformation("Health check requested");
        return Ok(new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow
        });
    }
}
