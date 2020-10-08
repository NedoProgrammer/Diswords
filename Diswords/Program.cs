using System.IO;
using Diswords.Core.Json;

namespace Diswords
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new DiswordsClient(JsonConfig.FromJsonFile("myconfig.json").Result);
            client.RunAsync().GetAwaiter().GetResult();
        }

    }
}