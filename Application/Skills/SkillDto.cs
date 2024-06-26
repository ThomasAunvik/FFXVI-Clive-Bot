﻿using CliveBot.Application.SkillLanguages;
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
        public SkillSummon Summon { get; set; } = SkillSummon.General;

        public int RatingPhysical { get; set; } = 0;
        public int RatingMagical { get; set; } = 0;

        public int CostBuy { get; set; } = 0;
        public int CostUpgrade { get; set; } = 0;
        public int CostMaster { get; set; } = 0;

        public string? IconUrl { get; set; }
        public string? PreviewImageUrl { get; set; }
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
                CostBuy = model.CostBuy,
                CostUpgrade = model.CostUpgrade,
                CostMaster = model.CostMaster,
                IconUrl = model.IconUrl,
                PreviewImageUrl = model.PreviewImageUrl,
            };
        }

        public static IEnumerable<SkillDto> ConvertDto(this IEnumerable<SkillModel> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
