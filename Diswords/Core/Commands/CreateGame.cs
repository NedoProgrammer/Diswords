using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Diswords.Core.Helpers;

namespace Diswords.Core.Commands
{
    /// <summary>
    ///     A game command that creates a new game.
    /// </summary>
    public class CreateGame : AdvancedContext
    {
        /// <summary>
        ///     Discord.NET method...
        /// </summary>
        /// <returns>nothing</returns>
        /// <exception cref="Exception">The language (somehow) was not found.</exception>
        [Command("create")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [Alias("создать")]
        public async Task Create()
        {
            await Context.Channel.SendMessageAsync(Locale.PleaseWait);
            //I consider this a warning because I wouldn't recommend creating a game in general, for example.
            await Context.Channel.SendMessageAsync(null, false,
                EmbedHelper.BuildWarning(Locale,
                    string.Format(Locale.NewChannelWarning, Client.Guilds.First(g => g.Id == Context.Guild.Id).Prefix,
                        Locale.CreateNew)));
            if (Client.Languages.All(x => x.ShortName != Locale.Name))
                throw new Exception($"Diswords: Language {Locale.Name} was not found.");
            //Change the slow mode interval (delay between sending messages).
            await ((SocketTextChannel) Context.Channel).ModifyAsync(c =>
            {
                c.SlowModeInterval = 10;
            });
            //Create the game.
            var game = new Game.Diswords(Client, Context.Message.Author as IGuildUser,
                Client.Guilds.First(g => g.Id == Context.Guild.Id), Context.Channel as SocketTextChannel, false, ((SocketTextChannel) Context.Channel).SlowModeInterval,
                Client.Languages.First(x => x.ShortName == Locale.Name));
            var word = game.Language.Words[new Random((int) DateTime.Now.Ticks).Next(game.Language.Words.Count)];
            //The bot starts the game, so it sets the word first.
            game.SetWord(word, Client.Client.CurrentUser);
            Client.Games.Add(game);
            await Context.Channel.SendMessageAsync(null, false,
                EmbedHelper.BuildSuccess(Locale, string.Format(Locale.GameCreated, word)));
        }

        /// <summary>
        ///     Discord.NET method...
        ///     <remarks>This command creates a new channel and starts the game there.</remarks>
        /// </summary>
        /// <returns>nothing</returns>
        /// <exception cref="Exception">The language (somehow) was not found.</exception>
        [Command("createnew")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageChannels)]
        [Alias("создатьновую")]
        public async Task CreateNew()
        {
            await Context.Channel.SendMessageAsync(Locale.PleaseWait);
            if (Client.Languages.All(x => x.ShortName != Locale.Name))
                throw new Exception($"Diswords: Language {Locale.Name} was not found.");
            var channel = await Context.Guild.CreateTextChannelAsync("diswords-" +
                                                                     (Context.Guild.TextChannels.Count(c =>
                                                                         c.Name.Contains("diswords")) + 1));
            //Change the slow mode interval (delay between sending messages).
            await channel.ModifyAsync(c => c.SlowModeInterval = 10);
            //Create the game.
            var game = new Game.Diswords(Client, Context.Message.Author as IGuildUser,
                Client.Guilds.First(g => g.Id == Context.Guild.Id), channel, true, 0,
                Client.Languages.First(x => x.ShortName == Locale.Name));
            var word = game.Language.Words[new Random((int) DateTime.Now.Ticks).Next(game.Language.Words.Count)];
            //The bot starts the game, so it sets the word first.
            game.SetWord(word, Client.Client.CurrentUser);
            Client.Games.Add(game);
            await channel.SendMessageAsync(null, false,
                EmbedHelper.BuildSuccess(Locale, string.Format(Locale.GameCreated, word)));
            await Context.Channel.SendMessageAsync(null, false,
                EmbedHelper.BuildSuccess(Locale, $"\n{channel.Mention}"));
        }

        /// <summary>
        ///     Discord.NET method...
        ///     <remarks>It creates the game with the written by the administrator language.</remarks>
        /// </summary>
        /// <returns>nothing</returns>
        /// <exception cref="Exception">The language (somehow) was not found.</exception>
        [Command("create")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [Alias("создать")]
        public async Task Create(string language)
        {
            await Context.Channel.SendMessageAsync(Locale.PleaseWait);
            //I consider this a warning because I wouldn't recommend creating a game in general, for example.
            await Context.Channel.SendMessageAsync(null, false,
                EmbedHelper.BuildWarning(Locale,
                    string.Format(Locale.NewChannelWarning, Client.Guilds.First(g => g.Id == Context.Guild.Id).Prefix,
                        Locale.CreateNew)));
            if (Client.Languages.All(x => x.ShortName != Locale.Name))
                throw new Exception($"Diswords: Language {Locale.Name} was not found.");
            //Change the slow mode interval (delay between sending messages).
            await ((SocketTextChannel) Context.Channel).ModifyAsync(c =>
            {
                c.SlowModeInterval = 10;
            });
            //Create the game.
            var game = new Game.Diswords(Client, Context.Message.Author as IGuildUser,
                Client.Guilds.First(g => g.Id == Context.Guild.Id), Context.Channel as SocketTextChannel, false, ((SocketTextChannel) Context.Channel).SlowModeInterval,
                Client.Languages.First(x => x.ShortName == language));
            var word = game.Language.Words[new Random((int) DateTime.Now.Ticks).Next(game.Language.Words.Count)];
            //The bot starts the game, so it sets the word first.
            game.SetWord(word, Client.Client.CurrentUser);
            Client.Games.Add(game);
            await Context.Channel.SendMessageAsync(null, false,
                EmbedHelper.BuildSuccess(Locale, string.Format(Locale.GameCreated, word)));
        }

        /// <summary>
        ///     Discord.NET method...
        ///     <remarks>It creates the game with the written by the administrator language.</remarks>
        /// </summary>
        /// <returns>nothing</returns>
        /// <exception cref="Exception">The language (somehow) was not found.</exception>
        [Command("createnew")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.SendMessages)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireBotPermission(ChannelPermission.ManageChannels)]
        [Alias("создатьновую")]
        public async Task CreateNew(string language)
        {
            await Context.Channel.SendMessageAsync(Locale.PleaseWait);
            if (Client.Languages.All(x => x.ShortName != Locale.Name))
                throw new Exception($"Diswords: Language {Locale.Name} was not found.");
            var channel = await Context.Guild.CreateTextChannelAsync("diswords-" +
                                                                     (Context.Guild.TextChannels.Count(c =>
                                                                         c.Name.Contains("diswords")) + 1));
            //Change the slow mode interval (delay between sending messages).
            await channel.ModifyAsync(c => c.SlowModeInterval = 10);
            //Create the game.
            var game = new Game.Diswords(Client, Context.Message.Author as IGuildUser,
                Client.Guilds.First(g => g.Id == Context.Guild.Id), channel, true, 0,
                Client.Languages.First(x => x.ShortName == language));
            var word = game.Language.Words[new Random((int) DateTime.Now.Ticks).Next(game.Language.Words.Count)];
            //The bot starts the game, so it sets the word first.
            game.SetWord(word, Client.Client.CurrentUser);
            Client.Games.Add(game);
            await channel.SendMessageAsync(null, false,
                EmbedHelper.BuildSuccess(Locale, string.Format(Locale.GameCreated, word)));
            await Context.Channel.SendMessageAsync(null, false,
                EmbedHelper.BuildSuccess(Locale, $"\n{channel.Mention}"));
        }
    }
}