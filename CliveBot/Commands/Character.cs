using CliveBot.Database;
using CliveBot.Database.Models;
using Discord;
using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Bot.Commands
{
    [Group("character", "Characters")]
    public class CharacterCommand : InteractionModuleBase
    {
        private readonly ApplicationDbContext db;
        private readonly IServiceProvider services;

        public CharacterCommand(IServiceProvider provider, ApplicationDbContext _db)
        {
            services = provider;
            db = _db;
        }

        public static EmbedBuilder CharacterEmbedBuild(Character character, string? locale = null)
        {
            var name = character.Name;
            var variant = character.Variants.FirstOrDefault(v => v.DefaultVariant);
            var desc = variant?.Description ?? "No Description";

            EmbedBuilder embed = new()
            {
                Title = name,
                Description = "",
            };

            embed.AddField("Description", desc);

            if (variant != null)
            {
                embed.WithThumbnailUrl(Config.UrlCdnConvert(variant.PreviewImageUrl));
            }

            return embed;
        }

        [SlashCommand("list", "List of characters")]
        public async Task CharacterList(
            string? filter = null,
            int page = 1
        )
        {
            if(page <= 0)
            {
                await RespondAsync("Invalid Command, Page count under 1");
                return;
            }

            await Context.Interaction.DeferAsync();

            var query = db.Characters.AsQueryable();

            if(filter != null)
            {
                query = query.Where(c => EF.Functions.ILike(c.Name, filter + "%"));
            }

            var characters = await query
                .OrderBy(o => o.Name)
                .Skip((page-1) * 10)
                .Take(10)
                .ToListAsync();

            var filterCount = await query.CountAsync();

            if(characters == null)
            {
                await Context.Interaction.ModifyOriginalResponseAsync(r => {
                    r.Embed = new EmbedBuilder().WithTitle("Failed to list characters").Build();
                });
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle("Characters")
                .WithFooter($"Page {page} of {Math.Ceiling(filterCount / 10.0)}. Total: {filterCount}");

            if(filter != null)
            {
                embed.WithDescription("With filter: " + filter);
            }

            StringBuilder sb = new();
            characters.ForEach(c => sb.AppendLine(c.Name));
            var stringValue = sb.ToString();

            embed.AddField("List of Characters", string.IsNullOrWhiteSpace(stringValue) ? "No Characters Listed" : stringValue);

            await Context.Interaction.ModifyOriginalResponseAsync((m) =>
            {
                m.Embed = embed.Build();
            });
        }

        [SlashCommand("search", "Details of characters")]
        public async Task Character(
            string query
        ) {
            await Context.Interaction.DeferAsync();

            var character = await db.Characters
                .Where(c => EF.Functions.ILike(c.Name, query + "%"))
                .Include(c => c.Variants.Where(v => v.DefaultVariant))
                .FirstOrDefaultAsync();

            if(character == null)
            {
                await Context.Interaction.ModifyOriginalResponseAsync((m) =>
                {
                    m.Embed = new EmbedBuilder().WithTitle("Character Not Found...").Build();
                });

                return;
            }

            var embed = CharacterEmbedBuild(character);

            await Context.Interaction.ModifyOriginalResponseAsync((m) =>
            {
                m.Embed = embed.Build();
            });
        }
    }
}
