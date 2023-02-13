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

        public static EmbedBuilder SkillEmbedBuild(SkillModel skill, string? locale = null)
        {
            var name = skill.Name;
            var desc = skill.Description;

            if(locale != null)
            {
                var localized = skill.Localized.FirstOrDefault(e => e.Locale == locale);
                if(localized != null)
                {
                    name = localized.Name;
                    desc = localized.Description;
                }
            }

            EmbedBuilder embed = new()
            {
                Title = name,
                Description = Enum.GetName(skill.Category)
            };

            embed.AddField("Description", desc);


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

            embed.WithThumbnailUrl(Config.UrlCdnConvert(skill.IconUrl));

            return embed;
        }

        [SlashCommand("skill", "List of skills per summon")]
        public async Task Skill(
            [Summary(name: "summon", description: "List the skills of a summon")]
            SkillSummon? skillSummon = null,
            [Summary("skill", description: "Select the target skill")]
            [Autocomplete(typeof(SkillAutocompleteHandler))]
            string? skillIdLang = null,
            [Summary("search", description: "Search for the target skill")]
            string? skillName = null,
            [Summary("Locale", "Select a Locale to edit on")]
            LocaleOptions? locale = null
        ) {
            SkillModel? skill = null;
            string? selectedLocale = null;
            if(skillIdLang != null)
            {
                var skillSplit = skillIdLang.Split(",");
                if (int.TryParse(skillSplit[0], out int skillId))
                {
                    skill = await db.Skills
                        .Include(s => s.MasteredVersion)
                        .Include(s => s.PreviousVersion)
                        .Include(s => s.Localized)
                        .FirstOrDefaultAsync(s => s.Id == skillId);

                    selectedLocale = skillSplit[1];
                }
            } else if(skillName != null)
            {
                var skillLang = await db.SkillLanguages
                    .Where((l) => EF.Functions.ILike(l.Name, skillName + "%"))
                    .OrderBy(l => l.Name)
                    .FirstOrDefaultAsync();

                if (skillLang?.Skill != null)
                {
                    skill = skillLang.Skill;
                    if (locale == null) selectedLocale = skillLang.Locale;
                }

                var id = skillLang?.SkillId;

                if (id != null) {
                    skill = await db.Skills
                       .Include(s => s.MasteredVersion)
                       .Include(s => s.PreviousVersion)
                       .Include(s => s.Localized)
                       .FirstOrDefaultAsync(s => s.Id == id);
                }
            } else if(skillSummon != null)
            {
                await ListSkills((SkillSummon)skillSummon);
                return;
            } else
            {
                await ListSummons(Context);
                return;
            }

            if(locale != null)
            {
                selectedLocale = Enum.GetName(locale ?? LocaleOptions.en);
            }

            if(selectedLocale == null)
            {
                selectedLocale = "en";
            }

            if(skill == null)
            {
                await RespondAsync(embed: new EmbedBuilder().WithTitle("Failed to find skill").Build());
                return;
            }

            var embed = SkillEmbedBuild(skill, locale: selectedLocale);

            var compBuilder = new ComponentBuilder()
                .WithButton("Preview", $"skillimgpreview:{skill.Id}", ButtonStyle.Success);

            if (skill.MasteredVersion != null || skill.PreviousVersion != null)
            {
                compBuilder
                    .WithButton(
                        "Masterize",
                        $"skillview:{skill.MasteredVersion?.Id ?? 0},{locale}",
                        ButtonStyle.Primary,
                        emote: Emote.Parse(emote_mana),
                        disabled: skill.MasteredVersion == null
                    ).WithButton(
                        "Downgrade",
                        $"skillview:{skill.PreviousVersion?.Id ?? 0},{locale}",
                        ButtonStyle.Secondary,
                        disabled: skill.PreviousVersion == null
                    );
            }

            await RespondAsync(
                embed: embed.Build(),
                components: compBuilder.Build()
            );
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

        [ComponentInteraction("skillview:*")]
        public async Task SkillAscend(string idlang)
        {
            var skillSplit = idlang.Split(",");

            if (!int.TryParse(skillSplit[0], out int skillid))
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

            var locale = skillSplit[1];

            var embed = SkillCommand.SkillEmbedBuild(skill, locale: locale);

            var compBuilder = new ComponentBuilder()
                .WithButton("Preview", $"skillimgpreview:{skill.Id}", ButtonStyle.Success);

            if (skill.MasteredVersion != null || skill.PreviousVersion != null)
            {
                compBuilder
                    .WithButton(
                        "Masterize",
                        $"skillview:{skill.MasteredVersion?.Id ?? 0},{locale}",
                        ButtonStyle.Primary,
                        emote: Emote.Parse(SkillCommand.emote_mana),
                        disabled: skill.MasteredVersion == null
                    ).WithButton(
                        "Downgrade",
                        $"skillview:{skill.PreviousVersion?.Id ?? 0},{locale}",
                        ButtonStyle.Secondary,
                        disabled: skill.PreviousVersion == null
                    );
            }

            await Context.Interaction.UpdateAsync((message) =>
            {
                message.Embed = embed.Build();
                message.Components = compBuilder.Build();
            });
        }

        [ComponentInteraction("skillimgpreview:*")]
        public async Task SkillImagePreview(string idlang)
        {
            var skillSplit = idlang.Split(",");

            if (!int.TryParse(skillSplit[0], out int skillid))
            {
                await Context.Interaction.RespondAsync(embed:
                    new EmbedBuilder()
                    .WithTitle("Failed to parse id")
                    .Build(),
                    ephemeral: true
                );
                return;
            }

            var skill = await db.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillid);

            if (skill == null)
            {
                await Context.Interaction.RespondAsync(embed:
                    new EmbedBuilder()
                        .WithTitle("Failed to find skill")
                        .Build(),
                        ephemeral: true
                );
                return;
            }

            var previewImageUrl = Config.UrlCdnConvert(skill.PreviewImageUrl);

            await Context.Interaction.RespondAsync(previewImageUrl, ephemeral: true);
        }
    }
}
