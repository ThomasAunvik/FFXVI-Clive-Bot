using CliveBot.Application.Infrastructure;
using CliveBot.Database.Models;
using CliveBot.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliveBot.Application.Errors;
using System.Net;

namespace CliveBot.Application.Characters.Queries
{
    public class CharacterDetails
    {
        public class Query : IRequest<CharacterDto>
        {
            public int CharacterId { get; set; }
        }

        public class Handler(ApplicationDbContext context, IConfiguration config) : BaseHandler(context, config), IRequestHandler<Query, CharacterDto>
        {
            public async Task<CharacterDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var character = await _context.Characters
                    .Include(s => s.Variants)
                        .ThenInclude(v => v.AdditionalFields)
                    .FirstOrDefaultAsync(s => s.Id == request.CharacterId, cancellationToken);

                if(character == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any character with id: " + request.CharacterId);
                }

                var characterDto = character.ConvertDto();

                characterDto.DefaultVariant = characterDto.Variants?.FirstOrDefault(v => v.DefaultVariant);
                return characterDto;
            }
        }
    }
}
