using System.Collections.Generic;
using System.Linq;

namespace Diswords.Locales
{
    /// <summary>
    ///     Class containing all messages that the bot will use.
    ///     <para>Used for multi-language support.</para>
    /// </summary>
    public interface ILocale
    {
        public static readonly List<ILocale> Locales = new List<ILocale> {new RussianLocale(), new EnglishLocale()};
        public string InvalidLanguage { get; }
        public string Help { get; }
        public string NewChannelWarning { get; }
        public string Name { get; }
        public string JoinedGuild { get; }
        public string SetupDone { get; }
        public string NotEnoughPermissions { get; }
        public string PleaseWait { get; }
        public string GameCreated { get; }
        public string WordAlreadyExists { get; }
        public string WrongWord { get; }
        public string NotAWord { get; }

        public string HowToSuggest { get; }
        public string WordNotFound { get; }
        public string TooManyWords { get; }
        public string SuggestCommand { get; }
        public string Continuing { get; }
        public string InvalidUser { get; }

        public string AlreadyUsedWord { get; }
        public string SuccessfullySuggested { get; }
        public string Error { get; }
        public string Warning { get; }
        public string Success { get; }
        public string CreateNew { get; }
        public string GamesPlayed { get; }
        public string Prefix { get; }
        public string DefaultLanguage { get; }
        public string GameDeleted { get; }
        public string Languages { get; }
        public string LanguageChanged { get; }
        public string PrefixChanged { get; }
        public string Commands { get; }
        public string GuildInfo { get; }
        public string WrongEndLetter { get; }
        public string DoneProcessing { get; }
        public string Rules { get; }
        public string OpenSource { get; }

        public static ILocale Find(string shortName)
        {
            return Locales.FirstOrDefault(x => x.Name == shortName);
        }
    }
}