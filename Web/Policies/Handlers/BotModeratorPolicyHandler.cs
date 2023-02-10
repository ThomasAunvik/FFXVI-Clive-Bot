using CliveBot.Database;
using CliveBot.Web.Policies.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CliveBot.Web.Policies.Handlers
{
    public class BotModeratorPolicyHandler : AuthorizationHandler<BotModeratorRequirement>
    {
        private readonly IServiceProvider _service;

        public BotModeratorPolicyHandler(IServiceProvider service) {
            _service = service;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            BotModeratorRequirement requirement
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

            if(Config.Owners.Any(o => o == userId))
            {
                context.Succeed(requirement);
                return;
            }

            using var scope = _service.CreateScope();
            using var _db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var isModerator = await _db.BotModerators.AnyAsync(u => 
                u.ConnectionSource == "Discord" &&
                u.ConnectionId == userId
            );

            if(!isModerator)
            {
                context.Fail(
                    new AuthorizationFailureReason(this, "User is not a Bot Moderator")
                );
                return;
            }

            context.Succeed(requirement);
        }
    }
}
