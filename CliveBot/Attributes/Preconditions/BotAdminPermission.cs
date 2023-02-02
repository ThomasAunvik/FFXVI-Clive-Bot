using Discord;
using Discord.Interactions;

namespace CliveBot.Bot.Attributes.Preconditions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BotAdminPermission : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            var user = context.User;

            var isBotOwner = Config.botOwners.Contains(user.Id);
            var isBotAdmin = Config.botAdmin.Contains(user.Id);

            if(isBotOwner || isBotAdmin)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError("User does not have Bot Owner or Bot Admin permission to execute this interaction."));
        }
    }
}
