using System.IO;
using Diswords.Core.Json;

namespace Diswords
{
    /// <summary>
    ///     Main class that will run the bot.
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Main method :D
        ///     It uses a specific json config (see <see cref="JsonConfig" /> for an example)
        /// </summary>
        /// <exception cref="FileNotFoundException">if the config file is not found.</exception>
        /// <exception cref="System.Exception">if something else has gone wrong.</exception>
        public static void Main()
        {
            var client = new DiswordsClient(JsonConfig.FromJsonFile("myconfig.json").Result);
            client.RunAsync().GetAwaiter().GetResult();
        }
    }
}