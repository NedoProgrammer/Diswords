using System.Collections.Generic;
using System.Linq;

namespace Diswords.Locales
{
    public interface ILocale
    {
        public static List<ILocale> Locales = new List<ILocale> {new RussianLocale(), new EnglishLocale()};
        public string Name { get; }
        public string JoinedGuild { get; }
        public string SetupDone { get; }
        public string NotEnoughPermissions { get; }
        public string PleaseWait { get; }
        public string GameCreated { get; }
        public string WrongWord { get; }
        public string NotAWord { get; }
        public string WordNotFound { get; }
        public string TooManyWords { get; }
        public string SuggestCommand { get; }
        public string Continuing { get; }
        public string InvalidUser { get; }
        public string AlreadyUsedWord { get; }

        public static ILocale Find(string shortName)
        {
            return Locales.FirstOrDefault(x => x.Name == shortName);
        }

        //TODO
    }
}