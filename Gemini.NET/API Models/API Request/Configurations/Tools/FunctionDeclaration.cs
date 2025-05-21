using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Request.Configurations.Tools
{
    public class FunctionDeclaration
    {
        [JsonProperty("name")]
        public required string Name { get; set; }
        [JsonProperty("description")]
        public required string Description { get; set; }
        [JsonProperty("parameters")]
        public Parameters? Parameters { get; set; }
    }
}
