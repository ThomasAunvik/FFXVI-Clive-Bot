using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
    public class BotModeratorPermissions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public bool ManageModerators { get; set; }
        public bool CanManageModerators { get { return ManageModerators; } }

        public bool AllPermissions { get; set; }

        public bool ManageSkills { get; set; }
        public bool CanManageSkills { get { return ManageSkills || AllPermissions; } }
        public bool ManageSkillInfo { get; set; }
        public bool CanManageSkillInfo { get { return ManageSkillInfo || AllPermissions; } }
        public bool ManageSkillTranslations { get; set; }
        public bool CanManageSkillTranslations { get { return ManageSkillTranslations || AllPermissions; } }


        public bool ManageCharacters { get; set; }
        public bool CanManageCharacters { get { return ManageCharacters || AllPermissions; } }

        public bool ManageCharacterInfo { get; set; }
        public bool CanManageCharacterInfo { get { return ManageCharacterInfo || AllPermissions; } }
        public bool ManageCharacterNotes { get; set; }
        public bool CanManageCharacterNotes { get { return ManageCharacterNotes || AllPermissions; } }

        public BotModerator? Moderator { get; set; }
    }
}
