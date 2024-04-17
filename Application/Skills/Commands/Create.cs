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
    public class SkillCreate
    {
        public class Command : SkillDto, IRequest<SkillDto> { }

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
                var skill = new SkillModel
                {
                    Name = request.Name,
                    Description = request.Description,
                    Category = request.Category,
                    Summon = request.Summon,
                    RatingMagical = request.RatingMagical,
                    RatingPhysical = request.RatingPhysical,
                    CostBuy = request.CostBuy,
                    CostUpgrade = request.CostUpgrade,
                    CostMaster = request.CostMaster,
                    IconUrl = request.IconUrl,
                    PreviewImageUrl = request.PreviewImageUrl,
                    Localized = [
                        new SkillLanguageModel
                        {
                            Locale = "en",
                            Name = request.Name,
                            Description = request.Description
                        }
                    ]
                };

                await _context.Skills.AddAsync(skill, cancellationToken);

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
