﻿using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CliveBot.Database.Models
{
    [Index(nameof(Name))]
    public class SkillLanguageModel
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
