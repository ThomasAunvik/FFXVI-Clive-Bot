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

namespace CliveBot.Application.Skills.Queries
{
    public class SkillDetails
    {
        public class Query : IRequest<SkillDto>
        {
            public int? SkillId { get; set; }
            public string? SkillName { get; set; }
        }

        public class Handler : BaseHandler, IRequestHandler<Query, SkillDto>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<SkillDto> Handle(Query request, CancellationToken cancellationToken)
            {
                SkillModel? skill = null;
                if(request.SkillId != null)
                {
                    skill = await _context.Skills
                        .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken) 
                        ?? throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);

                } else if(!string.IsNullOrEmpty(request.SkillName))
                {
                    skill = await _context.Skills
                        .FirstOrDefaultAsync((s) => EF.Functions.ILike(s.Name, request.SkillName + "%"), cancellationToken)
                        ?? throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                }

                if (skill == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any skill");
                }

                return skill.ConvertDto();
            }
        }
    }
}
