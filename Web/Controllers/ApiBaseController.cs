using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CliveBot.Web.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public abstract class ApiBaseController : ControllerBase
    {
        protected string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected string? Username => User.FindFirstValue(ClaimTypes.Name);
        protected string? Discriminator => User.FindFirstValue("urn:discord:user:discriminator");
        protected string? Avatar => User.FindFirstValue("urn:discord:avatar:hash");
    }
}
