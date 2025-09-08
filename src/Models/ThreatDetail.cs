using WebApplicationFilter.Api.src.Models.Enum;

namespace WebApplicationFilter.Api.src.Models
{
    public class ThreatDetail
    {
        public string Pattern { get; set; } = string.Empty;
        public int Score { get; set; } = 10;
        public string Category { get; set; } = ThreatCategory.Generic.ToString();
    }
    
}