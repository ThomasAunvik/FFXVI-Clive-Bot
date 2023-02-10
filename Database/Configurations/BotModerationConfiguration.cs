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
    internal class BotModerationConfiguration : IEntityTypeConfiguration<BotModerator>
    {
        public void Configure(EntityTypeBuilder<BotModerator> builder)
        {
            builder.HasOne(s => s.Permissions)
                   .WithOne(s => s.Moderator)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasForeignKey<BotModerator>(s => s.PermissionsId);
        }
    }
}
