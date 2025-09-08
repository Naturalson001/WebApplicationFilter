
using Microsoft.AspNetCore.Authorization;

namespace WebApplicationFilter.Api.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        private readonly ILogger<SecureController> _logger;

        public SecureController(ILogger<SecureController> logger)
        {
            _logger = logger;
        }

        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                service = "Security Filter API"
            });
        }

        // [HttpGet("stats")]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> GetStats()
        // {
        //     // You can add stats endpoint later
        //     return Ok(new
        //     {
        //         totalRules = 0,
        //         blockedRequests = 0,
        //         averageThreatScore = 0
        //     });
        // }
        
    }
}