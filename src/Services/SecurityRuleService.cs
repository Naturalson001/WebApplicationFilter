using System.Text.RegularExpressions;
using WebApplicationFilter.Api.src.Models.Enum;
using WebApplicationFilter.Api.src.Services.Interface;

namespace WebApplicationFilter.Api.src.Services
{
    public class SecurityRuleService : ISecurityRuleService
    {
        private List<SecurityRule> _rules = new List<SecurityRule>();

        public ThreatResult EvaluateRequest(string requestContent)
        {
            var threatResult = new ThreatResult
            {
                IsThreat = false,
                ThreatScore = 0,
                MatchedPatterns = [],
                IsBlocked = false,
                BlockReason = string.Empty,
                ThreatDetails = []
            };

            foreach (var rule in _rules)
            {
                foreach (var pattern in rule.MatchedPatterns)
                {
                    if (Regex.IsMatch(requestContent, pattern, RegexOptions.IgnoreCase))
                    {
                        threatResult.IsThreat = true;
                        threatResult.ThreatScore += rule.ThreatScore;
                        threatResult.MatchedPatterns.Add(pattern);
                        
                        threatResult.ThreatDetails.Add(new ThreatDetail
                        {
                            Pattern = pattern,
                            Score = rule.ThreatScore,
                            Category = ThreatCategory.Generic.ToString() 
                        });

                        if (rule.IsBlocked)
                        {
                            threatResult.IsBlocked = true;
                            threatResult.BlockReason = rule.Reason;
                            return threatResult;
                        }
                    }
                }
            }

            return threatResult;
        }

        public void LoadRules(IEnumerable<SecurityRule> rules)
        {
           _rules.Clear();
           _rules.AddRange(rules);
        }

        public IEnumerable<SecurityRule> GetRules()
        {
            return _rules;
        }
    }
}