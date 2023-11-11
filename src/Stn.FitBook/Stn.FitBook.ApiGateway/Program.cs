using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog.Enrichers;
using Serilog;
using Stn.FitBook.Service;

var builder = WebApplication.CreateBuilder(args);


ConfigurationManager config = builder.Configuration;


builder.Host.UseSerilog();
Log.Logger = new LoggerConfiguration()
        .Enrich.With(new ThreadIdEnricher())
        .ReadFrom.Configuration(config)
        .CreateLogger();
builder.Services.AddSingleton(Log.Logger);


builder.Configuration.AddJsonFile("ocelot.json", false, false);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);


var app = builder.Build();

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
