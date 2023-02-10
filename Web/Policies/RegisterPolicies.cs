using CliveBot.Web.Policies.Handlers;
using CliveBot.Web.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace CliveBot.Web.Policies
{
    public static class BotModPolicy
    {
        public readonly static string BotMod = "BotMod";
        public readonly static string ManageModerators = "ManageModerators";
        public readonly static string ManageSkills = "ManageSkills";
        public readonly static string ManageSkillInfo = "ManageSkillInfo";
        public readonly static string ManageSkillTranslations = "ManageSkillTranslations";
        public readonly static string ManageCharacters = "ManageCharacters";
        public readonly static string ManageCharacterInfo = "ManageCharacterInfo";
        public readonly static string ManageCharacterNotes = "ManageCharacterNotes";
    }

    public static class RegisterPolicies
    {
        public static AuthorizationOptions AddCustomPolicies(this AuthorizationOptions policies)
        {
            policies.AddPolicy(BotModPolicy.BotMod, policy =>
                policy.AddRequirements(new BotModeratorRequirement())
            );

            policies.AddBotModPolicies();

            return policies;
        }

        public static IServiceCollection AddPolicyHandlers(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, BotModeratorPolicyHandler>();
            services.AddSingleton<IAuthorizationHandler, BotModPermissionPolicyHandler>();
            return services;
        }

        public static AuthorizationOptions AddBotModPolicies(this AuthorizationOptions policies)
        {
            policies.AddPolicy(BotModPolicy.ManageModerators, policy =>
                policy.AddRequirements(new BotModPermissionRequirement(
                  user => (user.CanManageModerators, BotModPolicy.ManageModerators)
                ))
            );

            policies.AddPolicy(BotModPolicy.ManageSkills, policy =>
                policy.AddRequirements(new BotModPermissionRequirement(
                  user => (user.CanManageSkills, BotModPolicy.ManageSkills)
                ))
            );
            policies.AddPolicy(BotModPolicy.ManageSkillInfo, policy =>
                policy.AddRequirements(new BotModPermissionRequirement(
                  user => (user.CanManageSkillInfo, BotModPolicy.ManageSkillInfo)
                ))
            );
            policies.AddPolicy(BotModPolicy.ManageSkillTranslations, policy =>
                policy.AddRequirements(new BotModPermissionRequirement(
                  user => (user.CanManageSkillTranslations, BotModPolicy.ManageSkillTranslations)
                ))
            );

            policies.AddPolicy(BotModPolicy.ManageCharacters, policy =>
                policy.AddRequirements(new BotModPermissionRequirement(
                  user => (user.CanManageCharacters, BotModPolicy.ManageCharacters)
                ))
            );

            policies.AddPolicy(BotModPolicy.ManageCharacterInfo, policy =>
                policy.AddRequirements(new BotModPermissionRequirement(
                  user => (user.CanManageCharacterInfo, BotModPolicy.ManageCharacterInfo)
                ))
            );

            policies.AddPolicy(BotModPolicy.ManageCharacterNotes, policy =>
                policy.AddRequirements(new BotModPermissionRequirement(
                  user => (user.CanManageCharacterNotes, BotModPolicy.ManageCharacterNotes)
                ))
            );

            return policies;
        }
    }
}
