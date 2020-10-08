namespace Diswords.Locales
{
    public class RussianLocale : ILocale
    {
        public string Name { get; } = "ru";

        public string JoinedGuild { get; } =
            "Привет! :wave:\nСпасибо, что пригласили меня!\nДайте мне секунду, пока я настрою сервер..";

        public string SetupDone { get; } = "Готово! :smile:";
        public string NotEnoughPermissions { get; }
        public string PleaseWait { get; } = "Пожалуйста подождите..";
        public string GameCreated { get; } = "Игра была успешно создана!\nЯ начну..\n`{0}`";
        public string WrongWord { get; } = "Твоё слово не начинается на `{0}`!";

        public string NotAWord { get; } =
            "Хм.. Это  не похоже на слово.\nЕсли ты уверен(а) что это слово, пожалуйста используй `{0}{1} {2}`";

        public string WordNotFound { get; } =
            "Это слово не найдено в моей базе данных.. Пожалуйста используй `{0}{1} {2}` чтобы предложить его.";

        public string TooManyWords { get; } = "У тебя должно быть одно слово в сообщении, у тебя их {0}!";

        public string SuggestCommand { get; } = "предложить";
        public string Continuing { get; } = "Продолжаем! Следующая буква: `{0}`";
        public string InvalidUser { get; } = "Ты уже отправил слово! Кто-то другой должен ответить на него.";
        public string AlreadyUsedWord { get; } = "Это слово уже было использовано!";
    }
}