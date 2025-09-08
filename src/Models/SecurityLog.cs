namespace WebApplicationFilter.Api.src.Models
{
    public class SecurityLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public string ClientIp { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string PayloadSnippet { get; set; } = string.Empty;

        public int ThreatScore { get; set; }
        public string Reason { get; set; } = string.Empty;
        public List<string> MatchedPatterns { get; set; } = [];
        public bool WasBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}