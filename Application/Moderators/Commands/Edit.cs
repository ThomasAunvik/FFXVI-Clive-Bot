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
    public class ModeratorEdit
    {
        public class Command : ModeratorDto, IRequest<List<ModeratorDto>>
        {
            public int ModeratorId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Command, List<ModeratorDto>>
        {
            public async Task<List<ModeratorDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var moderator = await _context.BotModerators
                    .Include(m => m.Permissions)
                    .FirstOrDefaultAsync(s => s.Id == request.ModeratorId, cancellationToken) 
                    ?? throw new RestException(HttpStatusCode.NotFound, "Could not find any moderator with id: " + request.ModeratorId);
                
                moderator.Name = request.Name;
                moderator.ConnectionSource = request.ConnectionSource;
                moderator.ConnectionId = request.ConnectionId;

                var perm = moderator.Permissions ?? new();
                var reqPerm = request.Permissions;
                if(reqPerm != null)
                {
                    perm.ManageModerators = reqPerm.ManageModerators;
                    perm.AllPermissions = reqPerm.AllPermissions;
                    perm.ManageSkills = reqPerm.ManageSkills;
                    perm.ManageSkillInfo = reqPerm.ManageSkillInfo;
                    perm.ManageSkillTranslations = reqPerm.ManageSkillTranslations;
                    perm.ManageCharacters = reqPerm.ManageCharacters;
                    perm.ManageCharacterInfo = reqPerm.ManageCharacterInfo;
                    perm.ManageCharacterNotes = reqPerm.ManageCharacterNotes;
                }

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                var moderators = await _context.BotModerators
                    .Include(m => m.Permissions)
                    .ToListAsync(cancellationToken);

                return moderators.ConvertDto().ToList();
            }
        }
    }
}
