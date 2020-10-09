namespace Diswords.Locales
{
    /// <summary>
    ///     Russian locale of the bot.
    /// </summary>
    /// <inheritdoc cref="ILocale" />
    public class RussianLocale : ILocale
    {
        public string Processing { get; } = "{0}, Обрабатываю твоё предложение..";
        public string GamesPlayed { get; } = "Игр Сыграно";
        public string Prefix { get; } = "Префикс";
        public string DefaultLanguage { get; } = "Основной язык";
        public string GuildInfo { get; } = "Информация о сервере";
        public string WrongEndLetter { get; } = "Твоё слово не заканчивается на букву!";
        public string DoneProcessing { get; } = "{0}, Готово!";
        public string InvalidLanguage { get; } = "Язык {0} не был найден!";

        public string Help { get; } =
            "`{0}команды` - показать это меню! :)\n`{0}создать`* - создать новую игру **в этом** канале.\n`{0}создатьновую`* - **создать новый канал** и начать игру там.\n`{0}стоп` - остановить текущую игру.\n`{0}префикс`* - задать новый префикс сервера.\n`{0}язык`* - если никакие аргументы не были введены, показать список доступных языков, иначе изменить основной язык игр на этом сервере.\n`{0}предложить` - предложить новое слово, которое должно быть добавлено в базу данных.\n* - требует право `Администратор`.";

        public string NewChannelWarning { get; } =
            "Я советую использовать {0}{1}, потому что я заблокирую обычный ввод здесь.";

        public string Name { get; } = "ru";

        public string JoinedGuild { get; } =
            "Привет! :wave:\nСпасибо, что пригласили меня!\nДайте мне секунду, пока я настрою сервер..";

        public string SetupDone { get; } = "Готово! :smile:";
        public string NotEnoughPermissions { get; } = "У тебя не хватает прав для использования этой команды!";
        public string PleaseWait { get; } = "Пожалуйста подождите..";
        public string GameCreated { get; } = "Игра была успешно создана!\nЯ начну..\n`{0}`";
        public string WordAlreadyExists { get; } = "База данных уже содержит это слово!";
        public string WrongWord { get; } = "Твоё слово не начинается на `{0}`!";

        public string NotAWord { get; } =
            "Хм.. Это  не похоже на слово.";

        public string HowToSuggest { get; } = "Если ты уверен(а) что это слово, пожалуйста используй `{0}{1} {2} {3}`";

        public string WordNotFound { get; } =
            "Это слово не найдено в моей базе данных.. Пожалуйста используй `{0}{1} {2} {3}` чтобы предложить его.";

        public string TooManyWords { get; } = "У тебя должно быть одно слово в сообщении, у тебя их {0}!";

        public string SuccessfullySuggested { get; } =
            "{0}, Твоё слово было успешно предложено!\nТвой номер в очереди: **{1}**";

        public string SuggestCommand { get; } = "предложить";
        public string Continuing { get; } = "Продолжаем! Следующая буква: `{0}`";
        public string InvalidUser { get; } = "Ты уже отправил слово! Кто-то другой должен ответить на него.";
        public string AlreadyUsedWord { get; } = "Это слово уже было использовано!";
        public string Error { get; } = "Ошибка";
        public string Warning { get; } = "Предупреждение";
        public string Success { get; } = "Успех";
        public string GameDeleted { get; } = "Игра будет удалена через 10 секунд.\nСпасибо за игру! :smile:";
        public string CreateNew { get; } = "создатьновую";
        public string Languages { get; } = "Языки";
        public string LanguageChanged { get; } = "Язык сервера был успешно изменён!";
        public string PrefixChanged { get; } = "Префикс сервера был успешно изменён!";
        public string Commands { get; } = "Комманды";
    }
}