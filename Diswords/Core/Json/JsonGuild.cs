namespace Diswords.Core.Json
{
    public class JsonGuild : JsonSerializable<JsonGuild>
    {
        //The amount of games played.
        public int GamesPlayed;

        //The ID of the server.
        public ulong Id;

        //The short name for the language.
        //i.e. "en"
        public string Language;

        //The prefix of the server.
        //i.e. "." or "!"
        public string Prefix;

        //The Constructor.
        public JsonGuild(string language, string prefix, ulong id, int gamesPlayed = default)
        {
            Language = language;
            GamesPlayed = gamesPlayed;
            Prefix = prefix;
            Id = id;
        }
    }
}