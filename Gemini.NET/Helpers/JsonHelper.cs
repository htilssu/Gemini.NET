using Newtonsoft.Json;

namespace Gemini.NET.Helpers
{
    /// <summary>
    /// Helper class for JSON operations
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Serializes an object to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string AsString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Deserializes a JSON string to an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>An object of type T.</returns>
        public static T? AsObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
