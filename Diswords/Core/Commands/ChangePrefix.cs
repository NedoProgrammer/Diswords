#nullable enable
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Diswords.Core.Helpers;

namespace Diswords.Core.Commands
{
    /// <summary>
    ///     Moderator command that changes the prefix of the server.
    /// </summary>
    public class ChangePrefix : AdvancedContext
    {
        /// <summary>
        ///     The Discord.NET method..
        /// </summary>
        /// <param name="prefix">The new prefix of the server.</param>
        /// <returns>nothing</returns>
        [Command("prefix", RunMode = RunMode.Async)]
        [Alias("префикс")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task SetPrefix(string prefix)
        {
            //The loading gif :D
            RestUserMessage? loading = null;
            if (!string.IsNullOrEmpty(Client.Config.LoadingGif))
                loading = await Context.Channel.SendFileAsync(Client.Config.LoadingGif);
            //This code is kind of bad but i don't know how to improve it
            var guild = Client.Guilds.First(g => g.Id == Context.Guild.Id);
            guild.Prefix = prefix;
            guild = DiswordsClient.StaticGuilds.First(g => g.Id == Context.Guild.Id);
            guild.Prefix = prefix;
            var path =
                $"{Client.Config.RootDirectory}{Path.DirectorySeparatorChar}{Client.Config.GuildsDirectoryName}{(Client.Config.GuildsDirectoryName == "" ? "" : Path.DirectorySeparatorChar.ToString())}{guild.Id}.json";
            //Write the new server to the file.
            guild.OverwriteTo(path);
            if (loading != null) await loading.DeleteAsync();
            await Context.Channel.SendMessageAsync(null, false, EmbedHelper.BuildSuccess(Locale, Locale.PrefixChanged));
        }
    }
}