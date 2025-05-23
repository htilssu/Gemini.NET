using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling
{
    public class Parameters
    {
        [JsonProperty("type")]

        public string Type { get; } = "object";

        [JsonProperty("properties")]
        public object Properties { get; set; }
    }
}
