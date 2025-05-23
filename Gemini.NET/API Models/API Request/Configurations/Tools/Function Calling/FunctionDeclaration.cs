using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling
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
