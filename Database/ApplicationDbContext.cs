﻿using CliveBot.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CliveBot.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var env = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_URL");
            optionsBuilder.UseNpgsql(env);
        }

        public DbSet<SkillModel> Skills { get; set; }
        public DbSet<SkillLanguageModel> SkillLanguages { get; set; }
        public DbSet<BotModerator> BotModerators { get; set; }
        public DbSet<BotModeratorPermissions> BotModeratorPermissions { get; set; }

        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterNote> CharacterNotes { get; set; }
        public DbSet<CharacterVariant> CharacterVariants { get; set; }
        public DbSet<CharacterVariantField> CharacterVariantFields { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}