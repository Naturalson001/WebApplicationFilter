using WebApplicationFilter.Api.src.Services.Interface;
using WebApplicationFilter.Api.src.Services;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace WebApplicationFilter.Api.src.Middleware
{
    public class SecurityFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISecurityRuleService _ruleService;
        private readonly ILogService _logService;

        public SecurityFilterMiddleware(RequestDelegate next, ISecurityRuleService ruleService, ILogService logService)
        {
            _next = next;
            _ruleService = ruleService;
            _logService = logService;
        }

        public async Task Invoke(HttpContext context)
        {
            string requestContent = await ReadRequestBody(context.Request);

            var threatResult = _ruleService.EvaluateRequest(requestContent);

            await _logService.LogAttempt(context, threatResult);

            if (threatResult.IsBlocked)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync($"Request blocked: {threatResult.BlockReason}");
                return;
            }

            if (threatResult.IsThreat)
            {
                context.Response.Headers.Append("X-Security-Warning", "Potential threat detected");
            }

            await _next(context);
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            using var reader = new StreamReader(
                request.Body,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            request.Body.Position = 0;

            return body;
        }
    }
}