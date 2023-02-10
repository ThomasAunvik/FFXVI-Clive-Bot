using CliveBot.Database.Models;

namespace CliveBot.Application.Moderators
{
    public class ModeratorPermissionsDto
    {
        public int Id { get; set; }
        public bool ManageModerators { get; set; }
        public bool AllPermissions { get; set; }

        public bool ManageSkills { get; set; }
        public bool ManageSkillInfo { get; set; }
        public bool ManageSkillTranslations { get; set; }


        public bool ManageCharacters { get; set; }

        public bool ManageCharacterInfo { get; set; }
        public bool ManageCharacterNotes { get; set; }

        public ModeratorDto? Moderator { get; set; }
    }

    public static class ModeratorPermissionsDtoExtension
    {
        public static ModeratorPermissionsDto ConvertDto(this BotModeratorPermissions model)
        {
            return new ModeratorPermissionsDto()
            {
                Id = model.Id,
                ManageModerators = model.ManageModerators,
                AllPermissions = model.AllPermissions,
                ManageSkills = model.ManageSkills,
                ManageSkillInfo = model.ManageSkillInfo,
                ManageSkillTranslations = model.ManageSkillTranslations,
                ManageCharacters = model.ManageCharacters,
                ManageCharacterInfo = model.ManageCharacterInfo,
                ManageCharacterNotes = model.ManageCharacterNotes,
            };
        }

        public static IEnumerable<ModeratorPermissionsDto> ConvertDto(this IEnumerable<BotModeratorPermissions> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
