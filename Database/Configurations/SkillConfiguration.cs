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
	internal class SkillConfiguration : IEntityTypeConfiguration<SkillModel>
	{
		public void Configure(EntityTypeBuilder<SkillModel> builder)
		{
			builder.HasOne(s => s.Detail)
				   .WithOne(s => s.Skill)
				   .OnDelete(DeleteBehavior.Cascade)
				   .HasForeignKey<SkillDetail>(s => s.SkillId)
				   .IsRequired();
		}
	}
}
