using Microsoft.AspNetCore.Authorization;
using WebApplicationFilter.Api.src.Services.Interface;

namespace WebApplicationFilter.Api.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityLogsController : ControllerBase
    {
        private readonly ILogService _logService;
        private readonly ILogger<SecurityLogsController> _logger;

        public SecurityLogsController(ILogService logService, ILogger<SecurityLogsController> logger)
        {
            _logService = logService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                var logs = await _logService.GetLogsAsync();
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving security logs");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("filtered")]
        public async Task<IActionResult> GetFilteredLogs(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int minThreatScore = 0)
        {
            try
            {
                var logs = await _logService.GetLogsByDateRangeAsync(startDate ,endDate );
                var filteredLogs = logs.Where(log =>
                    (startDate == null || log.CreatedAt >= startDate) &&
                    (endDate == null || log.CreatedAt <= endDate) &&
                    log.ThreatScore >= minThreatScore
                ).ToList();

                return Ok(filteredLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving filtered security logs");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLog(string id)
        {
            try
            {
                var result = await _logService.DeleteLogAsync(id);
                return result ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting security log {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLogById(string id)
    {
        try
        {
            var log = await _logService.GetLogByIdAsync(id);
            return log != null ? Ok(log) : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving security log {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

}
}