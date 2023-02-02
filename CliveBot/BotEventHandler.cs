using CliveBot.Bot.Commands;
using CliveBot.Bot.Handler.Utils;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CliveBot.Bot
{
    internal class BotEventHandler
    {
        private readonly IServiceProvider provider;

        public BotEventHandler(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public static Task Log(LogMessage message)
        {
            Console.WriteLine(message);

            return Task.CompletedTask;
        }

        public async Task Ready()
        {
            var client = provider.GetRequiredService<DiscordSocketClient>();
            var interactionService = provider.GetRequiredService<InteractionService>();
            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
            await interactionService.RegisterCommandsToGuildAsync(466048423884226572);

            client.InteractionCreated += async interaction =>
            {
                var scope = provider.CreateScope();
                var ctx = new SocketInteractionContext(client, interaction);

                interactionService.InteractionExecuted += InteractionExecuted;

                var result = await interactionService.ExecuteCommandAsync(ctx, scope.ServiceProvider);
            };
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

            if (ctx.Interaction.HasResponded)
            {
                await ctx.Interaction.ModifyOriginalResponseAsync((e) =>
                {
                    e.Embed = errorEmbed.Build();
                });
            }
            else
            {
                await ctx.Interaction.RespondAsync(embed: errorEmbed.Build());
            }
        }
    }
}
