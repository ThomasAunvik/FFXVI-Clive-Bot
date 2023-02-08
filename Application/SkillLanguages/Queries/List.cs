using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Application.SkillLanguages;
using CliveBot.Database;
using CliveBot.Database.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CliveBot.Application.Skills.Queries
{
    public class SkillLanguageList
    {
        public class Query : IRequest<List<SkillLanguageDto>> {
            public required int SkillId { get; set; }
        }

        public class Handler : BaseHandler, IRequestHandler<Query, List<SkillLanguageDto>>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<List<SkillLanguageDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var skill = await _context.Skills
                    .Include(s => s.Localized)
                    .ConvertDto()
                    .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken);

                if (skill == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                }

                return skill.Localized.ToList();
            }
        }
    }
}
