using CliveBot.Bot.Attributes.Preconditions;
using CliveBot.Bot.Handler.Autocomplete;
using CliveBot.Bot.Handler.Modals;
using CliveBot.Bot.Handler.Utils;
using CliveBot.Database;
using CliveBot.Database.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace CliveBot.Bot.Commands
{
    [BotAdminPermission]
    public class SkillManager : InteractionModuleBase
    {
        private readonly ApplicationDbContext db;
        public SkillManager(IServiceProvider provider, ApplicationDbContext _db)
        {
            db = _db;
        }

        public enum Rating
        {
            [ChoiceDisplay("0 Star")]
            NoStar = 0,
            [ChoiceDisplay("0.5 Star")]
            HalfStar = 1,

            [ChoiceDisplay("1 Star")]
            OneStar = 2,
            [ChoiceDisplay("1.5 Star")]
            OneHalfStar = 3,

            [ChoiceDisplay("2 Star")]
            TwoStar = 4,
            [ChoiceDisplay("2.5 Star")]
            TwoHalfStar = 5,

            [ChoiceDisplay("3 Star")]
            ThreeStar = 6,
            [ChoiceDisplay("3.5 Star")]
            ThreeHalfStar = 7,

            [ChoiceDisplay("4 Star")]
            FourStar = 8,
            [ChoiceDisplay("4.5 Star")]
            FourHalfStar = 9,

            [ChoiceDisplay("5 Star")]
            FiveStar = 10,
        };

        [SlashCommand("skilladd", "Add a new skill")]
        public async Task SkillAdd(
            [Summary("Name", "The name of the skill, for example: 'Flame Punch'")]
            string name,
            [Summary("Description", $"Something that describes the skill, shown ingame.")]
            string description,

            [Summary("Summon", "What category of summon is the skill set on")]
            SkillSummon skillSummon,
            [Summary("Category", "What type of ability does this skill do")]
            SkillCategory category,
            [Summary("PhysicalRating", "What is the rating of it's physical ability")]
            Rating physicalRating,
            [Summary("MagicalRating", "What is the rating of it's magical ability")]
            Rating magicalRating,

            [Summary("IconUrl", "A small 128x128 image of the skill (https://i.imgur.com/afqM3Vd.png)")]
            string? iconUrl = null,
            [Summary("PreviewImage", "A large image or gif that shows the skill's usage (https://i.imgur.com/Q9Ntch3.png)")]
            string? previewImageUrl = null,
            [Summary("MasteryPoints", "The number to upgrade the skill")]
            int? masteryPoints = null
        )
        {
            description = description.Replace("\\n", "\n");

            await RespondAsync(embed: new EmbedBuilder().WithTitle("Adding new skill...").Build());

            var newSkill = new SkillModel()
            {
                Name = name,
                Description = description,
                IconUrl = iconUrl,
                PreviewImageUrl = previewImageUrl,
                Summon = skillSummon,
                Category = category,
                RatingPhysical = (int)physicalRating,
                RatingMagical = (int)magicalRating,
                MasterizationPoints = masteryPoints ?? 0,
                Localized = new List<SkillLanguage>() {
                    new SkillLanguage() {
                        Description=description,
                        Name=name
                    }
                }
            };

            await db.AddAsync(newSkill);
            int result = await db.SaveChangesAsync();

            if (result == 0)
            {
                await ModifyOriginalResponseAsync((message) =>
                {
                    message.Embed = new EmbedBuilder().WithTitle("Failed to add new skill.").Build();
                });
                return;
            }

            await ModifyOriginalResponseAsync((message) =>
            {
                message.Embed = SkillCommand.SkillEmbedBuild(newSkill).Build();
            });
        }

        [SlashCommand("skilledit", "Edit a skill")]
        public async Task SkillEdit(
            [Summary("Skill")]
            [Autocomplete(typeof(SkillAutocompleteHandler))]
            string skillIdLang,
            [Summary("Name", "The name of the skill, for example: 'Flame Punch'")]
            string? name = null,
            [Summary("Description", $"Something that describes the skill, shown ingame.")]
            string? description = null,
            [Summary("IconUrl", "A small 128x128 image of the skill (https://i.imgur.com/afqM3Vd.png)")]
            string? iconUrl = null,
            [Summary("PreviewImage", "A large image or gif that shows the skill's usage (https://i.imgur.com/Q9Ntch3.png)")]
            string? previewImageUrl = null,
            [Summary("MasteryPoints", "The number to upgrade the skill")]
            int? masteryPoints = null,
            [Summary("Summon", "What category of summon is the skill set on")]
            SkillSummon? skillSummon = null,
            [Summary("Category", "What type of ability does this skill do")]
            SkillCategory? category = null,
            [Summary("PhysicalRating", "What is the rating of it's physical ability")]
            Rating? physicalRating = null,
            [Summary("MagicalRating", "What is the rating of it's magical ability")]
            Rating? magicalRating = null
        )
        {
            description = description?.Replace("\\n", "\n");

            var skillSplit = skillIdLang.Split(",");
            var validSkillid = int.TryParse(skillSplit[0], out int skillId);
            if(!validSkillid)
            {
                await RespondAsync(embed: new EmbedBuilder().WithTitle("Failed to find skill.").Build());
                return;
            }

            await RespondAsync(embed: new EmbedBuilder().WithTitle("Updating skill...").Build());

            var skill = await db.Skills
                .Include(s => s.Localized)
                .FirstOrDefaultAsync((s) => s.Id == skillId);
            if (skill == null)
            {
                await ModifyOriginalResponseAsync((message) =>
                {
                    message.Embed = new EmbedBuilder().WithTitle("Failed to find skill.").Build();
                });
                return;
            }

            var locale = skill.Localized.FirstOrDefault(s => s.Locale == "en");

            if (name != null)
            {
                skill.Name = name;
                if (locale != null)
                {
                    locale.Name = name;
                }
            }

            if (description != null)
            {
                skill.Description = description;
                if (locale != null)
                {
                    locale.Description = description;
                }
            }

            if (iconUrl != null) skill.IconUrl = iconUrl;
            if (previewImageUrl != null) skill.PreviewImageUrl = previewImageUrl;
            if (masteryPoints != null) skill.MasterizationPoints = masteryPoints ?? 0;
            if (skillSummon != null) skill.Summon = skillSummon ?? SkillSummon.None;
            if (category != null) skill.Category = category ?? SkillCategory.None;
            if (physicalRating != null) skill.RatingPhysical = (int)physicalRating;
            if (magicalRating != null) skill.RatingMagical = (int)magicalRating;


            int result = await db.SaveChangesAsync();

            if (result == 0)
            {
                await ModifyOriginalResponseAsync((message) =>
                {
                    message.Embed = new EmbedBuilder().WithTitle("Failed to add new skill.").Build();
                });
                return;
            }

            await ModifyOriginalResponseAsync((message) =>
            {
                message.Embed = SkillCommand.SkillEmbedBuild(skill).Build();
            });
        }

       

        [SlashCommand("skilllanguage", "Edit or Add a skill's Locale")]
        public async Task SkillEdit(
            [Summary("Skill")]
            [Autocomplete(typeof(SkillAutocompleteHandler))]
            string skillIdLang,
            [Summary("Locale", "Select a Locale to edit on")]
            LocaleOptions locale
        )
        {
            var skillIdSplit = skillIdLang.Split(",");
            var validSkillId = int.TryParse(skillIdSplit[0], out int skillId);
            if (!validSkillId)
            {
                await ReplyAsync(embed: new EmbedBuilder().WithTitle("Failed to parse skill id").Build());
                return;
            }

            var skill = await db.Skills
                .Include(s => s.Localized)
                .FirstOrDefaultAsync(s => s.Id == skillId);

            if (skill == null)
            {
                await ReplyAsync(embed: new EmbedBuilder().WithTitle("Failed to find skill").Build());
                return;
            }

            var localeName = Enum.GetName(locale);
            var skillLanguage = skill.Localized.FirstOrDefault(l => l.Locale == localeName);

            var skillTitle = skillLanguage == null ? $"Create Language for {skill.Name}" : $"Update Language for {skill.Name}";
            
            var skillModal = new ModalSkillLanguage() {
                Name = skillLanguage?.Name ?? string.Empty,
                Description = skillLanguage?.Description ?? string.Empty,
            };

            await Context.Interaction.RespondWithModalAsync<ModalSkillLanguage>("skilllanguagemodal", modifyModal: (modal) => {
                modal.Title = skillTitle;
            });

            return;
            var modal = new ModalBuilder()
                .WithTitle(skillLanguage == null ? $"Create Language for {skill.Name}" : $"Update Language for {skill.Name}")
                .WithCustomId($"skilllanguagemodal")
                .AddTextInput(
                    "Name",
                    "skill_language_name",
                    placeholder: "Jump",
                    minLength: 3,
                    maxLength: 25,
                    required: true,
                    value: skillLanguage?.Name
                ).AddTextInput(
                    "Description",
                    "skill_language_description",
                    style: TextInputStyle.Paragraph,
                    placeholder: $"Hold {SkillCommand.emote_button_x} Button to Jump",
                    minLength: 3,
                    maxLength: 200,
                    required: false,
                    value: skillLanguage?.Description
                );

            await Context.Interaction.RespondWithModalAsync(modal: modal.Build());
        }

        [ModalInteraction("skilllanguagemodal")]
        public async Task SkillLangModalTest(ModalSkillLanguage modal)
        {
            Log.Logger.Information("Modal Works: ");
            await Context.Interaction.DeferAsync();
        }

        public async static Task SkillLanguageModalEdit(SocketModal modal, ApplicationDbContext db)
        {
            return;
            var modalId = modal.Data.CustomId.Split(":").Last();
            var ids = modalId.Split(",");

            bool result = int.TryParse(ids.FirstOrDefault(), out int id);
            if(!result)
            {
                await modal.RespondAsync(
                    embed: new EmbedBuilder().WithTitle("Failed to parse skill id").Build(),
                    ephemeral: true
                );
            }

            var locale = ids.LastOrDefault();

            var name = modal.Data.Components.FirstOrDefault(c => c.CustomId == "skillLanguageName")?.Value ?? "Unknown Name";
            var desc = modal.Data.Components.FirstOrDefault(c => c.CustomId == "skillLanguageDescription")?.Value ?? "No Description";

            if (locale == null)
            {
                await modal.RespondAsync(
                    embed: new EmbedBuilder().WithTitle("Failed to retrieve data").Build(),
                    ephemeral: true
                );
                return;
            }

            var skill = await db.Skills
                .FirstOrDefaultAsync(s => s.Id == id);

            if (skill == null)
            {
                await modal.RespondAsync(
                    embed: new EmbedBuilder().WithTitle("Failed to retrieve skill").Build(),
                    ephemeral: true
                );
                return;
            }

            if(locale == "en")
            {
                skill.Name = name;
                skill.Description = desc;
            }

            var lang = await db.SkillLanguages
                .FirstOrDefaultAsync(s => s.SkillId == id && s.Locale == locale);
            if(lang == null)
            {
                await db.SkillLanguages.AddAsync(new SkillLanguage()
                {
                    Name = name,
                    Description = desc,
                    Locale = locale,
                    SkillId = skill.Id, 
                });
            } else {
                lang.Name = name;
                lang.Description = desc;
            }

            var changes = await db.SaveChangesAsync();
            if (changes == 0) {

                await modal.RespondAsync(
                    embed: new EmbedBuilder().WithTitle("Failed to change Skill Language").Build(),
                    ephemeral: true
                );
                return;
            }
            await modal.RespondAsync(
                    embed: new EmbedBuilder().WithTitle("Successfully changed Skill Language").Build(),
                    ephemeral: true
                );
            return;
        }
    }

}
