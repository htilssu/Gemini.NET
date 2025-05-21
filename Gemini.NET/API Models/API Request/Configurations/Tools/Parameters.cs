using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Request.Configurations.Tools
{
    public class Parameters
    {
        [JsonProperty("type")]

        public string Type { get; } = "object";

        [JsonProperty("properties")]
        public object Properties { get; set; }
    }
}
