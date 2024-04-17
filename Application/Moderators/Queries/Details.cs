using CliveBot.Application.Infrastructure;
using CliveBot.Database.Models;
using CliveBot.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliveBot.Application.Errors;
using System.Net;

namespace CliveBot.Application.Moderators.Queries
{
    public class ModeratorDetails
    {
        public class Query : IRequest<ModeratorDto>
        {
            public int ModeratorId { get; set; }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Query, ModeratorDto>
        {
            public async Task<ModeratorDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var moderator = await _context.BotModerators
                    .Include(s => s.Permissions)
                    .FirstOrDefaultAsync(s => s.Id == request.ModeratorId, cancellationToken);

                if(moderator == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any moderator with id: " + request.ModeratorId);
                }

                return moderator.ConvertDto();
            }
        }
    }
}
