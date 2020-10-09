using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Diswords.Core.Json
{
    /// <summary>
    ///     A class that helps to serialize/deserialize json objects.
    /// </summary>
    /// <typeparam name="T">The class that is going to be serialized.</typeparam>
    public abstract class JsonSerializable<T>
    {
        /// <summary>
        ///     Convert an object to a json string.
        /// </summary>
        /// <returns>A formatted json string.</returns>
        public string ToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Diswords: exception occured (JsonSerializable.ToJson).\n{e}");
                throw;
            }
        }

        /// <summary>
        ///     Convert & write object to a file.
        /// </summary>
        /// <param name="path">The file to which the json string is written to.</param>
        public void OverwriteTo(string path)
        {
            try
            {
                File.WriteAllText(path, ToJson());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Diswords: exception occured (JsonSerializable.OverwriteTo).\n{e}");
                throw;
            }
        }

        /// <summary>
        ///     Deserialize a json string (from a file) into object (T)
        /// </summary>
        /// <param name="path">The file which contains the json string.</param>
        /// <returns>A deserialized object (T)</returns>
        public static async Task<T> FromJsonFile(string path)
        {
            try
            {
                return FromJson(await File.ReadAllTextAsync(path));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Diswords: exception occured (JsonSerializable.FromFile).\n{e}");
                throw;
            }
        }

        /// <summary>
        ///     Deserialize a json string into object (T)
        /// </summary>
        /// <param name="json">The json string.</param>
        /// <returns>A deserialized object (T)</returns>
        public static T FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Diswords: exception occured (JsonSerializable.FromJson).\n{e}");
                throw;
            }
        }
    }
}