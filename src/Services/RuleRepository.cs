using System.Data;
using WebApplicationFilter.Api.src.DataAccess;
using WebApplicationFilter.Api.src.Services.Interface;
using WebApplicationFilter.Api.src.Models;
using Mysqlx;
using System.Net;

namespace WebApplicationFilter.Api.src.Services
{
    public class RuleRepository : IRuleRepository
    {
        private readonly ISecurityDbContext _context;

        public RuleRepository(ISecurityDbContext context)
        {
            _context = context;

        }

        public async Task<Response<List<SecurityRule>>> GetAllRulesAsync()
        {
            Response<List<SecurityRule>> response = new();

            var result = await _context.GetAllSecurityRulesAsync();

            if (result.Data.Rows.Count == 0)
            {
                ErrorResponse.SetErrorResponse(
                    response,
                    "No security rules found.",
                    HttpStatusCode.NotFound
                );
                return response;
            }

            var rules = new List<SecurityRule>();

            foreach (DataRow row in result.Data.Rows)
            {
                var rule = new SecurityRule
                {
                    Id = row["id"].ToString() ?? string.Empty,
                    IsBlocked = Convert.ToBoolean(row["is_blocked"]),
                    Reason = row["reason"].ToString() ?? string.Empty,
                    ThreatScore = Convert.ToInt32(row["threat_score"]),
                    MatchedPatterns = row["matched_patterns"] switch
                    {
                        string[] arr => arr.ToList(),
                        string raw => [.. raw.Trim('{', '}')
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.Trim())],
                        _ => []
                    }


                };

                rules.Add(rule);
            }

            response.Data = rules;
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Description = "Security rules retrieved successfully.";

            return response;
        }


        public async Task<SecurityRule?> GetRuleByIdAsync(string id)
        {
            var result = await _context.GetSecurityRulesAsync(id);

            if (result.Data.Rows.Count == 0)
                return null;

            var row = result.Data.Rows[0];

            var securityRule = new SecurityRule
            {
                Id = row["id"].ToString() ?? string.Empty,
                IsBlocked = Convert.ToBoolean(row["is_blocked"]),
                Reason = row["reason"].ToString() ?? string.Empty,
                ThreatScore = Convert.ToInt32(row["threat_score"]),
                MatchedPatterns = row["matched_patterns"] switch
                {
                    string[] arr => arr.ToList(),
                    string raw => [.. raw.Trim('{', '}')
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(p => p.Trim())],
                    _ => []
                }
            };

            return securityRule;
        }

        public async Task<SecurityRule> CreateRuleAsync(SecurityRule rule)
        {
            var result = await _context.CreateSecurityRulesAsync(rule);
            return rule;
        }

        public async Task<SecurityRule?> UpdateRuleAsync(string id, SecurityRule rule)
        {
            if (id != rule.Id) return null;

            await _context.UpdateSecurityRulesAsync(rule);
            return rule;
        }

        public async Task<bool> DeleteRuleAsync(string id)
        {
            var result = await _context.GetSecurityRulesAsync(id);

            if (result.Data.Rows.Count == 0)
                return false;

            var row = result.Data.Rows[0];
            var ruleId = row["id"].ToString() ?? string.Empty;

            var deleteResult = await _context.DeleteSecurityRulesAsync(ruleId);
            return deleteResult.Success;
        }
    }
}