using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Diswords.Core.Helpers;

namespace Diswords.Core.Commands
{
    /// <summary>
    ///     Just like a "help command" :D
    /// </summary>
    public class ShowCommands : AdvancedContext
    {
        /// <summary>
        ///     Discord.NET method..
        /// </summary>
        /// <returns>nothing</returns>
        [Command("commands")]
        [Alias("команды")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task PrintCommands()
        {
            var embedBuilder = new EmbedBuilder().WithColor(Color.Orange).WithTitle(Locale.Commands)
                .WithDescription(string.Format(Locale.Help, Client.Guilds.First(g => g.Id == Context.Guild.Id).Prefix) +
                                 $"\n{Locale.OpenSource}");
            await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
        }
    }
}