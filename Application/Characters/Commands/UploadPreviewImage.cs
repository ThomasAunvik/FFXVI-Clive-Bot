using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Azure;
using CliveBot.Database;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Characters.Commands
{
    public class CharacterPreviewImage
    {
        public class Command : IRequest<CharacterVariantDto>
        {
            public int VariantId { get; set; }

            public required IFormFile File { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, CharacterVariantDto>
        {
            private readonly AzureUpload _blob;

            public Handler(ApplicationDbContext context, IConfiguration config, AzureUpload blobClient) : base(context, config)
            {
                _blob = blobClient;
            }

            public async Task<CharacterVariantDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var variant = await _context.CharacterVariants
                    .FirstOrDefaultAsync(s => s.Id == request.VariantId, cancellationToken);

                if (variant == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any variant with id: " + request.VariantId);
                }

                var extension = Path.GetExtension(request.File.FileName);
                var filePath = $"/images/characters/{variant.CharacterId}/variants/{variant.Id}/preview{extension ?? ""}";

                await using (var fileStream = request.File.OpenReadStream()){
                    var blob = await _blob.Upload(
                        filePath,
                        fileStream,
                        request.File.ContentType,
                        cancellationToken
                        );

                    fileStream.Close();
                }

                var previewUrlFilePath = $"cdn;{filePath}";
                var isSame = variant.PreviewImageUrl == previewUrlFilePath;
                variant.PreviewImageUrl = previewUrlFilePath;

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0 && !isSame)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return variant.ConvertDto();
            }
        }
    }
}
