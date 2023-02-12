using CliveBot.Bot.Handler.Utils;
using Discord;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;

namespace CliveBot.Bot.Commands
{
    public class Help : InteractionModuleBase
    {
        private readonly IServiceProvider services;

        public Help(IServiceProvider provider)
        {
            services = provider;
        }

        [SlashCommand("help", "Displays basic help command.\nOption: [Command] or 'verbose: All' for all possible commands")]
        public async Task HelpCommand(
            [Summary(description: "Search for available commands")]
            string search = "",
            [Summary(description: "Show all commands")]
            [Choice("All", "all")]
            [Choice("Limited", "limited")]
            string verbose = "limited"
        )
        {
            EmbedHandler embed = new(Context.User);


            bool all = verbose.Equals("all");
            if (string.IsNullOrWhiteSpace(search))
            {
                await HelpCommandShow(embed, all);
            }
            else
            {
                HelpCommandSearch(embed, search, services, Context);
            }
            
            await Context.Interaction.RespondAsync(embed: embed.Build(), ephemeral: true);
        }

        async Task HelpCommandShow(EmbedHandler embed, bool all = false)
        {
            embed.Description = $"These are the commands you can use \nFor more detailed command explanations type `/help <command>`" +
                (all ? "\nor if there are missing permissions." : "\nAdd 'all' argument to get all possible commands without the permission check.");

            var interactionService = services.GetRequiredService<InteractionService>();

            foreach (var module in interactionService.Modules)
            {
                var name = module.Name;

                var available = await module.SlashCommands.ToAsyncEnumerable().WhereAwait(async (x) => (await x.CheckPreconditionsAsync(Context, services)).IsSuccess).ToListAsync();

                string description = string.Join(", ", available.Select(x => x.Name).Distinct());


                string notSuccess = string.Empty;
                if (all)
                {
                    var notAvailable = await module.SlashCommands.ToAsyncEnumerable().WhereAwait(async (x) => !(await x.CheckPreconditionsAsync(Context, services)).IsSuccess).ToListAsync();
                    notSuccess = string.Join(", ", notAvailable.Select(x => x.Name).Distinct());
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    embed.AddField(x =>
                    {
                        x.Name = name;
                        x.Value =
                            (description != string.Empty ? description + "\n" : "") +
                            (notSuccess != string.Empty ? "Missing Permissions:\n" + notSuccess : "");
                        x.IsInline = false;
                    });
                }
            }
        }

        internal static void HelpCommandSearch(EmbedHandler builder, string search, IServiceProvider services, IInteractionContext Context)
        {
            var interactionService = services.GetRequiredService<InteractionService>();
            var similarCommands = interactionService.SlashCommands.Where((command) => command.Name.Contains(search));

            if (!similarCommands.Any())
            {
                builder.Title = $"Sorry, couldn't find a command like {search}.";
                builder.Description = "";
                return;
            }

            bool isOwner = Config.botOwners.Contains(Context.User.Id);

            foreach (var cmd in similarCommands)
            {
                builder.AddField(x =>
                {
                    x.Name = cmd.Name;
                    x.Value = $"**Usage:** {cmd.Name} {string.Join(" ", cmd.Parameters.Select(p => !p.IsRequired ? $"[{p.Name}]" : $"<{p.Name}>"))}\n" +
                             $"{cmd.Description}";
                    x.IsInline = false;
                });

                if (cmd.Preconditions.Count > 0 || cmd.Module.Preconditions.Count > 0)
                {
                    List<PreconditionAttribute> preconditions = cmd.Preconditions.ToList();
                    preconditions.AddRange(cmd.Module.Preconditions);

                    List<PreconditionAttribute> sameGroup = new();
                    string preconditionString = string.Empty;
                    for (int i = 0; i < preconditions.Count; i++)
                    {
                        PreconditionAttribute attrib = preconditions[i];
                        var same = preconditions.FindAll(x => x.Group == attrib.Group && x.Group != null);
                        if (same.Count != 0)
                        {
                            sameGroup.AddRange(same);
                            i += same.Count - 1;
                        }
                        else sameGroup.Add(attrib);

                        preconditionString += string.Join("\n**Or**\n", sameGroup.Select(x =>
                        {
                            if (x is RequireUserPermissionAttribute userPerm) return userPerm.ChannelPermission != null ? ("User Channel Permission: " + userPerm.ChannelPermission.ToString()) : ("User Guild Permission: " + userPerm.GuildPermission.ToString());
                            if (x is RequireBotPermissionAttribute botPerm) return botPerm.ChannelPermission != null ? ("Bot Channel Permission: " + botPerm.ChannelPermission.ToString()) : ("Bot Guild Permission: " + botPerm.GuildPermission.ToString());
                            if (x is RequireOwnerAttribute) return "Bot Owner Permission";
                            return x.GetType().Name;
                        })
                        ) + "\n";

                        sameGroup = new List<PreconditionAttribute>();
                    }

                    builder.AddField(x =>
                    {
                        x.Name = "Preconditions";
                        x.Value = preconditionString;
                        x.IsInline = false;
                    });
                }
            }
            if (builder.Fields.Count <= 0)
            {
                builder.Title = $"Sorry, couldn't find a command like {search}.";
                builder.Description = "";
            }
        }

        [SlashCommand("contact", "Send a message directly to the developer")]
        public async Task Contact()
        {
            
            ButtonBuilder joinServer = new("Join FFXVI Server", style: ButtonStyle.Link, url: "https://discord.gg/y34bsEg");
            ButtonBuilder joinSupportServer = new("Join Support Server", style: ButtonStyle.Link, url: "https://discord.gg/6NhU8Mdp4W");
            ButtonBuilder sendMessage = new("Send Message", "send-message");
            

            ActionRowBuilder actionRow = new();
            actionRow.WithButton(joinServer);
            actionRow.WithButton(joinSupportServer);
            actionRow.WithButton(sendMessage);

            ComponentBuilder component = new();
            component.AddRow(actionRow);

            await Context.Interaction.RespondAsync(components: component.Build(), ephemeral: true);
        }

        [SlashCommand("github", "Gets the GitHub link of the project")]
        public async Task GithubLink()
        {
            await Context.Interaction.RespondAsync("FFXVI-Clive-Bot Project: https://github.com/ThomasAunvik/FFXVI-Clive-Bot");
        }

        [SlashCommand("gitstatus", "Gets the current patch status for this bot")]
        public async Task GitStatus()
        {
            EmbedHandler embed = new(Context.User, "Git Status");

            string commitText = "None";
            if (!string.IsNullOrEmpty(Config.CURRENT_COMMIT))
            {
                commitText = "[" + Config.CURRENT_COMMIT[..7] + "](" +
                             "https://github.com/ThomasAunvik/FFXVI-Clive-Bot/commit/" + Config.CURRENT_COMMIT + ")";
            }

            embed.AddFieldSecure("Commit", commitText);
            embed.AddFieldSecure("Status", string.IsNullOrEmpty(Config.GIT_STATUS) ? "None" : Config.GIT_STATUS);
            await Context.Interaction.RespondAsync(embed: embed.Build());
        }
    }
}
