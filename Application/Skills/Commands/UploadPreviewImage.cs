using CliveBot.Application.Errors;
using CliveBot.Application.Infrastructure;
using CliveBot.Database.Models;
using CliveBot.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CliveBot.Azure;
using System.Reflection.Metadata;
using Azure.Core;
using Microsoft.AspNetCore.Http;

namespace CliveBot.Application.Skills.Commands
{
    public class SkillUploadPreviewImage
    {
        public class Command : IRequest<SkillDto>
        {
            public int SkillId { get; set; }

            public required IFormFile File { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {

            }
        }

        public class Handler : BaseHandler, IRequestHandler<Command, SkillDto>
        {
            private readonly AzureUpload _blob;

            public Handler(ApplicationDbContext context, IConfiguration config, AzureUpload blobClient) : base(context, config)
            {
                _blob = blobClient;
            }

            public async Task<SkillDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken);

                if (skill == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                }

                var filePath = $"/images/skill/{skill.Id}/preview.png";

                using var fileStream = request.File.OpenReadStream();
                await _blob.Upload(
                    filePath, 
                    fileStream,
                    request.File.ContentType,
                    cancellationToken
                );
                fileStream.Close();

                var iconUrlFilePath = $"cdn;{filePath}";
                var isSame = skill.PreviewImageUrl == iconUrlFilePath;
                skill.PreviewImageUrl = iconUrlFilePath;

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result == 0 && !isSame)
                {
                    throw new RestException(HttpStatusCode.InternalServerError, "Database failed to save data");
                }

                return skill.ConvertDto();
            }
        }
    }
}
