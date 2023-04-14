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
    public class CharacterVariantEdit
    {
        public class Command : CharacterVariantDto, IRequest<CharacterVariantDto>
        {
            public int VariantId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, CharacterVariantDto>
        {
            public Handler(ApplicationDbContext context, IConfiguration config) : base(context, config)
            {

            }

            public async Task<CharacterVariantDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var variant = await _context.CharacterVariants.FirstOrDefaultAsync(s => 
                        s.Id == request.VariantId, 
                        cancellationToken
                ) ?? throw new RestException(HttpStatusCode.NotFound, "Could not find any variant with id: " + request.VariantId);

                variant.Description = request.Description;
                variant.Age = request.Age;
                variant.PreviewImageUrl = request.PreviewImageUrl;

                variant.ToYear = request.ToYear;
                variant.FromYear = request.FromYear;

                if (request.DefaultVariant)
                {
                    await _context.CharacterVariants
                        .Where(v => v.CharacterId == variant.CharacterId)
                        .ForEachAsync((v) => v.DefaultVariant = false, cancellationToken);
                }

                variant.DefaultVariant = request.DefaultVariant;

                var current = variant.AdditionalFields?.Select(af => af.Id);

                var newVariants = request.AdditionalFields?
                    .Where(f => current == null || !current.Contains(f.Id))
                    .Select(v =>
                        new CharacterVariantField {
                        Variant = variant,
                        Title = v.Title,
                        Description = v.Description,
                    }).ToList();

                if(newVariants != null && newVariants.Any())
                {
                    await _context.CharacterVariantFields.AddRangeAsync(newVariants, cancellationToken);
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
