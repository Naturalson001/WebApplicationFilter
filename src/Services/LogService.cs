using WebApplicationFilter.Api.src.DataAccess;
using WebApplicationFilter.Api.src.Services.Interface;
using System.Data;

namespace WebApplicationFilter.Api.src.Services
{
    public class LogService : ILogService
    {
        private readonly ISecurityDbContext _db;
        // private readonly ILogger<LogService> _logger;

        public LogService(ISecurityDbContext db)
        {
            _db = db;
            //_logger = logger;
        }

        public async Task LogAttempt(HttpContext context, ThreatResult result)
        {
            var log = new SecurityLog
            {
                ClientIp = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
                Endpoint = context.Request.Path,
                PayloadSnippet = await GetPayloadSnippet(context.Request),
                ThreatScore = result.ThreatScore,
                Reason = result.BlockReason,
                MatchedPatterns = result.MatchedPatterns,
                WasBlocked = result.IsBlocked
            };

            await _db.InsertSecurityLogAsync(log, "LogAttempt");
        }

        public async Task<List<SecurityLog>> GetLogsAsync()
        {
            var result = await _db.GetAllSecurityLogsAsync();

            if (!result.Success || result.Data.Rows.Count == 0)
            {
                // _logger.LogWarning("No security logs found or DB query failed: {Message}", result.Message);
                return new List<SecurityLog>();
            }

            var logs = new List<SecurityLog>();

            foreach (DataRow row in result.Data.Rows)
            {
                logs.Add(new SecurityLog
                {
                    Id = row["id"].ToString() ?? string.Empty,
                    ClientIp = row["client_ip"].ToString() ?? string.Empty,
                    Endpoint = row["endpoint"].ToString() ?? string.Empty,
                    PayloadSnippet = row["payload_snippet"].ToString() ?? string.Empty,
                    ThreatScore = Convert.ToInt32(row["threat_score"]),
                    Reason = row["reason"].ToString() ?? string.Empty,
                    MatchedPatterns = row["matched_patterns"].ToString()?
                                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(p => p.Trim())
                                        .ToList() ?? new List<string>(),
                    WasBlocked = Convert.ToBoolean(row["was_blocked"])
                });
            }

            return logs;
        }

        private static async Task<string> GetPayloadSnippet(HttpRequest request)
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0;
            return body.Length > 200 ? string.Concat(body.AsSpan(0, 200), "...") : body;
        }

        public async Task<SecurityLog?> GetLogByIdAsync(string id)
        {
            try
            {
                var result = await _db.GetSecurityLogByIdAsync(id);
                if (!result.Success || result.Data.Rows.Count == 0)
                {
                    return null;
                }

                var row = result.Data.Rows[0];
                return ConvertDataRowToSecurityLog(row);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<SecurityLog>> GetLogsByDateRangeAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var result = await _db.GetAllSecurityLogsAsync();
                if (!result.Success)
                {
                    return new List<SecurityLog>();
                }

                var logs = ConvertDataTableToSecurityLogs(result.Data);
                return [.. logs.Where(log => log.CreatedAt >= startDate && log.CreatedAt <= endDate)];
            }
            catch (Exception)
            {
                return new List<SecurityLog>();
            }
        }

        public async Task<bool> DeleteLogAsync(string id)
        {
            try
            {
                var result = await _db.DeleteSecurityLogAsync(id);
                return result.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static SecurityLog ConvertDataRowToSecurityLog(DataRow row)
        {
            return new SecurityLog
            {
                Id = row["id"].ToString() ?? string.Empty,
                ClientIp = row["client_ip"].ToString() ?? string.Empty,
                Endpoint = row["endpoint"].ToString() ?? string.Empty,
                PayloadSnippet = row["payload_snippet"].ToString() ?? string.Empty,
                ThreatScore = Convert.ToInt32(row["threat_score"]),
                Reason = row["reason"].ToString() ?? string.Empty,
                MatchedPatterns = row["matched_patterns"].ToString()?
                                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                    .Select(p => p.Trim())
                                    .ToList() ?? new List<string>(),
                WasBlocked = Convert.ToBoolean(row["was_blocked"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                UpdatedAt = Convert.ToDateTime(row["updated_at"])
            };
        }
        private List<SecurityLog> ConvertDataTableToSecurityLogs(DataTable dataTable)
        {
            var logs = new List<SecurityLog>();
            foreach (DataRow row in dataTable.Rows)
            {
                logs.Add(ConvertDataRowToSecurityLog(row));
            }
            return logs;
        }
    }
}