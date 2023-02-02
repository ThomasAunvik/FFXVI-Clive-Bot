using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Discord.Interactions;
using CliveBot.Database;
using CliveBot.Bot;

var config = new DiscordSocketConfig()
{
    GatewayIntents = GatewayIntents.None,
};

var dbConnString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_URL");
if (string.IsNullOrWhiteSpace(dbConnString))
{
    throw new Exception("No Database Connection String, Example: 'Host=my_host;Database=my_db;Username=my_user;Password=my_pw'");
}

var collection = new ServiceCollection()
    .AddSingleton(config)
    .AddSingleton<DiscordSocketClient>()
    .AddSingleton<InteractionService>()
    .AddSingleton<BotEventHandler>()
    .AddDbContext<ApplicationDbContext>((config) =>
    {
        config.UseNpgsql(dbConnString);
    });

var provider = collection.BuildServiceProvider();


var client = provider.GetRequiredService<DiscordSocketClient>();
var eventHandler = provider.GetRequiredService<BotEventHandler>();

var token = Environment.GetEnvironmentVariable("DISCORD_APP_TOKEN");

// Configure Callbacks
client.Log += BotEventHandler.Log;
client.Ready += eventHandler.Ready;

// Start Service
await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

await Task.Delay(Timeout.Infinite);