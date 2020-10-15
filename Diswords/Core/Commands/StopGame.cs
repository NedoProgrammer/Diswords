using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Diswords.Core.Helpers;

namespace Diswords.Core.Commands
{
    /// <summary>
    ///     A game command that stops the current game.
    /// </summary>
    public class StopGame : AdvancedContext
    {
        /// <summary>
        ///     Discord.NET method.
        ///     <remarks>Only the creator of the game can stop it.</remarks>
        /// </summary>
        /// <returns>nothing</returns>
        [Command("stop")]
        [Alias("стоп")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task Stop()
        {
            if (Client.Games.All(x => x.Creator.Id != Context.User.Id) ||
                !((IGuildUser) Context.User).GuildPermissions.Administrator)
            {
                await Context.Channel.SendMessageAsync(null, false,
                    EmbedHelper.BuildError(Locale, Locale.NotEnoughPermissions));
                return;
            }

            var game = Client.Games.FirstOrDefault(x => x.Creator.Id == Context.User.Id);
            game?.Stop();
        }
    }
}