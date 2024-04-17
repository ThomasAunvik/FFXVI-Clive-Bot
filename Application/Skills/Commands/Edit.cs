using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
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

namespace CliveBot.Application.Skills.Commands
{
    public class SkillEdit
    {
        public class Command : SkillDto, IRequest<SkillDto>
        {
            public int SkillId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Command, SkillDto>
        {
            public async Task<SkillDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var skill = await _context.Skills
                    .Include(s => s.Localized)
                    .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken) 
                    ?? throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                
                var enL = skill.Localized.FirstOrDefault(s => s.Locale == "en");
                if (enL == null)
                {
                    enL = new SkillLanguageModel
                    {
                        Locale = "en",
                        Name = request.Name,
                        Description = request.Description
                    };
                    skill.Localized.Add(enL);
                }

                skill.Name = request.Name;
                enL.Name = request.Name;

                skill.Description = request.Description;
                enL.Description = request.Description;

                skill.Category = request.Category;
                skill.Summon = request.Summon;

                skill.RatingMagical = request.RatingMagical;
                skill.RatingPhysical = request.RatingPhysical;

                skill.CostBuy = request.CostBuy;
                skill.CostUpgrade = request.CostUpgrade;
                skill.CostMaster = request.CostMaster;

                skill.IconUrl = request.IconUrl;
                skill.PreviewImageUrl = request.PreviewImageUrl;

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return skill.ConvertDto();
            }
        }
    }
}
