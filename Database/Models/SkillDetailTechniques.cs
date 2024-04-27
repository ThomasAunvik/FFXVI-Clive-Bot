using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
	public class SkillDetailTechniques
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public SkillDetail? SkillDetail { get; set; }

		public string Title { get; set; } = "";
		public string Description { get; set; } = "";
	}
}
