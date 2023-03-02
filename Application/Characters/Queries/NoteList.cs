using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using CliveBot.Database.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CliveBot.Application.Characters.Queries
{
    public class CharacterNoteList
    {
        public class Query : IRequest<List<CharacterNoteDto>>
        {
            public int CharacterId { get; set; }

            public int Page { get; set; } = 1;
            public int Take { get; set; } = 20;
        }

        public class Handler : BaseHandler, IRequestHandler<Query, List<CharacterNoteDto>>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<List<CharacterNoteDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var charactersQuery = _context.CharacterNotes.AsQueryable();

                var variants = await charactersQuery
                    .Where(v => v.CharacterId == request.CharacterId)
                    .OrderBy(v => v.NoteName)
                    .Skip(request.Page * (request.Page - 1))
                    .Take(request.Take)
                    .ToListAsync(cancellationToken);

                return variants.ConvertDto().ToList();
            }
        }
    }
}
