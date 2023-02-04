using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliveBot.Bot.Handler.Utils
{
    public enum LocaleOptions
    {
        [ChoiceDisplay("English")]
        en,
        [ChoiceDisplay("Norwegian")]
        no,
        [ChoiceDisplay("Japanese")]
        jp,
        [ChoiceDisplay("German")]
        de,
        [ChoiceDisplay("French")]
        fr
    }
}
