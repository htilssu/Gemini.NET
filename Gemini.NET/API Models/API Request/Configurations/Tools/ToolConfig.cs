using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools
{
    public class ToolConfig
    {
        [JsonProperty("functionCallingConfig")]
        public FunctionCallingConfig FunctionCallingConfig { get; set; }
    }
}
