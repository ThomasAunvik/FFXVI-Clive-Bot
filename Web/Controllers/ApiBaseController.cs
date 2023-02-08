using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MediatR;

namespace CliveBot.Web.Controllers
{   
    /// <summary>
    /// Base Controller, with Cookie Authentication
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public abstract class ApiBaseController : ControllerBase
    {
        private IMediator? _mediator;
        /// <summary>
        /// Mediator, Requests to Application Project
        /// </summary>
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        /// <summary>
        /// Discord User Id
        /// </summary>
        protected string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// Discord Username
        /// </summary>
        protected string? Username => User.FindFirstValue(ClaimTypes.Name);

        /// <summary>
        /// Discord Discriminator (4 numbers)
        /// </summary>
        protected string? Discriminator => User.FindFirstValue("urn:discord:user:discriminator");

        /// <summary>
        /// Hash of the Discord User Avatar
        /// </summary>
        protected string? Avatar => User.FindFirstValue("urn:discord:avatar:hash");

    }
}
