using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CliveBot.Application.Characters.Commands
{
    public class CharacterNoteRemove
    {
        public class Command : IRequest<Unit>
        {
            public required int NoteId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Command, Unit>
        {
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var note = await _context.CharacterNotes.FirstOrDefaultAsync(
                    c => c.Id == request.NoteId,
                    cancellationToken
                ) ?? throw new RestException(HttpStatusCode.NotFound, $"Could not find any note of id: {request.NoteId}");

                _context.CharacterNotes.Remove(note);

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return Unit.Value;
            }
        }
    }
}
