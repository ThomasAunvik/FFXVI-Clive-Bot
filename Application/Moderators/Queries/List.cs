using CliveBot.Application.Infrastructure;
using CliveBot.Application.Skills;
using CliveBot.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CliveBot.Application.Moderators.Queries
{
    public class ModeratorList
    {
        public class Query : IRequest<List<ModeratorDto>> { }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Query, List<ModeratorDto>>
        {
            public async Task<List<ModeratorDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var moderators = await _context.BotModerators
                    .Include(m => m.Permissions)
                    .ToListAsync(cancellationToken);

                return moderators.ConvertDto().ToList();
            }
        }
    }
}
