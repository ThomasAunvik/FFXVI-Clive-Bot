using CliveBot.Web.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CliveBot.Web.Controllers
{
    /// <summary>
    /// User Endpoint
    /// </summary>
    public class UserController : ApiBaseController
    {
        /// <summary>
        /// Gets the currently logged in user.
        /// </summary>
        /// <returns>Discord Info, UserId, Username, Discriminator and Avatar</returns>
        /// <exception cref="Exception">If not authenticated, but still passes [Authorize], returns an Exception</exception>
        [HttpGet("current")]
        public ActionResult<DiscordUserDto> GetCurrentUser()
        {
            if(UserId == null || Username == null || Discriminator == null)
            {
                throw new Exception("Failed to parse discord data");
            }

            return new DiscordUserDto()
            {
                UserId = UserId,
                Username = Username,
                Discriminator = Discriminator,
                Avatar = Avatar,
            };
        }
    }
}
