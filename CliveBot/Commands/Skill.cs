using CliveBot.Bot.Handler.Utils;
using Discord;
using Discord.Interactions;
using System.Runtime.ConstrainedExecution;
using CliveBot.Database.Models;
using Microsoft.Extensions.Primitives;
using Discord.WebSocket;

namespace CliveBot.Bot.Commands
{
    public class SkillCommand : InteractionModuleBase
    {
        public static readonly string emote_skill_physical = "<:skill_physical:1070695272960774254>";
        public static readonly string emote_skill_magical = "<:skill_magical:1070699453863964754>";
        public static readonly string emote_mana = "<:mana:1070699699335598101>";
        public static readonly string emote_star_full = "<:star_full:1070699644205666364>";
        public static readonly string emote_star_half = "<:star_half:1070699604384960522>";
        public static readonly string emote_star_empty = "<:star_empty:1070699558461521930>";

        public static readonly string emote_button_square = "<:button_square:1070713630611492964>";
        public static readonly string emote_button_x = "<:button_x:1070713713864212491>";

        private readonly IServiceProvider services;

        public enum SkillSummonOption
        {
            [ChoiceDisplay("Ifrit")]
            Ifrit,
            [ChoiceDisplay("Garuda")]
            Garuda,
        }

        public SkillCommand(IServiceProvider provider)
        {
            services = provider;
        }

        public static EmbedBuilder SkillEmbedBuild(SkillModel skill)
        {
            EmbedBuilder embed = new();
            embed.Title = skill.Name;
            embed.Description = Enum.GetName(skill.Category);

            embed.AddField("Description", skill.Description);


            embed.AddField(
                "Ratings",
                $"{emote_skill_physical} {StarIconGenrator(skill.RatingPhysical)}\n" +
                $"{emote_skill_magical} {StarIconGenrator(skill.RatingMagical)}",
                inline: true
            );

            embed.AddField(
                "MASTERization",
                $"{emote_mana} {skill.MasterizationPoints}",
                inline: true
            );

            embed.WithThumbnailUrl(skill.IconUrl);
            embed.WithImageUrl(skill.PreviewImageUrl);

            return embed;
        }

        [AutocompleteCommand("name", "skill")]
        public async Task SkillAutoComplete()
        {
            SocketAutocompleteInteraction? autoComplete = Context.Interaction as SocketAutocompleteInteraction;
            if (autoComplete == null) return;
            var userInput = autoComplete.Data.Current.Value.ToString();
            if (userInput == null) return;

            IEnumerable<AutocompleteResult> results = new[]
            {
                new AutocompleteResult("foo", "foo_value"),
                new AutocompleteResult("bar", "bar_value"),
                new AutocompleteResult("baz", "baz_value"),
            }.Where(x => x.Name.StartsWith(userInput, StringComparison.InvariantCultureIgnoreCase)); // only send suggestions that starts with user's input; use case insensitive matching

            // max - 25 suggestions at a time
            await autoComplete.RespondAsync(results.Take(25));
        }

        [SlashCommand("skill", "List of skills per summon")]
        public async Task Skill(
            [Summary(description: "Target Summon")]
            SkillSummonOption? skillSummon = null,
            [Summary("name", description: "Search for target skill"), Autocomplete]
            string? skillName = ""
        )   {
            var skill = new SkillModel
            {
                Name = "Lunge",
                MasterizationPoints = 200,
                Description = $"Press {emote_button_x} and {emote_button_square} on the ground.\nAfter Stepping in at once, he attack is released.",
                RatingMagical = 1,
                RatingPhysical = 1,
                Summon = SkillSummon.Ifrit,
                IconUrl = "https://i.imgur.com/afqM3Vd.png",
                PreviewImageUrl = "https://i.imgur.com/Q9Ntch3.png",
                Category = SkillCategory.Offensive
            };

            var embed = SkillEmbedBuild(skill);

            var component = new ComponentBuilder()
                .WithButton("Maserizise", $"skill_master_{skill.Id}")
                .Build();

            await RespondAsync(embed: embed.Build(), components: component);
            
        }

        public static string StarIconGenrator(int value)
        {
            var isFullStar = value % 2 == 0;
            var fullStars = value - (isFullStar ? 0 : 1);

            var stars = "";
            for(int i = 0; i < 5; i++)
            {
                if (i < fullStars) {
                    stars += emote_star_full;
                } else if (i == value-1) {
                    stars += emote_star_half;
                } else {
                    stars += emote_star_empty;
                }
            }

            return stars;
        }
    }
    public class SkillInteractionModule : InteractionModuleBase<SocketInteractionContext<SocketMessageComponent>>
    {
        [ComponentInteraction("skill_master_*")]
        public async Task SkillAscend(string id)
        {
            int.TryParse(id, out int newId);
            var finalid = newId + 1;

            var skill = new SkillModel
            {
                Id = finalid,
                Name = $"Lunge ({finalid})",
                MasterizationPoints = 200,
                Description = $"Press {SkillCommand.emote_button_x} and {SkillCommand.emote_button_square} on the ground.\nAfter Stepping in at once, he attack is released.",
                RatingMagical = 1,
                RatingPhysical = 1,
                Summon = SkillSummon.Ifrit,
                IconUrl = "https://i.imgur.com/afqM3Vd.png",
                PreviewImageUrl = "https://i.imgur.com/Q9Ntch3.png",
                Category = SkillCategory.Offensive
            };

            var embed = SkillCommand.SkillEmbedBuild(skill);

            var component = new ComponentBuilder()
                .WithButton("Maserizise", $"skill_master_{skill.Id}")
                .Build();

            await Context.Interaction.UpdateAsync((message) =>
            {
                message.Embed = embed.Build();
                message.Components = component;
            });
        }
    }
}
