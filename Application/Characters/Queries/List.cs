using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using CliveBot.Database.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CliveBot.Application.Characters.Queries
{
    public class CharacterList
    {
        public class Query : IRequest<List<CharacterDto>> { }

        public class Handler : BaseHandler, IRequestHandler<Query, List<CharacterDto>>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<List<CharacterDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var charactersQuery = _context.Characters.AsQueryable();

                var characters = await charactersQuery
                    .Include(c => c.Variants.Where(v => v.DefaultVariant))
                    .ToListAsync(cancellationToken);

                return characters.ConvertDto().ToList();
            }
        }
    }
}
