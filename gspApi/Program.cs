
using System.Globalization;
using System.Threading.RateLimiting;
using gspAPI.BusTableAPI;
using gspAPI.DbContexts;
using gspAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.HttpOverrides;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Async(a => a.Console())
    .WriteTo.Async(a => a.File("logs/log.txt", rollingInterval: RollingInterval.Day))
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command",LogEventLevel.Warning)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MysqlContext>(optionsBuilder =>
{
    var connString = builder.Configuration.GetConnectionString("Default");
    optionsBuilder.UseMySql(connString,
        ServerVersion.AutoDetect(connString),
        contextOptionsBuilder =>
        {
            contextOptionsBuilder.EnableRetryOnFailure();
        } );
    // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    
    
    optionsBuilder.EnableSensitiveDataLogging();
});
builder.Services.AddScoped<IBusTableRepository, BusTableRepository>();
builder.Services.AddScoped<IBusTableGetter, BusTableGetter>();
builder.Services.AddHostedService<BusLocationPingerService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => 
        {
            
            //policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback).AllowAnyHeader().AllowAnyMethod();
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});


builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = (context, _) =>
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                ((int) retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
        }

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
            .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
            .LogWarning("OnRejected: {GetUserEndPoint}", GetUserEndPoint(context.HttpContext));

        return new ValueTask();
    };    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("fixedPings",
        context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions()
                {
                    PermitLimit = 10,
                    Window = TimeSpan.FromSeconds(3)
                }

            ));

    options.AddPolicy("fixedLatest",
        context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.Connection.RemoteIpAddress?.ToString(),
                factory: _ => new FixedWindowRateLimiterOptions()
                {
                    PermitLimit = 1,
                    Window = TimeSpan.FromSeconds(30)
                }

            ));
});
var app = builder.Build();




app.UseCors();
app.UseForwardedHeaders();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.Run();
static string GetUserEndPoint(HttpContext context) =>
    $"User {context.User.Identity?.Name ?? "Anonymous"} endpoint:{context.Request.Path}"
    + $" {context.Connection.RemoteIpAddress}";