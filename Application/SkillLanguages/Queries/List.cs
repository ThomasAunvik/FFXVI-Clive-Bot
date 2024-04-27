using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Application.SkillLanguages;
using CliveBot.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CliveBot.Application.Skills.Queries
{
    public class SkillLanguageList
    {
        public class Query : IRequest<List<SkillLanguageDto>> {
            public required int SkillId { get; set; }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Query, List<SkillLanguageDto>>
        {
            public async Task<List<SkillLanguageDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var skill = await _context.Skills
                    .Include(s => s.Localized)
                    .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken);

                if (skill == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                }

                return skill.Localized
                    .OrderBy(s => s.Locale)
                    .ConvertDto().ToList();
            }
        }
    }
}
