namespace Diswords.Core.Json
{
    /// <summary>
    ///     A json guild that will contain information about:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>the prefix</description>
    ///         </item>
    ///         <item>
    ///             <description>the language</description>
    ///         </item>
    ///         <item>
    ///             <description>the amount of games played.</description>
    ///         </item>
    ///     </list>
    /// </summary>
    public class JsonGuild : JsonSerializable<JsonGuild>
    {
        /// <summary>
        ///     Amount of games played.
        /// </summary>
        public int GamesPlayed;

        /// <summary>
        ///     ID of the server.
        /// </summary>
        public ulong Id;

        /// <summary>
        ///     Short name for the main language of that guild
        ///     <example>"en"</example>
        /// </summary>
        public string Language;

        /// <summary>
        ///     Prefix of the server.
        ///     <example>"!"</example>
        ///     <remarks>Default prefix is "dw."</remarks>
        /// </summary>
        public string Prefix;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="language">Main (fallback) language of the server.</param>
        /// <param name="prefix">Prefix of the server</param>
        /// <param name="id">ID of the server.</param>
        /// <param name="gamesPlayed">The amount of games played. (usually set to 0)</param>
        public JsonGuild(string language, string prefix, ulong id, int gamesPlayed = default)
        {
            Language = language;
            GamesPlayed = gamesPlayed;
            Prefix = prefix;
            Id = id;
        }
    }
}