using CliveBot.Application.SkillLanguages;
using CliveBot.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Characters
{
    public class CharacterVariantDto
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public required string Description { get; set; }
        public bool DefaultVariant { get; set; } = false;

        public int? Age { get; set; }

        public IEnumerable<CharacterVariantFieldDto>? AdditionalFields { get; set; }

        public string? PreviewImageUrl { get; set; }
    }

    public static class CharacterVariantDtoExtension
    {
        public static CharacterVariantDto ConvertDto(this CharacterVariant model)
        {
            return new CharacterVariantDto()
            {
                Id = model.Id,
                CharacterId = model.CharacterId,
                Description = model.Description,
                DefaultVariant = model.DefaultVariant,
                Age = model.Age,
                AdditionalFields = model.AdditionalFields.ConvertDto(),
                PreviewImageUrl = model.PreviewImageUrl,
            };
        }

        public static IEnumerable<CharacterVariantDto> ConvertDto(this IEnumerable<CharacterVariant> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
