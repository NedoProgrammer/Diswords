#nullable enable
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Diswords.Core.Helpers;
using Diswords.Locales;

namespace Diswords.Core.Commands
{
    public class SelectLanguage: AdvancedContext
    {
        [Command("language")]
        public async Task ListLanguages()
        {
            var description = Client.Languages.Aggregate("", (current, language) => current + $"\n• {language.FlagEmoji.Name} {language.Name} - `{language.ShortName}`");
            var embed = new EmbedBuilder().WithTitle(Locale.Languages).WithColor(Color.Blue).WithDescription(description).Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }
        [Command("language")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Select(string shortName)
        {
            if (Client.Languages.All(l => l.ShortName != shortName))
            {
                await Context.Channel.SendMessageAsync(null, false, EmbedHelper.BuildError(Locale, string.Format(Locale.InvalidLanguage, shortName)));
                return;
            }
            else
            {
                RestUserMessage? loading = null;
                if (!string.IsNullOrEmpty(Client.Config.LoadingGif))
                {
                    loading = await Context.Channel.SendFileAsync(Client.Config.LoadingGif);
                }

                var guild = Client.Guilds.First(g => g.Id == Context.Guild.Id);
                guild.Language = shortName;
                var path =
                    $"{Client.Config.RootDirectory}/{Client.Config.GuildsDirectoryName}{(Client.Config.GuildsDirectoryName == "" ? "" : "/")}{guild.Id}.json";
                await File.WriteAllTextAsync(path, guild.ToJson());
                if (loading != null) await loading.DeleteAsync();
                var newLocale = ILocale.Find(shortName);
                await Context.Channel.SendMessageAsync(null, false, EmbedHelper.BuildSuccess(newLocale, newLocale.LanguageChanged));
            }
        }   
    }
}