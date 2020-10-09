namespace Diswords.Locales
{
    /// <summary>
    ///     English locale of the bot.
    /// </summary>
    /// <inheritdoc cref="ILocale" />
    public class EnglishLocale : ILocale
    {
        public string Processing { get; } = "{0}, Processing your suggestion..";
        public string InvalidLanguage { get; } = "Language `{0}` was not found!";

        public string Help { get; } =
            "`{0}commands` - show this embed! :)\n`{0}guild` - get the information about this guild.\n`{0}create`* - create a new game **in this** channel.\n`{0}createnew`* - **create a new channel** and start a game there.\n`{0}stop` - stop the current game.\n`{0}prefix`* - set the new prefix of the server.\n`{0}language`* - if no arguments are passed, displays the lists of available languages, otherwise change the default language of games for this server.\n`{0}suggest` - suggest a new word to be added to the database.\n* - requires `Administrator` permission.";

        public string NewChannelWarning { get; } =
            "I recommend using {0}{1}, because I'll block the default input in this channel!";

        public string Name { get; } = "en";

        public string JoinedGuild { get; } =
            "Hi! :wave:\nThanks for inviting me!\nGive me one second while I'll prepare the server..";

        public string SetupDone { get; } = "Done! :smile:";
        public string NotEnoughPermissions { get; } = "You don't have enough permissions to do that command!";
        public string PleaseWait { get; } = "Please wait..";
        public string GameCreated { get; } = "The game was successfully created!\nI'll start..\n`{0}`";
        public string WordAlreadyExists { get; } = "{0}, The database already has that word!";
        public string WrongWord { get; } = "Your word doesn't start at `{0}`!";

        public string NotAWord { get; } =
            "Hm..That doesn't look like a word.";

        public string HowToSuggest { get; } = "If you are sure that it is, please use `{0}{1} {2} {3}`";

        public string SuccessfullySuggested { get; } =
            "{0}, your word was successfully suggested!\nYour queue number: **{1}**";

        public string WordNotFound { get; } =
            "That word wasn't found in my database.. Please use `{0}{1} {2} {3}` to suggest it.";

        public string TooManyWords { get; } = "You should have 1 word per message, but you have {0}!";
        public string SuggestCommand { get; } = "suggest";
        public string Continuing { get; } = "Continuing! Next letter: `{0}`";
        public string InvalidUser { get; } = "You already sent a word! Somebody else should answer to it.";
        public string AlreadyUsedWord { get; } = "That word was already used!";
        public string Error { get; } = "Error";
        public string Warning { get; } = "Warning";
        public string Success { get; } = "Success";
        public string DefaultLanguage { get; } = "Default Language";
        public string GameDeleted { get; } = "The game will be deleted in 10 seconds.\nThanks for playing! :smile:";
        public string CreateNew { get; } = "createnew";
        public string GamesPlayed { get; } = "Games Played";
        public string Prefix { get; } = "Prefix";
        public string Languages { get; } = "Languages";
        public string LanguageChanged { get; } = "The language was successfully changed!";
        public string PrefixChanged { get; } = "The prefix was successfully changed!";
        public string Commands { get; } = "Commands";
        public string GuildInfo { get; } = "Guild Info";
        public string WrongEndLetter { get; } = "Your word doesn't end on a letter!";

        public string DoneProcessing { get; } =
            "{0}, Done!";
    }
}