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
    internal class CharacterConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.HasMany(s => s.Variants)
                   .WithOne(s => s.Character)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey(s => s.CharacterId);
        }
    }
}
