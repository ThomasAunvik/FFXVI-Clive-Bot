using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
    public enum SkillCategory
    {
        None,
        Defensive,
        Offensive,
    }

    public enum SkillSummon
    {
        None,
        Ifrit,
        Garuda,
        Shiva,
        Titan,
        Odin,
        Bahamut,
    }

    public class SkillModel
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = "Unknown Name";
        public string Description { get; set; } = "Unknown Descritpion";
        public SkillCategory Category { get; set; } = SkillCategory.None;
        public SkillSummon Summon { get; set; } = SkillSummon.None;

        public int RatingPhysical { get; set; } = 0;
        public int RatingMagical { get; set; } = 0;

        public int MasterizationPoints { get; set; } = 0;

        public string? IconUrl { get; set; }
        public string? PreviewImageUrl { get; set; }
    }
}
