using CliveBot.Bot.Handler.Utils;
using Discord;
using Discord.Interactions;
using System.Runtime.ConstrainedExecution;
using CliveBot.Database.Models;
using Microsoft.Extensions.Primitives;
using Discord.WebSocket;
using CliveBot.Database;
using Microsoft.EntityFrameworkCore;
using CliveBot.Bot.Handler.Autocomplete;

namespace CliveBot.Bot.Commands
{
    public class SkillCommand : InteractionModuleBase
    {
        public const string emote_skill_physical = "<:skill_physical:1070695272960774254>";
        public const string emote_skill_magical = "<:skill_magical:1070699453863964754>";
        public const string emote_mana = "<:mana:1070699699335598101>";
        public const string emote_star_full = "<:star_full:1070699644205666364>";
        public const string emote_star_half = "<:star_half:1070699604384960522>";
        public const string emote_star_empty = "<:star_empty:1070699558461521930>";

        public const string emote_button_square = "<:button_square:1070713630611492964>";
        public const string emote_button_x = "<:button_x:1070713713864212491>";


        public const string emote_eikon_garuda = "<:eikon_garuda:1070847801816252476>";
        public const string emote_eikon_ifrit = "<:eikon_ifrit:1070847573465759744>";
        public const string emote_eikon_odin = "<:eikon_odin:1070847455429664880>";
        public const string emote_eikon_pheonix = "<:eikon_pheonix:1070847698560880672>";
        public const string emote_eikon_ramuh = "<:eikon_ramuh:1070847615329124422>";
        public const string emote_eikon_shiva = "<:eikon_shiva:1070847762469503056>";
        public const string emote_eikon_titan = "<:eikon_titan:1070847839837622354>";

        private readonly ApplicationDbContext db;
        private readonly IServiceProvider services;

        public SkillCommand(IServiceProvider provider, ApplicationDbContext _db)
        {
            services = provider;
            db = _db;
        }

        public static EmbedBuilder SkillEmbedBuild(SkillModel skill)
        {
            EmbedBuilder embed = new()
            {
                Title = skill.Name,
                Description = Enum.GetName(skill.Category)
            };

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

        [SlashCommand("skill", "List of skills per summon")]
        public async Task Skill(
            [Summary(name: "summon", description: "List the skills of a summon")]
            SkillSummon? skillSummon = null,
            [Summary("skill", description: "Select the target skill")]
            [Autocomplete(typeof(SkillAutocompleteHandler))]
            int? skillId = null,
            [Summary("search", description: "Search for the target skill")]
            string? skillName = null
        ) {
            SkillModel? skill = null;
            if(skillId != null)
            {
                skill = await db.Skills
                    .Include(s => s.MasteredVersion)
                    .Include(s => s.PreviousVersion)
                    .FirstOrDefaultAsync(s => s.Id == skillId);
            } else if(skillName != null)
            {
                skill = await db.SkillLanguages
                    .Where(l => l.Name.ToLower().StartsWith(skillName.ToLower()))
                    .Select(l => l.Skill)
                    .FirstOrDefaultAsync();
            } else if(skillSummon != null)
            {
                await ListSkills((SkillSummon)skillSummon);
                return;
            } else
            {
                await ListSummons(Context);
                return;
            }

            if(skill == null)
            {
                await RespondAsync(embed: new EmbedBuilder().WithTitle("Failed to find skill").Build());
                return;
            }

            var embed = SkillEmbedBuild(skill);

            MessageComponent? component = null;

            if (skill.MasteredVersion != null || skill.PreviousVersion != null)
            {
                component = new ComponentBuilder()
                    .WithButton(
                        "Masterize",
                        $"skill_view_{skill.MasteredVersion?.Id ?? 0}",
                        disabled: skill.MasteredVersion == null
                    ).WithButton(
                        "Downgrade",
                        $"skill_view_{skill.PreviousVersion?.Id ?? 0}",
                        disabled: skill.PreviousVersion == null
                    ).Build();
            }

            await RespondAsync(embed: embed.Build(), components: component);
            
        }

        public async Task ListSkills(SkillSummon skillSummon, int page = 0)
        {
            var skills = await db.Skills
                .Where(s => s.Summon == skillSummon)
                .Take(15)
                .Skip(page * 15)
                .ToListAsync();

            var skillCount = await db.Skills
                .Where(s => s.Summon == skillSummon).CountAsync();

            var embed = new EmbedBuilder()
                .WithTitle("Skills");

            string text = "";

            foreach(var skill in skills)
            {
                text += $"{skill.Name} ({skill.MasterizationPoints}) [p{skill.RatingPhysical}/10, m{skill.MasterizationPoints}/10]";
            }

            if (string.IsNullOrEmpty(text)) text = "Empty Text";

            embed.AddField(
                $"Skills of {Enum.GetName(skillSummon)}",
                text
            );

            await RespondAsync(embed: embed.Build());
        }

        public async Task ListSummons(IInteractionContext context)
        {
            var embed = new EmbedBuilder()
                .WithTitle("Skills")
                .WithDescription("A list of Eikons");

            embed.AddField("Summons",
                $"{emote_eikon_ifrit} Ifrit\n" +
                $"{emote_eikon_garuda} Garuda\n" +
                $"{emote_eikon_pheonix} Pheonix\n" +
                $"{emote_eikon_odin} Odin\n" +
                $"{emote_eikon_ramuh} Ramuh\n" +
                $"{emote_eikon_shiva} Shiva\n" +
                $"{emote_eikon_titan} Titan" 
            );

            await RespondAsync(embed: embed.Build());
        }

        public static string StarIconGenrator(int value) {
            var starValue = value / 2.0;

            var isFullStar = value % 2 == 0;
            var fullStars = starValue - (isFullStar ? 0 : 1);

            var stars = "";
            for(int i = 0; i < 5; i++)
            {
                if (i < fullStars) {
                    stars += emote_star_full;
                } else if (i + 1 > starValue && i < starValue) {
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
        private readonly ApplicationDbContext db;

        public SkillInteractionModule(ApplicationDbContext db)
        {
            this.db = db;
        }

        [ComponentInteraction("skill_view_*")]
        public async Task SkillAscend(string id)
        {
            if(!int.TryParse(id, out int skillid))
            {
                await ReplyAsync(embed: 
                    new EmbedBuilder()
                    .WithTitle("Failed to parse id")
                    .Build()
                );
                return;
            }

            var skill = await db.Skills
                    .Include(s => s.MasteredVersion)
                    .Include(s => s.PreviousVersion)
                    .FirstOrDefaultAsync(s => s.Id == skillid);

            if(skill == null)
            {
                await ReplyAsync(embed:
                    new EmbedBuilder()
                    .WithTitle("Failed to find skill")
                    .Build()
                );
                return;
            }


            var embed = SkillCommand.SkillEmbedBuild(skill);


            MessageComponent? component = null;
            if (skill.MasteredVersion != null || skill.PreviousVersion != null)
            {
                component = new ComponentBuilder()
                    .WithButton(
                        "Masterize",
                        $"skill_view_{skill.MasteredVersion?.Id ?? 0}",
                        disabled: skill.MasteredVersion == null
                    ).WithButton(
                        "Downgrade",
                        $"skill_view_{skill.PreviousVersion?.Id ?? 0}",
                        disabled: skill.PreviousVersion == null
                    ).Build();
            }

            await Context.Interaction.UpdateAsync((message) =>
            {
                message.Embed = embed.Build();
                message.Components = component;
            });
        }
    }
}
