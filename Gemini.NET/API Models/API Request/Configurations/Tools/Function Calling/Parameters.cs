using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling
{
    public class Parameters
    {
        [JsonProperty("type")]

        public string Type { get; } = "object";

        [JsonProperty("properties")]
        public required object Properties { get; set; }

        [JsonProperty("required")]
        public List<string>? Required { get; set; }
    }
}
