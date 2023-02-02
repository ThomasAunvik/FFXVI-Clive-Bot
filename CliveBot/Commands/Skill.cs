using CliveBot.Bot.Handler.Utils;
using Discord;
using Discord.Interactions;
using System.Runtime.ConstrainedExecution;

namespace CliveBot.Bot.Commands
{
    public class Skill : InteractionModuleBase
    {
        private readonly IServiceProvider services;

        public enum SkillSummon
        {
            [ChoiceDisplay("Ifrit")]
            Ifrit,
            [ChoiceDisplay("Garuda")]
            Garuda,
        }

        public Skill(IServiceProvider provider)
        {
            services = provider;
        }

        [SlashCommand("skill", "List of skills per summon")]
        public async Task HelpCommand(
            [Summary(description: "Target Summon")]
            SkillSummon? skillSummon = null,
            [Summary(description: "Search for target skill")]
            string search = ""
        )   {
            EmbedBuilder embed = new();

            EmbedFooterBuilder footer = new();
            footer.WithText("Powered by FFXVI");

            embed.Title = "Skill Flame Palm : Lvl20";
            embed.Description = "Press X and Y and you throw a flame palm";
            embed.Footer = footer;
            
            
            embed.WithThumbnailUrl("https://i.imgur.com/afqM3Vd.png");
            embed.WithImageUrl("https://i.imgur.com/Q9Ntch3.png");

            await RespondAsync(embed: embed.Build());
        }
    }
}
