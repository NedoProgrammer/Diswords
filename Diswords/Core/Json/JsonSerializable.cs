using System;
using Newtonsoft.Json;

namespace Diswords.Core.Json
{
    public abstract class JsonSerializable<T>
    {
        public string ToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Diswords: exception occured (Language.ToJson).\n{e}");
                throw;
            }
        }

        public static T FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Diswords: exception occured (Language.FromJson).\n{e}");
                throw;
            }
        }
    }
}