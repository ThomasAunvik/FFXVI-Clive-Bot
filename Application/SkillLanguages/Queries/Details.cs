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
using CliveBot.Application.SkillLanguages;

namespace CliveBot.Application.Skills.Queries
{
    public class SkillLanguageDetails
    {
        public class Query : IRequest<SkillLanguageDto>
        {
            public required int SkillId { get; set; }
            public required string Locale { get; set; }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Query, SkillLanguageDto>
        {
            public async Task<SkillLanguageDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var skill = await _context.Skills
                    .Include(s => s.Localized.Where(l => l.Locale == request.Locale))
                    .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken);

                if(skill == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                }

                var lang = skill.Localized.FirstOrDefault();
                if (lang == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, $"Could not find any language ({request.Locale}) skill with id: {request.SkillId}");
                }

                return lang.ConvertDto();
            }
        }
    }
}
