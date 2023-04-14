using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using CliveBot.Database.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;

namespace CliveBot.Application.Characters.Commands
{
    public class CharacterVariantCreate
    {
        public class Command : CharacterVariantDto, IRequest<CharacterVariantDto> { }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() { }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, CharacterVariantDto>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<CharacterVariantDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var character = await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == request.CharacterId, 
                    cancellationToken
                ) ?? throw new RestException(HttpStatusCode.NotFound, "Could not find character of id: " + request.CharacterId);

                var variant = new CharacterVariant
                {
                    Character = character,
                    Age = request.Age,
                    DefaultVariant = request.DefaultVariant,
                    Description = request.Description,
                    AdditionalFields = request.AdditionalFields?
                        .Select(f => 
                            new CharacterVariantField { 
                                Title = f.Title, 
                                Description = f.Description,
                        }),
                    PreviewImageUrl = request.PreviewImageUrl,
                };

                await _context.CharacterVariants.AddAsync(variant, cancellationToken);

                if (request.DefaultVariant)
                {
                    await _context.CharacterVariants
                        .Where(v => v.CharacterId == character.Id)
                        .ForEachAsync((v) => v.DefaultVariant = false, cancellationToken);
                }

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return variant.ConvertDto();
            }
        }
    }
}
