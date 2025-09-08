using System.Data;

namespace WebApplicationFilter.Api.src.DataAccess
{
    public interface ISecurityDbContext
    {
        Task<ProcessResult<DataTable>> GetSecurityInfoAsync(string userId);
        Task<ProcessResult<string>> InsertSecurityLogAsync(SecurityLog securityLog, string action);
        Task<ProcessResult<string>> UpdateSecurityInfoAsync(SecurityLog securityLog, string info);
        Task<ProcessResult<string>> DeleteSecurityLogAsync(string userId);
        Task<ProcessResult<string>> UpdateSecurityRulesAsync(SecurityRule rule);
        Task<ProcessResult<DataTable>> GetSecurityRulesAsync(string id);
        Task<ProcessResult<string>> DeleteSecurityRulesAsync(string id);
        Task<ProcessResult<string>> CreateSecurityRulesAsync(SecurityRule rule);
        Task<ProcessResult<DataTable>> GetAllSecurityRulesAsync();
        Task<ProcessResult<DataTable>> GetAllSecurityLogsAsync();
        Task<ProcessResult<DataTable>> GetSecurityLogByIdAsync(string id);

    }
}