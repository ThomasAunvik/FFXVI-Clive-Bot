using CliveBot.Database.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Configurations
{
	internal class SkillDetailConfiguration : IEntityTypeConfiguration<SkillDetail>
	{
		public void Configure(EntityTypeBuilder<SkillDetail> builder)
		{
			builder.HasOne(s => s.Skill)
				   .WithOne(s => s.Detail)
				   .OnDelete(DeleteBehavior.NoAction)
				   .HasForeignKey<SkillDetail>(s => s.SkillId);
		}
	}
}
