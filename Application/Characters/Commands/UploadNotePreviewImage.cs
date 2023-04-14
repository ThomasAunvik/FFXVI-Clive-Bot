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
    public class CharacterNotePreviewImage
    {
        public class Command : IRequest<CharacterNoteDto>
        {
            public int NoteId { get; set; }

            public required IFormFile File { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, CharacterNoteDto>
        {
            private readonly AzureUpload _blob;

            public Handler(ApplicationDbContext context, IConfiguration config, AzureUpload blobClient) : base(context, config)
            {
                _blob = blobClient;
            }

            public async Task<CharacterNoteDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var note = await _context.CharacterNotes
                    .FirstOrDefaultAsync(s => s.Id == request.NoteId, cancellationToken);

                if (note == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any note with id: " + request.NoteId);
                }

                var extension = Path.GetExtension(request.File.FileName);
                var filePath = $"/images/characters/{note.CharacterId}/notes/{note.Id}/preview{extension ?? ""}";

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
                var isSame = note.PreviewImageUrl == previewUrlFilePath;
                note.PreviewImageUrl = previewUrlFilePath;

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0 && !isSame)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return note.ConvertDto();
            }
        }
    }
}
