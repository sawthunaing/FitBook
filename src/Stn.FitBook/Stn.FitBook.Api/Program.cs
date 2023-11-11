using Microsoft.EntityFrameworkCore;
using Serilog.Enrichers;
using Serilog;
using Stn.FitBook.Service;
using Stn.FitBook.Repo.Ef.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.MySql.Core;
using System.Configuration;
using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;
using Stn.FitBook.Domain.Middlewares;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager config = builder.Configuration;

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseMySQL(config.GetConnectionString("DefaultConnection"));
}).AddHealthChecks();

builder.Services.AddJwtAuthentication(builder.Configuration);

string hangfireConnectionString = config.GetConnectionString("DefaultConnection");
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(
        new MySqlStorage(
            hangfireConnectionString,
            new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(10),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 25000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablePrefix = "Hangfire",
            }
        )
    ));

// Add the processing server as IHostedService
builder.Services.AddHangfireServer(options => options.WorkerCount = 1);
builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
        .Enrich.With(new ThreadIdEnricher())
        .ReadFrom.Configuration(config)
        .CreateLogger();
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddConfig();
builder.Services.AddCors();

builder.Services.AddControllers();
RegisterRedisCache(builder);

var app = builder.Build();

//app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHealthChecks("/healthz");

app.UseHangfireDashboard();

app.MapControllers();

app.Run();

void RegisterRedisCache(WebApplicationBuilder builder)
{
    var redisConnectionString = builder.Configuration.GetSection("RedisConnectionString");
    var multiplexer = ConnectionMultiplexer.Connect(redisConnectionString.Value);
    builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
}
