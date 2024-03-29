﻿using CliveBot.Database.Models;

namespace CliveBot.Application.Characters
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public CharacterVariantDto? DefaultVariant;

        public IEnumerable<CharacterVariantDto>? Variants { get; set; }
        public IEnumerable<CharacterNoteDto>? Notes { get; set; }
    }

    public static class CharacterDtoExtension
    {
        public static CharacterDto ConvertDto(this Character model)
        {
            return new CharacterDto()
            {
                Id = model.Id,
                Name = model.Name,
                Variants = model.Variants.ConvertDto(),
                Notes = model.Notes.ConvertDto(),
            };
        }

        public static IEnumerable<CharacterDto> ConvertDto(this IEnumerable<Character> listModels)
        {
            return listModels.Select(x => x.ConvertDto());
        }
    }
}
