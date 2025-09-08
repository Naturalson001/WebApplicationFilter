namespace WebApplicationFilter.Api.src.DataAccess
{
    public class SecurityDbSql
    {
        public const string InsertSecurityLog = "INSERT INTO security_logs  (id, client_ip, endpoint, payload_snippet, threat_score, reason, matched_patterns, created_at, updated_at)" +
                                                "VALUES (#, #, #, #, #, #, #, getdate(), getdate())";

        public const string GetAllSecurityLogs = "SELECT id, client_ip, endpoint, payload_snippet, threat_score, reason, matched_patterns, created_at, updated_at, was_blocked FROM security_logs ";
        public const string GetSecurityLogById = "SELECT id, client_ip, endpoint, payload_snippet, threat_score, reason, matched_patterns, created_at, updated_at FROM security_logs " +
                                                " WHERE id = #";
        public const string UpdateSecurityLog = "UPDATE security_logs  SET client_ip = #, endpoint = #, payload_snippet = #, threat_score = #, reason = #, matched_patterns = #, updated_at = getdate()" +
                                                " WHERE id = #";
        public const string DeleteSecurityLog = "DELETE FROM security_logs  WHERE id = #";
        public const string CreateSecurityRule = "INSERT INTO public.security_rules (id, is_blocked, reason, threat_score, matched_patterns, created_at, updated_at)" +
                                                 "VALUES (#, #, #, #, #, getdate(), getdate())";
        public const string GetAllSecurityRules = "SELECT id, is_blocked, reason, threat_score, matched_patterns, created_at, updated_at FROM public.security_rules";
        public const string GetSecurityRuleById = "SELECT id, is_blocked, reason, threat_score, matched_patterns, created_at, updated_at FROM public.security_rules" +
                                                 " WHERE id = #";
        public const string UpdateSecurityRule = "UPDATE public.security_rules SET is_blocked = #, reason = #, threat_score = #, matched_patterns = #, updated_at = getdate()" +
                                                 " WHERE id = #";
        public const string DeleteSecurityRule = "DELETE FROM public.security_rules WHERE id = #";

    }
}