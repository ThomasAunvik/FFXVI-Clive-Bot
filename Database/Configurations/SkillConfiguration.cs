using CliveBot.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Configurations
{
    internal class SkillConfiguration : IEntityTypeConfiguration<SkillModel>
    {
        public void Configure(EntityTypeBuilder<SkillModel> builder)
        {
            builder.HasOne(s => s.MasteredVersion)
                   .WithOne(s => s.PreviousVersion)
                   .HasForeignKey<SkillModel>(s => s.MasteredVersionForeignKey);
        }
    }
}
