
using Microsoft.AspNetCore.Authorization;
using WebApplicationFilter.Api.src.Services.Interface;

namespace WebApplicationFilter.Api.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
    public class RulesController : ControllerBase
    {
        private readonly IRuleRepository _ruleRepository;
        private readonly ISecurityRuleService _ruleService;
        private readonly ILogger<RulesController> _logger;

        public RulesController(IRuleRepository ruleRepository, ISecurityRuleService ruleService, ILogger<RulesController> logger)
        {
            _ruleRepository = ruleRepository;
            _ruleService = ruleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRules()
        {
            try
            {
                var rules = await _ruleRepository.GetAllRulesAsync();
                return Ok(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving security rules");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRuleById(string id)
        {
            try
            {
                var rule = await _ruleRepository.GetRuleByIdAsync(id);
                return rule != null ? Ok(rule) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving security rule {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRule([FromBody] SecurityRule rule)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdRule = await _ruleRepository.CreateRuleAsync(rule);

                // Reload rules into the service
                await ReloadRulesAsync();

                return CreatedAtAction(nameof(GetRuleById), new { id = createdRule.Id }, createdRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating security rule");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRule(string id, [FromBody] SecurityRule rule)
        {
            if (id != rule.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedRule = await _ruleRepository.UpdateRuleAsync(id, rule);
                if (updatedRule == null)
                    return NotFound();

                // Reload rules into the service
                await ReloadRulesAsync();

                return Ok(updatedRule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating security rule {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRule(string id)
        {
            try
            {
                var result = await _ruleRepository.DeleteRuleAsync(id);
                if (!result)
                    return NotFound();

                await ReloadRulesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting security rule {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("reload")]
        public async Task<IActionResult> ReloadRules()
        {
            try
            {
                await ReloadRulesAsync();
                return Ok("Rules reloaded successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reloading rules");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task ReloadRulesAsync()
        {
            var result = await _ruleRepository.GetAllRulesAsync();

            if (result.Success && result.Data != null)
            {
                _ruleService.LoadRules(result.Data);
            }
            else
            {
            
                Console.WriteLine($"Failed to load rules: {result.Description}");
            }
        }

    }

}