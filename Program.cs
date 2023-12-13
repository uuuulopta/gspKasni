
using gspAPI.DbContexts;
using gspAPI.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
    
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Async(a => a.Console())
    .WriteTo.Async(a => a.File("logs/log.txt", rollingInterval: RollingInterval.Day))
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();


// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMiniProfiler().AddEntityFramework();
builder.Services.AddDbContext<MysqlContext>(optionsBuilder =>
{
    var connString = builder.Configuration.GetConnectionString("Default");
    optionsBuilder.UseMySql(connString,
        ServerVersion.AutoDetect(connString));
    // optionsBuilder.EnableSensitiveDataLogging();
});
builder.Services.AddScoped<IBusTableRepository, BusTableRepository>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseMiniProfiler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
