using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Diswords.Core.Helpers;
using Diswords.Core.Json;
using Diswords.Locales;
using StringComparison = Diswords.Core.Helpers.StringComparison;

namespace Diswords.Game
{
    /// <summary>
    ///     A game class.
    ///     Handles every input, everything. :D
    /// </summary>
    public class Diswords
    {
        /// <summary>
        ///     A diswords client (contains the discord client as well)
        ///     <para>Will be used to add this game to the client's list</para>
        /// </summary>
        private readonly DiswordsClient _client;

        /// <summary>
        ///     Was a new channel created just for this game?
        /// </summary>
        private readonly bool _newChannelCreated;

        /// <summary>
        ///     The last (valid) letter of the word that was last sent.
        ///     <para>See <see cref="_getValidWord" /> to see what a "valid" word is.</para>
        /// </summary>
        private char _lastLetter = ' ';

        /// <summary>
        ///     The last user who sent a correct word.
        /// </summary>
        private IUser _lastSender;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="client">Diswords Client :D</param>
        /// <param name="creator">User who sent the create game command.</param>
        /// <param name="guild">Guild (Server) where the game will be played.</param>
        /// <param name="channel">Channel in what the game will be played.</param>
        /// <param name="createdNewChannel">Was a new channel created just for this game?</param>
        /// <param name="language">Language of the game.</param>
        public Diswords(DiswordsClient client, IGuildUser creator, JsonGuild guild, ITextChannel channel,
            bool createdNewChannel, JsonLanguage language)
        {
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            Channel = channel ?? throw new ArgumentNullException(nameof(channel));
            Language = language ?? throw new ArgumentNullException(nameof(language));
            Locale = ILocale.Find(Language.ShortName);
            Guild = guild;
            _newChannelCreated = createdNewChannel;
            _client = client;
        }

        /// <summary>
        ///     The creator of the game.
        /// </summary>
        public IGuildUser Creator { get; }

        /// <summary>
        ///     List of words that users can't say, because they were repeated.
        /// </summary>
        private List<string> UsedWords { get; } = new List<string>();

        /// <summary>
        ///     The channel where the is will be played.
        /// </summary>
        public ITextChannel Channel { get; }

        /// <summary>
        ///     The language of the game.
        /// </summary>
        public JsonLanguage Language { get; }

        /// <summary>
        ///     The locale of the game.
        /// </summary>
        public ILocale Locale { get; }

        /// <summary>
        ///     The guild (server) where the game will be played
        /// </summary>
        public JsonGuild Guild { get; }

        /// <summary>
        ///     Stop the game.
        /// </summary>
        public async void Stop()
        {
            Guild.GamesPlayed++;
            Guild.OverwriteTo(
                $"{_client.Config.RootDirectory}{Path.DirectorySeparatorChar}{_client.Config.GuildsDirectoryName}{(_client.Config.GuildsDirectoryName == "" ? "" : Path.DirectorySeparatorChar.ToString())}{Guild.Id}.json");
            switch (_newChannelCreated)
            {
                case true:
                {
                    await Channel.SendMessageAsync(null, false, EmbedHelper.BuildSuccess(Locale, Locale.GameDeleted));
                    await Task.Delay(10000);
                    await Channel.DeleteAsync();
                    _client.Games.Remove(this);
                    break;
                }
                case false:
                {
                    await Channel.SendMessageAsync(null, false, EmbedHelper.BuildSuccess(Locale, Locale.GameDeleted));
                    await Task.Delay(10000);
                    _client.Games.Remove(this);
                    break;
                }
            }
        }

        /// <summary>
        ///     Set the new last word & user
        /// </summary>
        /// <param name="word">The word sent.</param>
        /// <param name="sender">The sender of that word.</param>
        public void SetWord(string word, IUser sender)
        {
            _lastSender = sender;
            UsedWords.Add(word.ToLower());
            word = _getValidWord(word);
            _lastLetter = word.Last();
        }

        /// <summary>
        ///     Remove all forbidden characters from a word from a language.
        ///     <para>Basically, it just replaces all forbidden characters (for example in English - x) to "".</para>
        /// </summary>
        /// <param name="word">The initial word that needs to be changed.</param>
        /// <returns>New word, that doesn't have the forbidden characters.</returns>
        private string _getValidWord(string word)
        {
            return Language.ForbiddenCharacters.Aggregate(word.ToLower(),
                (current, invalidChar) => current.Replace(invalidChar.ToString().ToLower(), ""));
        }

        /// <summary>
        ///     Check if the input from user was valid.
        /// </summary>
        /// <param name="input">The string that the user sent</param>
        /// <param name="sender">The user that sent the message.</param>
        /// <returns>The Check Result. See <see cref="InputCheckResult" /></returns>
        private InputCheckResult _isValidInput(string input, IUser sender)
        {
            if (sender.IsBot) return InputCheckResult.IsBot;
            if (input.StartsWith("//")) return InputCheckResult.IsComment;
            if (input.StartsWith(">")) return InputCheckResult.IsQuote;
            if (input.StartsWith("<:")) return InputCheckResult.IsEmoji;
            if (_lastSender.Id == sender.Id) return InputCheckResult.SameUser;
            if (UsedWords.Contains(input.ToLower())) return InputCheckResult.RepeatedWord;
            var splitWords = input.Split(" ");
            if (splitWords.Length > 1) return InputCheckResult.TooManyWords;
            if (!input.ToLower().StartsWith(char.ToLower(_lastLetter))) return InputCheckResult.WrongLetter;
            var words = Language.Words.Where(w => w.ToLower().StartsWith(char.ToLower(_lastLetter))).ToList();
            var results = words.Select(w => StringComparison.LevenshteinDistance(input, w)).ToList();
            if (results.Max() < 0.5f)
                return InputCheckResult.Gibberish;
            if (results.Max() > 0.5f && !words.Contains(input.ToLower()))
                return InputCheckResult.NotFound;
            return InputCheckResult.Success;
        }

        /// <summary>
        ///     Handle the input from the user.
        ///     <para>Called each time a bot receives a message and it doesn't have the prefix</para>
        /// </summary>
        /// <param name="word">The word that needs to be checked.</param>
        /// <param name="sender">The sender of that word.</param>
        public async void HandleInput(string word, IUser sender)
        {
            switch (_isValidInput(word, sender))
            {
                case InputCheckResult.Success:
                    SetWord(word, sender);
                    var message =
                        await Channel.SendMessageAsync(null, false,
                            EmbedHelper.BuildSuccess(Locale,
                                string.Format(Locale.Continuing, _getValidWord(word).Last())));
                    await Task.Delay(5000);
                    await message.DeleteAsync();
                    break;
                case InputCheckResult.IsBot:
                    return;
                case InputCheckResult.IsComment:
                    return;
                case InputCheckResult.IsQuote:
                    return;
                case InputCheckResult.IsEmoji:
                    return;
                case InputCheckResult.SameUser:
                    await Channel.SendMessageAsync(null, false, EmbedHelper.BuildError(Locale, Locale.InvalidUser));
                    break;
                case InputCheckResult.RepeatedWord:
                    await Channel.SendMessageAsync(null, false, EmbedHelper.BuildError(Locale, Locale.AlreadyUsedWord));
                    break;
                case InputCheckResult.TooManyWords:
                    await Channel.SendMessageAsync(null, false,
                        EmbedHelper.BuildError(Locale, string.Format(Locale.TooManyWords, word.Split(" ").Length)));
                    break;
                case InputCheckResult.WrongLetter:
                    await Channel.SendMessageAsync(null, false,
                        EmbedHelper.BuildError(Locale, string.Format(Locale.WrongWord, _lastLetter)));
                    break;
                case InputCheckResult.Gibberish:
                    await Channel.SendMessageAsync(null, false,
                        EmbedHelper.BuildError(Locale, Locale.NotAWord + "\n" +
                                                       string.Format(Locale.HowToSuggest, Guild.Prefix,
                                                           Locale.SuggestCommand, Language.ShortName, word)));
                    break;
                case InputCheckResult.NotFound:
                    await Channel.SendMessageAsync(null, false, EmbedHelper.BuildError(Locale,
                        string.Format(Locale.WordNotFound, Guild.Prefix, Locale.SuggestCommand, Language.ShortName,
                            word)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum InputCheckResult
        {
            Success,
            IsBot,
            IsComment,
            IsQuote,
            IsEmoji,
            SameUser,
            RepeatedWord,
            TooManyWords,
            WrongLetter,
            Gibberish,
            NotFound
        }
    }
}