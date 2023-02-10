using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliveBot.Web.Policies
{
    public class ModAuthorize : TypeFilterAttribute
    {
        public ModAuthorize(
            bool ManageModerators = false,
            bool ManageSkills = false,
            bool ManageSkillInfo = false,
            bool ManageSkillTranslations = false,
            bool ManageCharacters = false,
            bool ManageCharacterInfo = false,
            bool ManageCharacterNotes = false,
            string? AdditionalPolicy = null
        ) : base(typeof(AuthorizeAllPolicyFilter)) {
            List<string> policies = new() { BotModPolicy.BotMod };
            if (ManageModerators) policies.Add(BotModPolicy.ManageModerators);
            if (ManageSkills) policies.Add(BotModPolicy.ManageSkills);
            if (ManageSkillInfo) policies.Add(BotModPolicy.ManageSkillInfo);
            if (ManageSkillTranslations) policies.Add(BotModPolicy.ManageSkillTranslations);
            if (ManageCharacters) policies.Add(BotModPolicy.ManageCharacters);
            if (ManageCharacterInfo) policies.Add(BotModPolicy.ManageCharacterInfo);
            if (ManageCharacterNotes) policies.Add(BotModPolicy.ManageCharacterNotes);
            if (AdditionalPolicy != null) policies.Add(AdditionalPolicy);

            var policyString = string.Join(",", policies);
            Arguments = new object[] { policyString };
        }
    }
}
