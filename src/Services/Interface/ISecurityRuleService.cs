namespace WebApplicationFilter.Api.src.Services.Interface
{
    public interface ISecurityRuleService
    {
        ThreatResult EvaluateRequest(string requestContent);
        void LoadRules(IEnumerable<SecurityRule> rules);
        IEnumerable<SecurityRule> GetRules();

    }
}