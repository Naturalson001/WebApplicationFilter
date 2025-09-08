namespace WebApplicationFilter.Api.src.Services.Interface
{
    public interface ILogService
    {
        Task LogAttempt(HttpContext context, ThreatResult result);
        Task<List<SecurityLog>> GetLogsAsync();
        Task<SecurityLog?> GetLogByIdAsync(string id); 
        Task<List<SecurityLog>> GetLogsByDateRangeAsync(DateTime? startDate, DateTime? endDate); 
        Task<bool> DeleteLogAsync(string id); 
    }
}