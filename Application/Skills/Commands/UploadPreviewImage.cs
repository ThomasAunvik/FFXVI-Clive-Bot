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

        public class Handler(ApplicationDbContext context, IConfiguration config, AzureUpload blobClient) : BaseHandler(context, config), IRequestHandler<Command, SkillDto>
        {
            public async Task<SkillDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var skill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken)
                    ?? throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                
                string extension = Path.GetExtension(request.File.FileName);

                string filePath = $"/images/skill/{skill.Id}/preview{extension ?? ""}";

                using var fileStream = request.File.OpenReadStream();
                var blobResult = await blobClient.Upload(
                    filePath, 
                    fileStream,
                    request.File.ContentType,
                    cancellationToken
                );
                fileStream.Close();

                var iconUrlFilePath = $"cdn;{blobResult.Path}";
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