using CliveBot.Application.Infrastructure;
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

namespace CliveBot.Application.Skills.Queries
{
    public class SkillList
    {
        public class Query : IRequest<List<SkillDto>> {
            public SkillSummon? Summon { get; set; }
        }

        public class Handler : BaseHandler, IRequestHandler<Query, List<SkillDto>>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<List<SkillDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var skillsQuery = _context.Skills.AsQueryable();

                if (request.Summon != null)
                {
                    skillsQuery = skillsQuery.Where((s) => s.Summon == request.Summon);
                }

                var skills = await skillsQuery
                    .ConvertDto()
                    .ToListAsync(cancellationToken);

                return skills;
            }
        }
    }
}
