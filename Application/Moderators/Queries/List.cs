using CliveBot.Application.Infrastructure;
using CliveBot.Application.Skills;
using CliveBot.Database;
using CliveBot.Database.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CliveBot.Application.Moderators.Queries
{
    public class ModeratorList
    {
        public class Query : IRequest<List<ModeratorDto>> { }

        public class Handler : BaseHandler, IRequestHandler<Query, List<ModeratorDto>>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

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
