using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;

namespace CliveBot.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet("~/signin")]
        public IActionResult SignIn(string? redirect)
        {
            var redirectUri = "/";
            if(isLocalUrl(redirect))
            {
                redirectUri = redirect;
            }

            return Challenge(new AuthenticationProperties { RedirectUri = redirectUri }, "Discord");
        }

        private bool isLocalUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                return ((url[0] == '/' && (url.Length == 1 ||
                        (url[1] != '/' && url[1] != '\\'))) ||   // "/" or "/foo" but not "//" or "/\"
                        (url.Length > 1 &&
                         url[0] == '~' && url[1] == '/'));   // "~/" or "~/foo"
            }
        }

        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        public IActionResult SignOutCurrentUser()
        {
            // Instruct the cookies middleware to delete the local cookie created
            // when the user agent is redirected from the external identity provider
            // after a successful authentication flow (e.g Google or Facebook).
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
