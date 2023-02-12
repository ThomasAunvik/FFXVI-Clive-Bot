using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CliveBot.Application.SkillLanguages.Commands
{
    public class SkillLanguageRemove
    {
        public class Command : IRequest<List<SkillLanguageDto>>
        {
            public required int EditSkillId { get; set; }
            public required string EditLocale { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, List<SkillLanguageDto>>
        {
            private readonly IMediator _mediator;
            public Handler(ApplicationDbContext context, IConfiguration config, IMediator mediator) : base(context, config)
            {
                _mediator = mediator;
            }

            public async Task<List<SkillLanguageDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var language = await _context.SkillLanguages.FirstOrDefaultAsync(
                    s => s.SkillId == request.EditSkillId && s.Locale == request.EditLocale,
                    cancellationToken
                );

                if (language == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, $"Could not find any locale ({request.EditLocale}) on skillId: {request.EditSkillId}");
                }

                _context.SkillLanguages.Remove(language);

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                var languages = await _context.SkillLanguages
                    .Where(l => l.SkillId == request.EditSkillId)
                    .OrderBy(s => s.Locale)
                    .ToListAsync(cancellationToken);

                return languages.ConvertDto().ToList();
            }
        }
    }
}
