using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace CliveBot.Web.Events
{
    /// <summary>
    /// Cookie Authentication Event Controller
    /// </summary>
    public class XCookieAuthEvents : CookieAuthenticationEvents
    {
        /// <summary>
        /// If logged off, only return a 401 error
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.WriteAsJsonAsync(new { Error = "Unauthorized" });
            return Task.CompletedTask;
        }

        /// <summary>
        /// Redirects to Signout Page
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = $"/signout";
            return base.RedirectToLogout(context);
        }

        /// <summary>
        /// Redirects to Access Denied Page
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = $"/errors/accessdenied";
            return base.RedirectToAccessDenied(context);
        }
        
        /// <summary>
        /// Redirects to Main Page
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            context.RedirectUri = $"/";
            return base.RedirectToReturnUrl(context);
        }
    }
}
