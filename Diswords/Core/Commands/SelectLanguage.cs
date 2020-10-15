#nullable enable
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Diswords.Core.Helpers;
using Diswords.Locales;

namespace Diswords.Core.Commands
{
    /// <summary>
    ///     Moderator command that changes the default language of the server.
    /// </summary>
    public class SelectLanguage : AdvancedContext
    {
        /// <summary>
        ///     Discord.NET method..
        ///     <para>Prints a list of all available languages.</para>
        /// </summary>
        /// <returns>nothing</returns>
        [Command("language")]
        [Alias("язык")]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task ListLanguages()
        {
            var description = Client.Languages.Aggregate("",
                (current, language) =>
                    current + $"\n• {language.FlagEmoji.Name} {language.Name} - `{language.ShortName}`");
            var embed = new EmbedBuilder().WithTitle(Locale.Languages).WithColor(Color.Blue)
                .WithDescription(description).Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }

        /// <summary>
        ///     Discord.NET method..
        ///     <para>Sets the new language (from the list, see <see cref="ListLanguages" />)</para>
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns></returns>
        [Command("language")]
        [Alias("язык")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        public async Task Select(string shortName)
        {
            //The requested language was not found.
            if (Client.Languages.All(l => l.ShortName != shortName))
            {
                await Context.Channel.SendMessageAsync(null, false,
                    EmbedHelper.BuildError(Locale, string.Format(Locale.InvalidLanguage, shortName)));
            }
            else
            {
                //The loading GIF :D
                RestUserMessage? loading = null;
                if (!string.IsNullOrEmpty(Client.Config.LoadingGif))
                    loading = await Context.Channel.SendFileAsync(Client.Config.LoadingGif);

                var guild = Client.Guilds.First(g => g.Id == Context.Guild.Id);
                guild.Language = shortName;
                var path =
                    $"{Client.Config.RootDirectory}{Path.DirectorySeparatorChar}{Client.Config.GuildsDirectoryName}{(Client.Config.GuildsDirectoryName == "" ? "" : Path.DirectorySeparatorChar.ToString())}{guild.Id}.json";
                //Write the new guild to a file.
                guild.OverwriteTo(path);
                if (loading != null) await loading.DeleteAsync();
                //Send an embed with the new language.
                var newLocale = ILocale.Find(shortName);
                await Context.Channel.SendMessageAsync(null, false,
                    EmbedHelper.BuildSuccess(newLocale, newLocale.LanguageChanged));
            }
        }
    }
}