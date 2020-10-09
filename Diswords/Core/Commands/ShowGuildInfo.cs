using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Diswords.Core.Helpers;

namespace Diswords.Core.Commands
{
    /// <summary>
    ///     Show the additional information about the server.
    /// </summary>
    public class ShowGuildInfo : AdvancedContext
    {
        /// <summary>
        ///     Discord.NET method
        /// </summary>
        /// <returns>nothing</returns>
        [Command("guild")]
        [Alias("сервер")]
        public async Task ShowInfo()
        {
            var guild = Client.Guilds.First(g => g.Id == Context.Guild.Id);
            var language = Client.Languages.First(l => l.ShortName == guild.Language);
            //There are three field that are sent:
            // - How many games did that server play
            // - What is the default language of the server
            // - The prefix of the server (idk why but why not)
            var fields = new List<EmbedFieldBuilder>
            {
                new EmbedFieldBuilder {IsInline = true, Name = Locale.GamesPlayed, Value = guild.GamesPlayed},
                new EmbedFieldBuilder
                {
                    IsInline = true,
                    Name = Locale.DefaultLanguage,
                    Value = $"{language.FlagEmoji.Name} {language.Name}"
                },
                new EmbedFieldBuilder {IsInline = true, Name = Locale.Prefix, Value = guild.Prefix}
            };
            var embed = new EmbedBuilder().WithColor(Color.DarkGreen).WithTitle(Locale.GuildInfo).WithFields(fields);
            await Context.Channel.SendMessageAsync(null, false, embed.Build());
        }
    }
}