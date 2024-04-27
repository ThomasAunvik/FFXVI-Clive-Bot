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
		None = 0,
        Attack = 1,
        Magic = 2,
        Other = 3,
        Jump = 4,
        Evade = 5,
	}

	public enum SkillSummon
	{
		General = 0,
        Ifrit = 1,
        Pheonix = 2,
        Garuda = 3,
        Shiva = 4,
        Titan = 5,
        Ramuh = 6,
        Odin = 7,
        Bahamut = 8,
        Leviathan = 9,
        Ultima = 10,
    }

	public class SkillModel
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int Id { get; set; }
		public string Name { get; set; } = "Unknown Name";
		public string Description { get; set; } = "Unknown Description";
		public ICollection<SkillLanguageModel> Localized { get; set; } = [];
		public SkillCategory Category { get; set; } = SkillCategory.None;
		public SkillSummon Summon { get; set; } = SkillSummon.General;

		public int RatingPhysical { get; set; } = 0;
		public int RatingMagical { get; set; } = 0;

		public int CostBuy { get; set; } = 0;
		public int CostUpgrade { get; set; } = 0;
		public int CostMaster { get; set; } = 0;

		public string? IconUrl { get; set; }
		public string? PreviewImageUrl { get; set; }

		public SkillDetail Detail { get; set; } = new();
	}
}
