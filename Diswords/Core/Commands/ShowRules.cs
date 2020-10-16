using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Diswords.Core.Helpers;

namespace Diswords.Core.Commands
{
    /// <summary>
    ///     Show rules of the game "words".
    /// </summary>
    public class ShowRules : AdvancedContext
    {
        /// <summary>
        ///     Discord.NET method...
        /// </summary>
        /// <returns></returns>
        [Command("rules")]
        [Alias("правила")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task Rules()
        {
            var embedBuilder = new EmbedBuilder().WithColor(Color.Orange).WithTitle(Locale.Commands)
                .WithDescription(Locale.Rules);
            await Context.Channel.SendMessageAsync(null, false, embedBuilder.Build());
        }
    }
}