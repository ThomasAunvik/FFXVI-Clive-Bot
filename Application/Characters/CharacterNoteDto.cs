using CliveBot.Application.SkillLanguages;
using CliveBot.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Application.Characters
{
    public class CharacterNoteDto
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public required string NoteName { get; set; }
        public required string NoteDescription { get; set; }
        public required string Locale { get; set; }

        public string? PreviewImageUrl { get; set; }
    }

    public static class CharacterNoteDtoExtension
    {
        public static CharacterNoteDto ConvertDto(this CharacterNote model)
        {
            return new CharacterNoteDto()
            {
                Id = model.Id,
                CharacterId = model.CharacterId,
                NoteName = model.NoteName,
                NoteDescription = model.NoteDescription,
                Locale = model.Locale,
                PreviewImageUrl = model.PreviewImageUrl,
            };
        }

        public static IEnumerable<CharacterNoteDto> ConvertDto(this IEnumerable<CharacterNote> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
