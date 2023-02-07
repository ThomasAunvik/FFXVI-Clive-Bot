namespace CliveBot.Web.Objects
{
    public class DiscordUserDto
    {
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public required string Discriminator { get; set; }
        public string? Avatar { get; set; }
    }
}
