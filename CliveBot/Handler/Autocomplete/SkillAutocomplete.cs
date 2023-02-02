using Discord.Interactions;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using CliveBot.Database;
using Microsoft.EntityFrameworkCore;

namespace CliveBot.Bot.Handler.Autocomplete
{
    public class SkillAutocompleteHandler : AutocompleteHandler
    {

        private readonly ApplicationDbContext db;
        public SkillAutocompleteHandler(ApplicationDbContext _db)
        {
            db = _db;
        }

        public override async Task<AutocompletionResult> GenerateSuggestionsAsync(IInteractionContext context, IAutocompleteInteraction autocompleteInteraction, IParameterInfo parameter, IServiceProvider services)
        {
            if (context.Interaction is not SocketAutocompleteInteraction autoComplete) return AutocompletionResult.FromSuccess();

            var userInput = autoComplete.Data.Current.Value.ToString();
            if (userInput == null) return AutocompletionResult.FromSuccess();

            var results = await db.SkillLanguages
                .Where((l) => l.Name.ToLower().StartsWith(userInput.ToLower()))
                .Take(25)
                .Select((l) => new AutocompleteResult(l.Name, l.SkillId))
                .ToListAsync();

            return AutocompletionResult.FromSuccess(results);
        }
    }
}
