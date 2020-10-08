using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Diswords.Core.Json;
using Diswords.Locales;
using StringComparison = Diswords.Core.StringComparison;

namespace Diswords.Game
{
    public class Diswords
    {
        private char _lastLetter = ' ';
        private IUser _lastSender;

        public Diswords(IGuildUser creator, JsonGuild guild, SocketTextChannel channel, JsonLanguage language)
        {
            Creator = creator ?? throw new ArgumentNullException(nameof(creator));
            Channel = channel ?? throw new ArgumentNullException(nameof(channel));
            Language = language ?? throw new ArgumentNullException(nameof(language));
            Locale = ILocale.Find(Language.ShortName);
            Guild = guild;
        }

        public IGuildUser Creator { get; }
        private List<string> UsedWords { get; } = new List<string>();
        public SocketTextChannel Channel { get; }
        public JsonLanguage Language { get; }
        public ILocale Locale { get; }
        public JsonGuild Guild { get; }

        public void SetWord(string word, IUser sender)
        {
            _lastSender = sender;
            UsedWords.Add(word.ToLower());
            word = _getValidWord(word);
            _lastLetter = word.Last();
        }

        private string _getValidWord(string word)
        {
            return Language.ForbiddenCharacters.Aggregate(word.ToLower(),
                (current, invalidChar) => current.Replace(invalidChar.ToString().ToLower(), ""));
        }

        public async void CheckInput(string word, IUser sender)
        {
            if (sender.IsBot) return;
            if (word.StartsWith("//")) return;
            if (word.StartsWith(">")) return;
            if (word.StartsWith(":")) return;
            if (_lastSender.Id == sender.Id)
            {
                await Channel.SendMessageAsync(Locale.InvalidUser);
                return;
            }

            if (UsedWords.Contains(word.ToLower()))
            {
                await Channel.SendMessageAsync(Locale.AlreadyUsedWord);
                return;
            }

            var splitWords = word.Split(" ");
            if (splitWords.Length > 1)
            {
                await Channel.SendMessageAsync(string.Format(Locale.TooManyWords, splitWords.Length));
                return;
            }

            if (!word.ToLower().StartsWith(char.ToLower(_lastLetter)))
            {
                await Channel.SendMessageAsync(string.Format(Locale.WrongWord, _lastLetter));
                return;
            }

            var words = Language.Words.Where(w => w.ToLower().StartsWith(char.ToLower(_lastLetter))).ToList();
            if (words.Contains(word.ToLower()))
            {
                SetWord(word, sender);
                var message =
                    await Channel.SendMessageAsync(string.Format(Locale.Continuing, _getValidWord(word).Last()));
                await Task.Delay(5000);
                await message.DeleteAsync();
                return;
            }

            var results = words.Select(w => StringComparison.LevenshteinDistance(word, w)).ToList();
            if (results.Max() < 0.5f)
                await Channel.SendMessageAsync(
                    string.Format(Locale.NotAWord, Guild.Prefix, Locale.SuggestCommand, word));
            else
                await Channel.SendMessageAsync(string.Format(Locale.WordNotFound, Guild.Prefix, Locale.SuggestCommand,
                    word));
        }
    }
}