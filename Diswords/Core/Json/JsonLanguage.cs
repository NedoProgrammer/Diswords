using System;
using System.Collections.Generic;
using Discord;

namespace Diswords.Core.Json
{
    //A class that will represent a language for the bot.
    //This class makes it easy to add new languages and managing it later in the game.
    public class JsonLanguage : JsonSerializable<JsonLanguage>
    {
        //The emoji of the flag of the language 
        //i.e. \uD83C\uDDEC or 🇬🇧
        public Emoji FlagEmoji;

        //The characters on which words cannot start.
        //For example, in Russian, 'ъ' is forbidden.
        public List<char> ForbiddenCharacters;

        //The name of the language. 
        //i.e. "English"
        public string Name;

        //Short name for that language.
        //i.e. "en"
        public string ShortName;

        //The words available for the language
        public List<string> Words;

        public JsonLanguage(string name, string shortName, Emoji flagEmoji, List<char> forbiddenCharacters,
            List<string> words)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
            FlagEmoji = flagEmoji ?? throw new ArgumentNullException(nameof(flagEmoji));
            ForbiddenCharacters = forbiddenCharacters ?? throw new ArgumentNullException(nameof(forbiddenCharacters));
            Words = words ?? throw new ArgumentNullException(nameof(words));
        }
    }
}