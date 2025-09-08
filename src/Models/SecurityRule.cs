namespace WebApplicationFilter.Api.src.Models
{
    public class SecurityRule
    {
        public string Id { get; set; } 
        public bool IsBlocked { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int ThreatScore { get; set; }
        public List<string> MatchedPatterns { get; set; } = [];
    }
}