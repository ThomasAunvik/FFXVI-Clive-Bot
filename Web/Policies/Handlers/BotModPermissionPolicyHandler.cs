using CliveBot.Database;
using CliveBot.Web.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CliveBot.Web.Policies.Handlers
{
    public class BotModPermissionPolicyHandler : AuthorizationHandler<BotModPermissionRequirement>
    {

        private readonly IServiceProvider _service;

        public BotModPermissionPolicyHandler(IServiceProvider service)
        {
            _service = service;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BotModPermissionRequirement requirement
        )
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                context.Fail(
                    new AuthorizationFailureReason(this, "User Id not found")
                );
                return;
            }

            if (Config.Owners.Any(o => o == userId))
            {
                context.Succeed(requirement);
                return;
            }

            using var scope = _service.CreateScope();
            using var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var user = await _db.BotModerators
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => 
                u.ConnectionSource == "Discord" &&
                u.ConnectionId == userId
            );
            if (user == null)
            {
                context.Fail(
                    new AuthorizationFailureReason(this, "User is not Bot Moderator")
                );
                return;
            }

            var permissions = user?.Permissions;
            if (permissions == null)
            {
                context.Fail(
                    new AuthorizationFailureReason(this, "User is missing a permission object.")
                );
                return;
            }

            (bool hasPermission, string permissionName) = requirement.PermissionCallback(permissions);
            if (!hasPermission)
            {
                context.Fail(
                    new AuthorizationFailureReason(this, "User does not have permission (" + permissionName + ")")
                );
                return;
            }

            context.Succeed(requirement);
        }
    }
}
