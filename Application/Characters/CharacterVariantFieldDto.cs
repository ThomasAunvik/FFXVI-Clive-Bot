using CliveBot.Application.SkillLanguages;
using CliveBot.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Characters
{
    public class CharacterVariantFieldDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
    }

    public static class CharacterVariantFieldDtoExtension
    {
        public static CharacterVariantFieldDto ConvertDto(this CharacterVariantField model)
        {
            return new CharacterVariantFieldDto()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
            };
        }

        public static IEnumerable<CharacterVariantFieldDto> ConvertDto(this IEnumerable<CharacterVariantField> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
