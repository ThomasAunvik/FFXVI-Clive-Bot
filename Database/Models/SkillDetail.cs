using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
	public class SkillDetail
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int SkillId { get; set; }
		public SkillModel? Skill { get; set; }

		public string? Detail { get; set; }
		public string? Mastery { get; set; }
        public ICollection<SkillDetailTechniques> BattleTechniques { get; set; } = [];
	}
}
