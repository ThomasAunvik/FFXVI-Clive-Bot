using CliveBot.Bot.Commands;
using CliveBot.Bot.Handler.Utils;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
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

        public static Task Log(LogMessage message)
        {
            Console.WriteLine(message);

            return Task.CompletedTask;
        }

        public async Task Ready()
        {
            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), provider);

#if !DEBUG
            await interactionService.RegisterCommandsGloballyAsync();
#else
            await interactionService.RegisterCommandsToGuildAsync(466048423884226572);
            await interactionService.RegisterCommandsToGuildAsync(1070690445623042108);
            await interactionService.RegisterCommandsToGuildAsync(755902027829215272);
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

        private async Task InteractionExecuted(ICommandInfo info, IInteractionContext ctx, IResult result)
        {
            if (result.IsSuccess) return;
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
            
            Console.WriteLine(result.ErrorReason);
        }
    }
}
