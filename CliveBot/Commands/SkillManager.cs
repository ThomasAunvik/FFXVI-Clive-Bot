using CliveBot.Bot.Attributes.Preconditions;
using CliveBot.Bot.Handler.Autocomplete;
using CliveBot.Database;
using CliveBot.Database.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
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
            [Summary("IconUrl", "A small 128x128 image of the skill (https://i.imgur.com/afqM3Vd.png)")]
            string? iconUrl,
            [Summary("PreviewImage", "A large image or gif that shows the skill's usage (https://i.imgur.com/Q9Ntch3.png)")]
            string? previewImageUrl,
            [Summary("MasteryPoints", "The number to upgrade the skill")]
            int? masteryPoints,
            [Summary("Summon", "What category of summon is the skill set on")]
            SkillSummon skillSummon,
            [Summary("Category", "What type of ability does this skill do")]
            SkillCategory category,
            [Summary("PhysicalRating", "What is the rating of it's physical ability")]
            Rating physicalRating,
            [Summary("MagicalRating", "What is the rating of it's magical ability")]
            Rating magicalRating
        ) {
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

            if(result == 0)
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
            int skillId,
            [Summary("Name", "The name of the skill, for example: 'Flame Punch'")]
            string? name,
            [Summary("Description", $"Something that describes the skill, shown ingame.")]
            string? description,
            [Summary("IconUrl", "A small 128x128 image of the skill (https://i.imgur.com/afqM3Vd.png)")]
            string? iconUrl,
            [Summary("PreviewImage", "A large image or gif that shows the skill's usage (https://i.imgur.com/Q9Ntch3.png)")]
            string? previewImageUrl,
            [Summary("MasteryPoints", "The number to upgrade the skill")]
            int? masteryPoints,
            [Summary("Summon", "What category of summon is the skill set on")]
            SkillSummon? skillSummon,
            [Summary("Category", "What type of ability does this skill do")]
            SkillCategory? category,
            [Summary("PhysicalRating", "What is the rating of it's physical ability")]
            Rating? physicalRating,
            [Summary("MagicalRating", "What is the rating of it's magical ability")]
            Rating? magicalRating
        )
        {
            description = description?.Replace("\\n", "\n");

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

            if(description != null)
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
    }
}
