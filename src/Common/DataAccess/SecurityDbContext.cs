

using System.Data;
using System.Net;
using DbConnectAdapter;

namespace WebApplicationFilter.Api.src.DataAccess
{
    public class SecurityDbContext : ISecurityDbContext
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<SecurityDbContext> _logger;

        public SecurityDbContext(IDbContext dbContext, ILogger<SecurityDbContext> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ProcessResult<string>> CreateSecurityRulesAsync(SecurityRule rule)
        {
            var result = await _dbContext.CommandAsync(SecurityDbSql.CreateSecurityRule, [rule.Id, rule.IsBlocked, rule.Reason, rule.ThreatScore, string.Join(", ", rule.MatchedPatterns)]);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to create security rule for user {UserId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    rule.Id, result.Message, result.Error, result.Command, result.Parameter);
                return ProcessResult<string>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<string>.SuccessResult("Security rule created successfully.");
        }

        public async Task<ProcessResult<string>> DeleteSecurityLogAsync(string userId)
        {
            var result = await _dbContext.CommandAsync(SecurityDbSql.DeleteSecurityLog, [userId]);
            if (!result.Success)
            {
                _logger.LogError(
                     "Failed to delete security info for user {UserId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                     userId, result.Message, result.Error, result.Command, result.Parameter);
                return ProcessResult<string>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<string>.SuccessResult("Customer address deleted successfully.");

        }

        public async Task<ProcessResult<string>> DeleteSecurityRulesAsync(string id)
        {
            var result = await _dbContext.CommandAsync(SecurityDbSql.DeleteSecurityRule, [id]);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to delete security rule {RuleId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    id, result.Message, result.Error, result.Command, result.Parameter);
                return ProcessResult<string>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<string>.SuccessResult("Security rule deleted successfully.");
        }

        public async Task<ProcessResult<DataTable>> GetAllSecurityLogsAsync()
        {
            var result = await _dbContext.QueryTableAsync(SecurityDbSql.GetAllSecurityLogs);
            if (!result.Success)
            {
                _logger.LogError("Failed to get all security logs. Message: {Message}", result.Message);
                return ProcessResult<DataTable>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<DataTable>.SuccessResult(result.DataTable);
        }

        public async Task<ProcessResult<DataTable>> GetAllSecurityRulesAsync()
        {
            var result = await _dbContext.QueryTableAsync(SecurityDbSql.GetAllSecurityRules);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to get all security rules. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    result.Message, result.Error, result.Query, result.Parameter);
                return ProcessResult<DataTable>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<DataTable>.SuccessResult(result.DataTable);
        }

        public async Task<ProcessResult<DataTable>> GetSecurityInfoAsync(string userId)
        {
            var result = await _dbContext.QueryTableAsync(SecurityDbSql.GetSecurityLogById, [userId]);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to get security info for user {UserId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    userId, result.Message, result.Error, result.Query, result.Parameter);
                return ProcessResult<DataTable>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<DataTable>.SuccessResult(result.DataTable);
        }

        public async Task<ProcessResult<DataTable>> GetSecurityLogByIdAsync(string id)
        {
            var result = await _dbContext.QueryTableAsync(SecurityDbSql.GetSecurityLogById, [id]);
            if (!result.Success)
            {
                _logger.LogError("Failed to get security log {Id}. Message: {Message}", id, result.Message);
                return ProcessResult<DataTable>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<DataTable>.SuccessResult(result.DataTable);
        }

        public async Task<ProcessResult<DataTable>> GetSecurityRulesAsync(string id)
        {
            var result = await _dbContext.QueryTableAsync(SecurityDbSql.GetSecurityRuleById, [id]);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to get security rule {RuleId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    id, result.Message, result.Error, result.Query, result.Parameter);
                return ProcessResult<DataTable>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<DataTable>.SuccessResult(result.DataTable);
        }

        public async Task<ProcessResult<string>> InsertSecurityLogAsync(SecurityLog securityLog, string action)
        {
            var result = await _dbContext.CommandAsync(SecurityDbSql.InsertSecurityLog, [securityLog.Id, securityLog.ClientIp,
                                                      securityLog.Endpoint, securityLog.PayloadSnippet, securityLog.ThreatScore,
                                                        securityLog.Reason, string.Join(", ", securityLog.MatchedPatterns)]);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to insert security log for user {UserId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    securityLog.Id, result.Message, result.Error, result.Command, result.Parameter);
                return ProcessResult<string>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<string>.SuccessResult("Security log inserted successfully.");
        }

        public async Task<ProcessResult<string>> UpdateSecurityInfoAsync(SecurityLog securityLog, string info)
        {
            var result = await _dbContext.CommandAsync(SecurityDbSql.UpdateSecurityLog, [securityLog.ClientIp,
                                                      securityLog.Endpoint, securityLog.PayloadSnippet, securityLog.ThreatScore,
                                                        securityLog.Reason, string.Join(", ", securityLog.MatchedPatterns), securityLog.Id]);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to update security info for user {UserId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    securityLog.Id, result.Message, result.Error, result.Command, result.Parameter);
                return ProcessResult<string>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<string>.SuccessResult("Customer address updated successfully.");
        }

        public async Task<ProcessResult<string>> UpdateSecurityRulesAsync(SecurityRule rule)
        {
            var result = await _dbContext.CommandAsync(SecurityDbSql.UpdateSecurityRule, [rule.IsBlocked, rule.Reason, rule.ThreatScore, string.Join(", ", rule.MatchedPatterns), rule.Id]);
            if (!result.Success)
            {
                _logger.LogError(
                    "Failed to update security rule {RuleId}. Message: {Message}, Error: {Error}, Command: {Command}, Parameter: {Parameter}",
                    rule.Id, result.Message, result.Error, result.Command, result.Parameter);
                return ProcessResult<string>.Failure($"Database error: {result.Message}", HttpStatusCode.InternalServerError);
            }
            return ProcessResult<string>.SuccessResult("Security rule updated successfully.");
        }
    }
}