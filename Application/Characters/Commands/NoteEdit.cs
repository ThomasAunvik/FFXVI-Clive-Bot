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
    public class CharacterNoteEdit
    {
        public class Command : CharacterNoteDto, IRequest<CharacterNoteDto>
        {
            public int NoteId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() { }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, CharacterNoteDto>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config) { }

            public async Task<CharacterNoteDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var note = await _context.CharacterNotes
                    .FirstOrDefaultAsync(s => s.Id == request.NoteId, cancellationToken);

                if (note == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any character with id: " + request.CharacterId);
                }

                note.NoteName = request.NoteName;
                note.NoteDescription = request.NoteDescription;
                note.Locale = request.Locale;
                note.PreviewImageUrl = request.PreviewImageUrl;

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
