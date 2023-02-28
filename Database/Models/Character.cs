using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
    public class Character
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }

        public int? DefaultVariantFk;
        public CharacterVariant? DefaultVariant;

        public IEnumerable<CharacterVariant> Variants { get; set; } = new List<CharacterVariant>();
        public IEnumerable<CharacterNote> Notes { get; set; } = new List<CharacterNote>();
    }
}
