using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
    public class CharacterVariant
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public required string Description { get; set; }
        public bool DefaultVariant { get; set; } = false;

        public int? Age { get; set; }
        public IEnumerable<CharacterVariantField>? AdditionalFields { get; set; } = new List<CharacterVariantField>();

        public string? PreviewImageUrl { get; set; }

        public Character? Character { get; set; }
    }
}
