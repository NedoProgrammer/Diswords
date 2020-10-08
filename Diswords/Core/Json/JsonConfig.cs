using System.IO;

namespace Diswords.Core.Json
{
    public class JsonConfig : JsonSerializable<JsonConfig>
    {
        //The constructor
        public JsonConfig(string defaultLanguage = "en", string defaultPrefix = "dw.", string rootDirectory = "",
            string guildsDirectoryName = "", string languagesDirectoryName = "")
        {
            DefaultLanguage = defaultLanguage;
            DefaultPrefix = defaultPrefix;
            RootDirectory = rootDirectory == "" ? Directory.GetCurrentDirectory() : rootDirectory;
            GuildsDirectoryName = guildsDirectoryName;
            LanguagesDirectoryName = languagesDirectoryName;
        }

        //The root directory with all the contenents and etc.
        public string RootDirectory { get; }

        //The directory where the guilds will be stored.
        //Should be located in root directory
        public string GuildsDirectoryName { get; }

        //The directory where all languages will be stored.
        //Should be located in the root directory
        public string LanguagesDirectoryName { get; }

        //The default language, which the bot will set when joined the server
        public string DefaultLanguage { get; }

        //Default prefix of the server.
        public string DefaultPrefix { get; }
    }
}