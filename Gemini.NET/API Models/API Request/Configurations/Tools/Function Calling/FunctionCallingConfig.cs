using GeminiDotNET.ApiModels.Enums;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling
{
    public class FunctionCallingConfig
    {
        [JsonProperty("mode")]
        public FunctionCallingMode Mode { get; set; } = FunctionCallingMode.AUTO;
    }
}
