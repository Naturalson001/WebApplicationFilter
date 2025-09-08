using WebApplicationFilter.Api.src.Models.Enum;

namespace WebApplicationFilter.Api.src.Models
{
    public class ThreatResult
    {
        public bool IsThreat { get; set; }
        public int ThreatScore { get; set; }
        public List<string> MatchedPatterns { get; set; } = new List<string>();
        public bool IsBlocked { get; set; }
        public string BlockReason { get; set; } = string.Empty;
        public List<ThreatDetail> ThreatDetails { get; set; } = new List<ThreatDetail>();
    }
}