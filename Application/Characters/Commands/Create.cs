using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using CliveBot.Database.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CliveBot.Application.Characters.Commands
{
    public class CharacterCreate
    {
        public class Command : CharacterDto, IRequest<CharacterDto> { }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, CharacterDto>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config)
            {

            }

            public async Task<CharacterDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var character = new Character
                {
                    Name = request.Name,
                    Variants = new List<CharacterVariant>()
                    {
                        new CharacterVariant()
                        {
                            Description = "",
                            Age = 0,
                        }
                    }
                };

                await _context.Characters.AddAsync(character, cancellationToken);

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return character.ConvertDto();
            }
        }
    }
}
