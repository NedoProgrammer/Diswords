using System;
using System.Collections.Generic;
using Discord;

namespace Diswords.Core.Json
{
    /// <summary>
    ///     A class that will represent a language for the bot.
    ///     <para>This class makes it easy to add new languages and managing it later in the game.</para>
    /// </summary>
    public class JsonLanguage : JsonSerializable<JsonLanguage>
    {
        /// <summary>
        ///     The emoji of the flag of the language.
        ///     <example>\uD83C\uDDEC</example>
        /// </summary>
        /// 🇬🇧
        [NonSerialized] public Emoji FlagEmoji;

        /// <summary>
        ///     The characters on which words cannot start.
        ///     <para>For example, in Russian, 'ъ' is forbidden.</para>
        /// </summary>
        public List<char> ForbiddenCharacters;

        /// <summary>
        ///     The name of the language.
        ///     <example>"English"</example>
        /// </summary>
        public string Name;

        /// <summary>
        ///     Short name of that language.
        ///     <example>"en"</example>
        /// </summary>
        public string ShortName;

        /// <summary>
        ///     The words available for the language.
        /// </summary>
        public List<string> Words;

        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="name">Name of the language.</param>
        /// <param name="emoji">The emoji (flag) that represents this language.</param>
        /// <param name="shortName">Short name of that language.</param>
        /// <param name="forbiddenCharacters">The characters on which words cannot start.</param>
        /// <param name="words">A list of words that are usable.</param>
        public JsonLanguage(string name, string emoji, string shortName, List<char> forbiddenCharacters,
            List<string> words)
        {
            FlagEmoji = new Emoji(emoji);
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
            ForbiddenCharacters = forbiddenCharacters ?? throw new ArgumentNullException(nameof(forbiddenCharacters));
            Words = words ?? throw new ArgumentNullException(nameof(words));
        }
    }
}