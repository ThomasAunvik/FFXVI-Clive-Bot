using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.IO;

namespace CliveBot.Web.Controllers
{
    /// <summary>
    /// Creates redirect urls and challanges, and signing out
    /// </summary>
    public class AuthenticationController(IConfiguration config) : Controller
    {
        /// <summary>
        /// Signing in to the application trough Cookie
        /// </summary>
        /// <param name="redirect">Path to return to after authentication</param>
        /// <returns>Challange Result</returns>
        [HttpGet("~/signin")]
        public IActionResult SignIn(string? redirect)
        {
            var frontEndUrl = config.GetValue<string>("FrontendUrl");

            var redirectUri = "/";
            if (frontEndUrl != null)
            {
                redirectUri = frontEndUrl;
            }

            if (IsLocalUrl(redirect) && frontEndUrl != null)
            {
                Uri newUri = new(new(frontEndUrl), redirect);

                redirectUri = newUri.AbsoluteUri;
            }

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, "Discord");
        }

        private static bool IsLocalUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                return (url[0] == '/' && (url.Length == 1 ||
                        (url[1] != '/' && url[1] != '\\'))) ||   // "/" or "/foo" but not "//" or "/\"
                        (url.Length > 1 &&
                         url[0] == '~' && url[1] == '/');   // "~/" or "~/foo"
            }
        }

        /// <summary>
        /// Signs off the user
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        public IActionResult SignOutCurrentUser(string redirect)
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).

            var frontEndUrl = config.GetValue<string>("FrontendUrl");

            var redirectUri = "/";
            if (frontEndUrl != null)
            {
                redirectUri = frontEndUrl;
            }

            if (IsLocalUrl(redirect) && frontEndUrl != null)
            {
                Uri newUri = new(new(frontEndUrl), redirect);

                redirectUri = newUri.AbsoluteUri;
            }

            return SignOut(
                new AuthenticationProperties { RedirectUri = redirectUri },
                CookieAuthenticationDefaults.AuthenticationScheme
            );
        }
    }
}
