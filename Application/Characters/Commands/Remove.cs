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
    public class CharacterRemove
    {
        public class Command : IRequest<Unit>
        {
            public required int CharacterId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, Unit>
        {
            private readonly IMediator _mediator;
            public Handler(ApplicationDbContext context, IConfiguration config, IMediator mediator) : base(context, config)
            {
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var character = await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == request.CharacterId,
                    cancellationToken
                ) ?? throw new RestException(HttpStatusCode.NotFound, $"Could not find any character of id: {request.CharacterId}");

                _context.Characters.Remove(character);

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
