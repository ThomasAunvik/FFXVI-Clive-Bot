using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
    public class CharacterVariantField
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }

        public CharacterVariant? Variant { get; set; }
    }
}
