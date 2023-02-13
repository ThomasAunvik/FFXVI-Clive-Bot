using Serilog;
using System.Net.NetworkInformation;

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
            return url.Replace("cdn;", "https://cdn.xvibot.com");
        }

        public static async Task ReadConfig()
        {
            if (File.Exists("current_commit.txt"))
            {
                CURRENT_COMMIT = await File.ReadAllTextAsync("current_commit.txt");
                Log.Logger.Information("Current Commit: " + CURRENT_COMMIT);
            }

            if (File.Exists("git_status.txt"))
            {
                GIT_STATUS = await File.ReadAllTextAsync("git_status.txt");
                Log.Logger.Information("Current Git Status: " + GIT_STATUS);
            }
        }
    }
}
