using CliveBot.Database.Models;

namespace CliveBot.Application.SkillLanguages
{
    public class SkillLanguageDto
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public string Locale { get; set; } = "en";
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public static class SkillLanguageDtoExtension
    {
        public static SkillLanguageDto ConvertDto(this SkillLanguageModel model)
        {
            return new SkillLanguageDto()
            {
                Id = model.Id,
                SkillId = model.SkillId,
                Locale = model.Locale,
                Name = model.Name,
                Description = model.Description,
            };
        }

        public static IEnumerable<SkillLanguageDto> ConvertDto(this IEnumerable<SkillLanguageModel> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }

        public static IQueryable<SkillLanguageDto> ConvertDto(this IQueryable<SkillLanguageModel> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
