using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        Pheonix,
        Garuda,
        Shiva,
        Titan,
        Ramuh,
        Odin,
        Bahamut,
    }

    public class SkillModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int Id { get; set; }
        public string Name { get; set; } = "Unknown Name";
        public string Description { get; set; } = "Unknown Description";
        public List<SkillLanguageModel> Localized { get; set; } = new List<SkillLanguageModel>();
        public SkillCategory Category { get; set; } = SkillCategory.None;
        public SkillSummon Summon { get; set; } = SkillSummon.None;

        public int RatingPhysical { get; set; } = 0;
        public int RatingMagical { get; set; } = 0;

        public int MasterizationPoints { get; set; } = 0;

        public string? IconUrl { get; set; }
        public string? PreviewImageUrl { get; set; }

        public int? MasteredVersionForeignKey { get; set; }
        public SkillModel? MasteredVersion { get; set; }
        public SkillModel? PreviousVersion { get; set; }
    }
}
