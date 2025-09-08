
namespace WebApplicationFilter.Api.src.Services.Interface
{
    public interface IRuleRepository
    {
       Task<Response<List<SecurityRule>>> GetAllRulesAsync();
       Task<SecurityRule?> GetRuleByIdAsync(string id);
       Task<SecurityRule> CreateRuleAsync(SecurityRule rule);
       Task<SecurityRule?> UpdateRuleAsync(string id, SecurityRule rule);
       Task<bool> DeleteRuleAsync(string id);
    }
}