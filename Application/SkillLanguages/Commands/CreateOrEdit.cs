using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using CliveBot.Database.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CliveBot.Application.SkillLanguages.Commands
{
    public class ModeratorCreateOrEdit
    {
        public class Command : SkillLanguageDto, IRequest<List<SkillLanguageDto>>
        {
            public int EditSkillId { get; set; }
            public string EditLocale { get; set; } = string.Empty;
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

                var skill = await _context.Skills
                    .Include(s => s.Localized.Where(l => l.Locale == request.EditLocale))
                    .FirstOrDefaultAsync(s => s.Id == request.EditSkillId, cancellationToken);
                
                if (skill == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, $"Could not find any skill with id: " + request.EditSkillId);
                }

                var language = skill.Localized.FirstOrDefault();

                if (language == null)
                {
                    skill.Localized.Add(new SkillLanguageModel()
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Locale = request.EditLocale
                    });
                } else {
                    language.Name = request.Name;
                    language.Description = request.Description;
                }

                if(request.EditLocale == "en")
                {
                    skill.Name = request.Name;
                    skill.Description = request.Description ?? string.Empty;
                }

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
