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

namespace CliveBot.Application.Skills.Commands
{
    public class SkillUploadIconImage
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
                    .FirstOrDefaultAsync(s => s.Id == request.SkillId, cancellationToken) 
                    ?? throw new RestException(HttpStatusCode.NotFound, "Could not find any skill with id: " + request.SkillId);
                
                string extension = Path.GetExtension(request.File.FileName);

                string filePath = $"/images/skill/{skill.Id}/icon{extension ?? ""}";

                using var fileStream = request.File.OpenReadStream();
                var blobResult = await _blob.Upload(
                    filePath,
                    fileStream, 
                    request.File.ContentType, 
                    cancellationToken
                );

                fileStream.Close();

                var iconUrlFilePath = $"cdn;{blobResult.Path}";
                var isSame = skill.IconUrl == iconUrlFilePath;
                skill.IconUrl = iconUrlFilePath;

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
