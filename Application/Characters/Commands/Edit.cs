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

namespace CliveBot.Application.Characters.Commands
{
    public class CharacterEdit
    {
        public class Command : CharacterDto, IRequest<CharacterDto>
        {
            public int CharacterId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() { }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Command, CharacterDto>
        {
            public async Task<CharacterDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var character = await _context.Characters
                    .FirstOrDefaultAsync(s => s.Id == request.CharacterId, cancellationToken);

                if (character == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any character with id: " + request.CharacterId);
                }

                character.Name = request.Name;

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
