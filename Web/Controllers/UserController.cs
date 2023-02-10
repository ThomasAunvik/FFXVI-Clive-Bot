using CliveBot.Web.Objects;
using Microsoft.AspNetCore.Mvc;

namespace CliveBot.Web.Controllers
{
    public class UserController : ApiBaseController
    {
        [HttpGet("current")]
        public DiscordUserDto GetCurrentUser()
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
