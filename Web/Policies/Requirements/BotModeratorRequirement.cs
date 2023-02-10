using Microsoft.AspNetCore.Authorization;

namespace CliveBot.Web.Policies.Requirements
{
    public class BotModeratorRequirement : IAuthorizationRequirement
    {
        public BotModeratorRequirement() { }
    }
}
