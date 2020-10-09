using System;
using System.IO;
using System.Linq;

namespace Diswords.Core.Json
{
    /// <summary>
    ///     Config for the bot. Contains:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>Token of the Discord bot</description>
    ///         </item>
    ///         <item>
    ///             <description>Default (fallback) language</description>
    ///         </item>
    ///         <item>
    ///             <description>Default (fallback) prefix</description>
    ///         </item>
    ///         <item>
    ///             <description>Root directory</description>
    ///         </item>
    ///         <item>
    ///             <description>Guilds directory</description>
    ///         </item>
    ///         <item>
    ///             <description>Languages directory</description>
    ///         </item>
    ///         <item>
    ///             <description>Loading GIF (for long commands)</description>
    ///         </item>
    ///     </list>
    /// </summary>
    public class JsonConfig : JsonSerializable<JsonConfig>
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        /// <param name="token">Token of the discord bot</param>
        /// <param name="defaultLanguage">Default (fallback) language</param>
        /// <param name="defaultPrefix">Default (fallback) prefix</param>
        /// <param name="rootDirectory">Directory which contains guilds and languages of the bot.</param>
        /// <param name="guildsDirectoryName">Name of the directory which contains the guilds. (must be in the root directory)</param>
        /// <param name="languagesDirectoryName">
        ///     Name of the directory which contains the languages. (must be in the root
        ///     directory)
        /// </param>
        /// <param name="loadingGif">
        ///     Name (and extension) of the gif which will be sent when a long command is executed. (must be
        ///     in the root directory)
        /// </param>
        /// <exception cref="Exception"></exception>
        public JsonConfig(string token, string defaultLanguage = "en", string defaultPrefix = "dw.",
            string rootDirectory = "",
            string guildsDirectoryName = "", string languagesDirectoryName = "", string loadingGif = "")
        {
            Token = token;
            DefaultLanguage = defaultLanguage;
            DefaultPrefix = defaultPrefix;
            if (rootDirectory != "")
            {
                RootDirectory = rootDirectory;
            }
            else
            {
                //Start with the current directory
                var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
                //While the directory object is valid
                while (directory != null && directory.Exists && directory.Parent != null)
                {
                    //Does the directory contain Guilds and Languages folders?
                    if (guildsDirectoryName != "" &&
                        directory.GetDirectories().All(d => d.Name != guildsDirectoryName) ||
                        languagesDirectoryName != "" &&
                        directory.GetDirectories().All(d => d.Name != languagesDirectoryName))
                    {
                        //No, go to the upper directory.
                        directory = directory.Parent;
                        continue;
                    }

                    //Set the path to the last directory found containing those directories (oof)
                    //can be null.
                    RootDirectory = directory.FullName;
                    break;
                }

                //All of the directories don't contain needed directories
                if (directory == null || !Directory.Exists(directory.FullName))
                    throw new Exception(
                        "Diswords: couldn't find the root directory that contains languages and guilds folder.");
            }

            Console.WriteLine("Diswords: Config.RootDirectory is " + RootDirectory);
            GuildsDirectoryName = guildsDirectoryName;
            LanguagesDirectoryName = languagesDirectoryName;
            LoadingGif = $"{RootDirectory}{Path.DirectorySeparatorChar}{loadingGif}";
        }

        /// <summary>
        ///     The root directory with all the contenents and etc.
        /// </summary>
        public string RootDirectory { get; }

        /// <summary>
        ///     The directory where the guilds will be stored.
        ///     <remarks>Must be in the root directory.</remarks>
        /// </summary>
        public string GuildsDirectoryName { get; }

        /// <summary>
        ///     The directory where all languages will be stored.
        ///     <remarks>Must be in the root directory.</remarks>
        /// </summary>
        public string LanguagesDirectoryName { get; }

        /// <summary>
        ///     The default language, which the bot will use when joined the server
        /// </summary>
        public string DefaultLanguage { get; }

        /// <summary>
        ///     Default prefix of the server.
        /// </summary>
        public string DefaultPrefix { get; }

        /// <summary>
        ///     Discord Bot Token.
        /// </summary>
        public string Token { get; }

        /// <summary>
        ///     Path of the loading gif that the bot will send when it's executing a long command.
        ///     <remarks>This field is optional.</remarks>
        /// </summary>
        public string LoadingGif { get; }
    }
}