using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Application.Moderators.Queries;
using CliveBot.Database;
using CliveBot.Database.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CliveBot.Application.Moderators.Commands
{
    public class ModeratorRemove
    {
        public class Command : IRequest<List<ModeratorDto>>
        {
            public int ModeratorId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, List<ModeratorDto>>
        {
            private readonly IMediator _mediator;
            public Handler(ApplicationDbContext context, IConfiguration config, IMediator mediator) : base(context, config)
            {
                _mediator = mediator;
            }

            public async Task<List<ModeratorDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var moderator = await _context.BotModerators
                    .FirstOrDefaultAsync(s => s.Id == request.ModeratorId, cancellationToken);

                if (moderator == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any moderator with id: " + request.ModeratorId);
                }

                _context.BotModerators.Remove(moderator);

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return await _mediator.Send(new ModeratorList.Query(), cancellationToken);
            }
        }
    }
}
