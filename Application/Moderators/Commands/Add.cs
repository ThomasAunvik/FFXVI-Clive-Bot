using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Application.Moderators.Queries;
using CliveBot.Database;
using CliveBot.Database.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CliveBot.Application.Moderators.Commands
{
    public class ModeratorAdd
    {
        public class Command : ModeratorDto, IRequest<List<ModeratorDto>> { }

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
                var moderator = new BotModerator()
                {
                    Name = request.Name,
                    ConnectionSource = request.ConnectionSource,
                    ConnectionId = request.ConnectionId,
                    Permissions = new BotModeratorPermissions()
                    {
                        ManageModerators = request.Permissions?.ManageModerators ?? false,
                        AllPermissions = request.Permissions?.AllPermissions ?? false,
                        ManageSkills = request.Permissions?.ManageSkills ?? false,
                        ManageSkillInfo = request.Permissions?.ManageSkillInfo ?? false,
                        ManageSkillTranslations = request.Permissions?.ManageSkillTranslations ?? false,
                        ManageCharacters = request.Permissions?.ManageCharacters ?? false,
                        ManageCharacterInfo = request.Permissions?.ManageCharacterInfo ?? false,
                        ManageCharacterNotes = request.Permissions?.ManageCharacterNotes ?? false,
                    }
                };

                await _context.AddAsync(moderator, cancellationToken);

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
