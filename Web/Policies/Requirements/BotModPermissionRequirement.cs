using CliveBot.Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace CliveBot.Web.Policies.Requirements
{
    public class BotModPermissionRequirement : IAuthorizationRequirement
    {
        public Func<BotModeratorPermissions, (bool, string)> PermissionCallback; 

        public BotModPermissionRequirement(Func<BotModeratorPermissions, (bool, string)> callback) {
            PermissionCallback = callback;
        }
    }
}
