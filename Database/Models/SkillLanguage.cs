using System.ComponentModel.DataAnnotations.Schema;

namespace CliveBot.Database.Models
{
    public class SkillLanguage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SkillId { get; set; }
        public string Locale { get; set; } = "en";
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public SkillModel? Skill { get; set; }
    }
}
