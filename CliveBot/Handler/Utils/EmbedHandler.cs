using Discord;
using System.Runtime.ConstrainedExecution;

namespace CliveBot.Bot.Handler.Utils
{
    internal class EmbedHandler : EmbedBuilder
    {
        const int MAX_FIELD_VALUE_LENGTH = 2048;
        static readonly Color EMBED_COLOR = new(114, 137, 218);

        private IUser? _user;
        public IUser? User => _user;
        private void SetUser(IUser user) => _user = user;

        public EmbedHandler(IUser? user = null, string title = "", string description = "", bool debug = false)
        {
            _user = user;

            Title = title;
            Description = description;

            if (user != null)
            {
                Author = new EmbedAuthorBuilder() { 
                    Name = user.Username + "#" + user.Discriminator, 
                    IconUrl = user.GetAvatarUrl() 
                };
            }

            Color = EMBED_COLOR;
        }

        public EmbedHandler(IGuildUser? user = null, string title = "", string description = "", bool debug = false)
        {
            _user = user;

            Title = title;
            Description = description;

            if (user != null)
            {
                Author = new EmbedAuthorBuilder() { 
                    Name = user.Username + "#" + user.Discriminator, 
                    IconUrl = user.GetGuildAvatarUrl() 
                };
            }

            Color = EMBED_COLOR;
        }

        public int AddFieldSecure(string name, object value, bool inline = false)
        {
            if (value is string stringValue)
            {
                value = SecureEmbedText(stringValue);
            }
            AddField(name, value, inline);
            return Fields.Count - 1; // Index value of field for Fields.
        }

        public void AddFieldSecure(EmbedFieldBuilder field)
        {
            if (field.Value is string stringValue)
            {
                field.Value = SecureEmbedText(stringValue);
            }
            AddField(field);
        }

        public static string SecureEmbedText(string value)
        {
            if (value == null) return string.Empty;

            if (value.Contains("<br>")) value = value.Replace("<br>", "\n");
            if (value.Contains("\\n")) value = value.Replace("\\n", "\n");
            if (value.Contains("\n\r")) value = value.Replace("\n\r", "\n");
            if (value.Contains("\n\n")) value = value.Replace("\n\n", "\n");

            if (value.Length > MAX_FIELD_VALUE_LENGTH)
            {
                value = string.Concat(value, MAX_FIELD_VALUE_LENGTH - 3) + "...";
            }

            return value;
        }

    }
}
