using Microsoft.AspNetCore.Mvc;
using Ordering.Api.Data;

namespace Ordering.Api.Controllers;

/// <summary>
/// The health controller for the ordering system.
/// </summary>
[ApiController]
[Route("health")]
public class HealthController : ControllerBase 
{
    /// <summary>
    /// The database context for the ordering system.
    /// </summary>
    private readonly OrderingDbContext _context;
    /// <summary>
    /// The logger for the health controller.
    /// </summary>
    private readonly ILogger<HealthController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HealthController"/> class.
    /// </summary>
    /// <param name="context">The database context for the ordering system.</param>
    /// <param name="logger">The logger for the health controller.</param>
    public HealthController(OrderingDbContext context, ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets the health of the ordering system.
    /// </summary>
    /// <returns>The health of the ordering system.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<object>> GetHealth()
    {
        try
        {
            // Check database connectivity
            await _context.Database.CanConnectAsync();

            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                checks = new
                {
                    database = "Healthy"
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return StatusCode(503, new
            {
                status = "Unhealthy",
                timestamp = DateTime.UtcNow,
                error = ex.Message
            });
        }
    }
}




