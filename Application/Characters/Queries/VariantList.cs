using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using CliveBot.Database.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CliveBot.Application.Characters.Queries
{
    public class CharacterVariantList
    {
        public class Query : IRequest<List<CharacterVariantDto>>
        {
            public int CharacterId {get; set;}
        }

        public class Handler : BaseHandler, IRequestHandler<Query, List<CharacterVariantDto>>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<List<CharacterVariantDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var charactersQuery = _context.CharacterVariants.AsQueryable();

                var variants = await charactersQuery
                    .Where(v => v.CharacterId == request.CharacterId)
                    .ToListAsync(cancellationToken);

                return variants.ConvertDto().ToList();
            }
        }
    }
}
