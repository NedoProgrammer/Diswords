namespace Diswords.Locales
{
    public class EnglishLocale : ILocale
    {
        public string Name { get; } = "en";

        public string JoinedGuild { get; } =
            "Hi! :wave:\nThanks for inviting me!\nGive me one second while I'll prepare the server..";

        public string SetupDone { get; } = "Done! :smile:";
        public string NotEnoughPermissions { get; }
        public string PleaseWait { get; } = "Please wait..";
        public string GameCreated { get; } = "The game was successfully created!\nI'll start..\n`{0}`";
        public string WrongWord { get; } = "Your word doesn't start at `{0}`!";

        public string NotAWord { get; } =
            "Hm..That doesn't look like a word.\nIf you are sure that it is, please use `{0}.{1} {2}`";

        public string WordNotFound { get; } =
            "That word wasn't found in my database.. Please use `{0}.{1} {2}` to suggest it.";

        public string TooManyWords { get; } = "You should have 1 word per message, but you have {0}!";
        public string SuggestCommand { get; } = "suggest";
        public string Continuing { get; } = "Continuing! Next letter: `{0}`";
        public string InvalidUser { get; } = "You already sent a word! Somebody else should answer to it.";
        public string AlreadyUsedWord { get; } = "That word was already used!";
    }
}