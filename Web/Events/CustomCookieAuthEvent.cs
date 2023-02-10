using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace CliveBot.Web.Events
{
    public class XCookieAuthEvents : CookieAuthenticationEvents
    {
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.WriteAsJsonAsync(new { Error = "Unauthorized" });
            return Task.CompletedTask;
        }

        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = $"/signout";
            return base.RedirectToLogout(context);
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = $"/errors/accessdenied";
            return base.RedirectToAccessDenied(context);
        }

        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = $"/";
            return base.RedirectToReturnUrl(context);
        }
    }
}
