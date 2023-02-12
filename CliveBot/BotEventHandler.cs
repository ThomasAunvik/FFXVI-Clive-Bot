using CliveBot.Bot.Commands;
using CliveBot.Bot.Handler.Utils;
using CliveBot.Database;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Serilog;
using Serilog.Context;
using System;
using System.Reflection;

namespace CliveBot.Bot
{
    internal class BotEventHandler
    {
        private readonly IServiceProvider provider;
        private readonly DiscordSocketClient client;
        private readonly InteractionService interactionService;

        public BotEventHandler(IServiceProvider provider, DiscordSocketClient client, InteractionService interactionService)
        {
            this.provider = provider;
            this.client = client;
            this.interactionService = interactionService;
        }

        public static Task LogDiscord(LogMessage message)
        {
            LogContext.PushProperty("SourceContext", "Discord");
            switch(message.Severity)
            {
                case LogSeverity.Critical:
                    Log.Logger.Fatal(message.Exception, message.Message);
                    break;
                case LogSeverity.Error:
                    Log.Logger.Error(message.Exception, message.Message);
                    break;
                case LogSeverity.Warning:
                    Log.Logger.Warning(message.Exception, message.Message);
                    break;
                case LogSeverity.Debug:
                    Log.Logger.Debug(message.Exception, message.Message);
                    break;
                case LogSeverity.Info:
                    Log.Logger.Information(message.Exception, message.Message);
                    break;
                case LogSeverity.Verbose:
                    Log.Logger.Verbose(message.Exception, message.Message);
                    break;
                default:
                    Log.Logger.Information(message.Message);
                    break;

            }

            return Task.CompletedTask;
        }

        public async Task Ready()
        {
            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), provider);

#if !DEBUG
            await interactionService.RegisterCommandsGloballyAsync();
#else
            await interactionService.RegisterCommandsToGuildAsync(1070690445623042108);
#endif

            interactionService.InteractionExecuted += InteractionExecuted;

            client.InteractionCreated += async interaction =>
            {
                if (interaction is SocketMessageComponent) return;

                var scope = provider.CreateScope();
                var ctx = new SocketInteractionContext(client, interaction);

                var result = await interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
            };

            client.ButtonExecuted += ButtonExecuted;
        }

        internal async Task ButtonExecuted(SocketMessageComponent interaction)
        {
            var scope = provider.CreateScope();
            var ctx = new SocketInteractionContext<SocketMessageComponent>(client, interaction);

            await interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
        }

        private Task InteractionExecuted(ICommandInfo info, IInteractionContext ctx, IResult result)
        {
            if (result.IsSuccess) return Task.CompletedTask;
            EmbedHandler errorEmbed = new(ctx.User);

            if (result.Error == InteractionCommandError.UnmetPrecondition && ctx is ISlashCommandInteraction slashInteraction)
            {
                Help.HelpCommandSearch(errorEmbed, slashInteraction.Data.Name, provider, ctx);
            }
            else if (result is ExecuteResult execResult)
            {
                errorEmbed.Title = "Critical Error";
                errorEmbed.Description = execResult.ErrorReason;
            }

            LogContext.PushProperty("SourceContext", "Discord");
            Log.Error(result.ErrorReason);

            return Task.CompletedTask;
        }
    }
}
