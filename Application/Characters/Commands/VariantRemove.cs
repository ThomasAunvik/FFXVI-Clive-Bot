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
    public class CharacterVariantRemove
    {
        public class Command : IRequest<Unit>
        {
            public required int VariantId { get; set; }
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
                var variant = await _context.CharacterVariants.FirstOrDefaultAsync(
                    c => c.Id == request.VariantId,
                    cancellationToken
                ) ?? throw new RestException(HttpStatusCode.NotFound, $"Could not find any variant of id: {request.VariantId}");

                _context.CharacterVariants.Remove(variant);

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
