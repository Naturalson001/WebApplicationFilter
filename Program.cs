using DbConnectAdapter;
using Microsoft.AspNetCore.Builder;
using Serilog;
using WebApplicationFilter.Api.src.DataAccess;
using WebApplicationFilter.Api.src.Services;
using WebApplicationFilter.Api.src.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog FIRST
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File($"Logs/log-{DateTime.Now:yyyy-MM-dd}.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Read configuration
var configuration = builder.Configuration; // Use the built-in configuration

// Configure database connection
var connectionSettings = configuration.GetSection("Connection").Get<Connection>();
if (connectionSettings == null)
{
    throw new InvalidOperationException("Connection settings not found in configuration");
}
builder.Services.AddSingleton<IConnection>(connectionSettings);
builder.Services.AddSingleton<IDbContext, DbContext>();

// Add your services - IMPORTANT: Add SecurityDbContext FIRST
builder.Services.AddScoped<ISecurityDbContext, SecurityDbContext>(); 
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ISecurityRuleService, SecurityRuleService>();
builder.Services.AddScoped<IRuleRepository, RuleRepository>();

// Add framework services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplicationFilter API v1");
        options.RoutePrefix = "swagger"; 
    });
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();