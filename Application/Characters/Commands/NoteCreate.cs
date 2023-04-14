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
    public class CharacterNoteCreate
    {
        public class Command : CharacterNoteDto, IRequest<CharacterNoteDto> { }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() { }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, CharacterNoteDto>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<CharacterNoteDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var character = await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == request.CharacterId, 
                    cancellationToken
                ) ?? throw new RestException(HttpStatusCode.NotFound, "Could not find character of id: " + request.CharacterId);

                var note = new CharacterNote
                {
                    Character = character,   
                    NoteName = request.NoteName,
                    NoteDescription = request.NoteDescription,
                    Locale = request.Locale,
                    PreviewImageUrl = request.PreviewImageUrl,
                };

                await _context.CharacterNotes.AddAsync(note, cancellationToken);

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return note.ConvertDto();
            }
        }
    }
}
