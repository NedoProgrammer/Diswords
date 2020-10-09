#nullable enable
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Rest;
using Diswords.Core.Helpers;
using Diswords.Core.Json;

namespace Diswords.Core.Commands
{
    public class SuggestWord : AdvancedContext
    {
        /// <summary>
        ///     List of suggestions created and needed to be checked.
        /// </summary>
        private readonly List<Suggestion> _suggestions = new List<Suggestion>();

        /// <summary>
        ///     Discord.NET method..
        ///     <remarks>This is a pretty long-executing method, so it's RunMode.Async</remarks>
        /// </summary>
        /// <param name="language">Language of the suggestion.</param>
        /// <param name="word">The word..</param>
        /// <returns>nothing..</returns>
        [Command("suggest", RunMode = RunMode.Async)]
        [Alias("предложить")]
        public async Task Suggest(string language, string word)
        {
            //Send a loading GIF if the file for it exists.
            RestUserMessage? loading = null;
            if (!string.IsNullOrEmpty(Client.Config.LoadingGif))
                loading = await Context.Channel.SendFileAsync(
                    Client.Config.LoadingGif);
            //Find the language from the user in the language list.
            var lang = Client.Languages.FirstOrDefault(l => l.ShortName == language);
            if (lang == null)
            {
                //Failed if the language from user was not found.
                await Context.Channel.SendMessageAsync(null, false,
                    EmbedHelper.BuildError(Locale, string.Format(Locale.InvalidLanguage, language)));
                if (loading != null) await loading.DeleteAsync();
                return;
            }

            //Create a suggestion.
            var suggested = await Context.Channel.SendMessageAsync(null, false,
                EmbedHelper.BuildSuccess(Locale,
                    string.Format(Locale.SuccessfullySuggested, Context.User.Mention, _suggestions.Count + 1)));
            _suggestions.Add(new Suggestion(Context.User.Id, suggested, language, word));
            if (_suggestions.Count == 1)
                //If the list before was empty, start processing suggestions.
                ProcessSuggestions();
            if (loading != null) await loading.DeleteAsync();
        }

        /// <summary>
        ///     Method that will check the submissions and add them to the database.
        /// </summary>
        private async void ProcessSuggestions()
        {
            //Words that are going to be added to the database.
            var addedWords = new List<Suggestion>();
            //While there are suggestions
            while (_suggestions.Count != 0)
            {
                var suggestion = _suggestions.First();
                var language = Client.Languages.First(l => l.ShortName == suggestion.Language);
                //Word doesn't end with a letter.
                if (!char.IsLetter(suggestion.Word.Last()))
                {
                    //Decline the suggestion.
                    await suggestion.Message.ModifyAsync(m => m.Embed =
                        EmbedHelper.BuildError(Locale,
                            Locale.WrongEndLetter));
                    _suggestions.Remove(_suggestions.First(s => s.Author == suggestion.Author));
                    continue;
                }

                //Word already exists in the database.
                if (language.Words.Contains(suggestion.Word.ToLower()))
                {
                    //Decline the suggestion.
                    await suggestion.Message.ModifyAsync(m => m.Embed =
                        EmbedHelper.BuildError(Locale,
                            Locale.WordAlreadyExists));
                    _suggestions.Remove(_suggestions.First(s => s.Author == suggestion.Author));
                    continue;
                }

                //Check if the word is gibberish.
                var results = language.Words.Where(w => w.ToLower().StartsWith(char.ToLower(suggestion.Word.First())))
                    .ToList().Select(w => StringComparison.LevenshteinDistance(suggestion.Word, w)).ToList();
                //This is going to be really close-checking, since 
                //this system is automated and 0 people are checking it.
                if (!results.Any() || results.Max() < 0.3f)
                {
                    //Word looks like gibberish, decline it.
                    await suggestion.Message.ModifyAsync(m => m.Embed =
                        EmbedHelper.BuildError(Locale,
                            Locale.NotAWord));
                    _suggestions.Remove(_suggestions.First(s => s.Author == suggestion.Author));
                    continue;
                }

                //Accept the word. Yay :D
                addedWords.Add(suggestion);
                await suggestion.Message.ModifyAsync(m => m.Embed = EmbedHelper.BuildSuccess(Locale,
                    string.Format(Locale.DoneProcessing, Client.Client.GetUser(suggestion.Author).Mention)));
                _suggestions.Remove(_suggestions.First(s => s.Author == suggestion.Author));
            }

            //Update the database.
            UpdateDatabase(addedWords);
        }

        /// <summary>
        ///     Update the database with new words.
        /// </summary>
        /// <param name="addedWords">Words needed to be added</param>
        private async void UpdateDatabase(IEnumerable<Suggestion> addedWords)
        {
            foreach (var group in addedWords.GroupBy(j => j.Word))
            {
                JsonLanguage language = null!;
                foreach (var suggestion in group)
                {
                    language = Client.Languages.First(l => l.ShortName == suggestion.Language);
                    language.Words.Add(suggestion.Word.ToLower());
                    language = DiswordsClient.StaticLanguages.First(l => l.ShortName == suggestion.Language);
                    language.Words.Add(suggestion.Word.ToLower());
                }

                await File.WriteAllTextAsync(
                    $"{Client.Config.RootDirectory}{Path.DirectorySeparatorChar}{Client.Config.LanguagesDirectoryName}{(string.IsNullOrEmpty(Client.Config.LanguagesDirectoryName) ? "" : "{Path.PathSeparator}")}{language.ShortName}.json",
                    language.ToJson());
            }
        }

        /// <summary>
        ///     Class representing basic information about the user suggestion.
        ///     <para>Contains:</para>
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Author</description>
        ///         </item>
        ///         <item>
        ///             <description>Language</description>
        ///         </item>
        ///         <item>
        ///             <description>Word</description>
        ///         </item>
        ///     </list>
        /// </summary>
        private class Suggestion
        {
            /// <summary>
            ///     The constructor.
            /// </summary>
            /// <param name="author">User ID of the suggestion creator.</param>
            /// <param name="message">Discord Embed with suggestion state.</param>
            /// <param name="language">Language of the suggestion.</param>
            /// <param name="word">The word suggested.</param>
            public Suggestion(ulong author, RestUserMessage message, string language, string word)
            {
                Author = author;
                Message = message;
                Language = language;
                Word = word;
            }

            /// <summary>
            ///     Author of the suggestion.
            /// </summary>
            public ulong Author { get; }

            /// <summary>
            ///     Discord Message with the suggestion state.
            /// </summary>
            public RestUserMessage Message { get; }

            /// <summary>
            ///     Language of the word suggested
            /// </summary>
            public string Language { get; }

            /// <summary>
            ///     The Word :D
            /// </summary>
            public string Word { get; }
        }
    }
}