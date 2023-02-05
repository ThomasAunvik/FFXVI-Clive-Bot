using CliveBot.Bot.Commands;
using CliveBot.Database.Models;
using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Bot.Handler.Modals
{
    public class ModalSkillLanguage : IModal
    {
        public ModalSkillLanguage() { }

        [RequiredInput]
        [InputLabel("Name")]
        [ModalTextInput("skill_language_name", placeholder: "Jump", maxLength: 25)]
        public string Name { get; set; } = string.Empty;

        // Additional paremeters can be specified to further customize the input.    
        // Parameters can be optional
        [RequiredInput(false)]
        [InputLabel("Description")]
        [ModalTextInput("skill_language_description", TextInputStyle.Paragraph, $"Hold {SkillCommand.emote_button_x} Button to Jump", maxLength: 300)]
        public string Description { get; set; } = string.Empty;

        public string Title => "Edit Skill Language";
    }
}
