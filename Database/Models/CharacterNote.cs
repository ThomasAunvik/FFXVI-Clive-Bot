using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Database.Models
{
    public class CharacterNote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CharacterId { get; set; }

        public required string NoteName { get; set; }
        public required string NoteDescription { get; set; }

        public required string Locale { get; set; }


        public string? PreviewImageUrl { get; set; }


        public Character? Character { get; set; }
    }
}
