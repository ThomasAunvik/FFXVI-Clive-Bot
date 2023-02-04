using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Discord.Interactions;
using CliveBot.Database;
using CliveBot.Bot;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var config = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.None,
};

var dbConnString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_URL");
if (string.IsNullOrWhiteSpace(dbConnString))
{
    throw new Exception("No Database Connection String, Example: 'Host=my_host;Database=my_db;Username=my_user;Password=my_pw'");
}

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.File(
                Path.Combine(Directory.GetCurrentDirectory(), "logs/bot.log"),
                rollingInterval: RollingInterval.Day,
                shared: true
            )
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [{SourceContext:l}] {Message}{NewLine}{Exception}")
            .CreateLogger();

var collection = new ServiceCollection()
    .AddSingleton(config)
    .AddLogging(builder =>
    {
        builder.AddSerilog();
    })
    .AddSingleton<DiscordSocketClient>()
    .AddSingleton<InteractionService>()
    .AddSingleton<BotEventHandler>()
    .AddDbContext<ApplicationDbContext>((config) =>
    {
        config.UseNpgsql(dbConnString);
#if DEBUG
        config.EnableSensitiveDataLogging();
#endif
    });

var provider = collection.BuildServiceProvider();

var migrate = Environment.GetEnvironmentVariable("MIGRATION_MODE");
if (migrate == "always")
{
    var db = provider.GetRequiredService<ApplicationDbContext>();
    Log.Logger.Information("Migrating...");
    await db.Database.MigrateAsync();
    Log.Logger.Information("Migrating Finished");
}

var client = provider.GetRequiredService<DiscordSocketClient>();
var eventHandler = provider.GetRequiredService<BotEventHandler>();

var token = Environment.GetEnvironmentVariable("DISCORD_BOT_TOKEN");

// Configure Callbacks
client.Log += BotEventHandler.LogDiscord;
client.Ready += eventHandler.Ready;
client.ButtonExecuted += eventHandler.ButtonExecuted;

// Start Service
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

// Start Web Host
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services.AddHealthChecks();
    builder.Services.AddApplicationInsightsTelemetry();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    app.UseHealthChecks("/healthz");
    app.UseStaticFiles();

    app.MapGet("/", () => "Hello World!");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}