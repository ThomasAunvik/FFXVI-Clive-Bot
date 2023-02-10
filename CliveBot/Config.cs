namespace CliveBot.Bot
{
    public static class Config
    {
        public static readonly List<ulong> botOwners = new() {
            96580514021912576
        };

        public static readonly List<ulong> botAdmin = new() {
            96580514021912576
        };

        public static string CURRENT_COMMIT = "";
        public static string GIT_STATUS = "";

        public static string? UrlCdnConvert(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            return url.Replace("cdn;", "https://cdn.xvibot.com/");
        }
    }
}
