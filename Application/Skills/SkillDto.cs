using CliveBot.Application.SkillLanguages;
using CliveBot.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Skills
{
    public class SkillDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Unknown Name";
        public string Description { get; set; } = "Unknown Descritpion";
        public IEnumerable<SkillLanguageDto> Localized { get; set; } = new List<SkillLanguageDto>();
        public SkillCategory Category { get; set; } = SkillCategory.None;
        public SkillSummon Summon { get; set; } = SkillSummon.None;

        public int RatingPhysical { get; set; } = 0;
        public int RatingMagical { get; set; } = 0;

        public int MasterizationPoints { get; set; } = 0;

        public string? IconUrl { get; set; }
        public string? PreviewImageUrl { get; set; }

        public SkillDto? MasteredVersion { get; set; }
        public SkillDto? PreviousVersion { get; set; }
    }

    public static class SkillDtoExtension
    {
        public static SkillDto ConvertDto(this SkillModel model)
        {
            return new SkillDto()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Localized = model.Localized.ConvertDto(),
                Category = model.Category,
                Summon = model.Summon,
                RatingPhysical = model.RatingPhysical,
                RatingMagical = model.RatingMagical,
                MasterizationPoints = model.MasterizationPoints,
                IconUrl = model.IconUrl,
                PreviewImageUrl = model.PreviewImageUrl,
                MasteredVersion = model.MasteredVersion?.ConvertDto(),
                PreviousVersion = model.PreviousVersion?.ConvertDto(),
            };
        }

        public static IEnumerable<SkillDto> ConvertDto(this IEnumerable<SkillModel> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }

        public static IQueryable<SkillDto> ConvertDto(this IQueryable<SkillModel> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
