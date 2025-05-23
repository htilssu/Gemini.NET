using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools;
using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Shared
{
    /// <summary>
    /// Represents a tool that extends the model's capabilities.
    /// Tools provide additional functionality like web search that the model can use during generation.
    /// </summary>
    public class Tool
    {
        /// <summary>
        /// The Google Search tool configuration.
        /// When configured, allows the model to perform web searches during content generation.
        /// </summary>
        [JsonProperty("googleSearch")]
        public GoogleSearch? GoogleSearch { get; set; }

        /// <summary>
        /// The function calling tool configuration.
        /// When configured, allows the model to call functions during content generation.
        /// </summary>
        [JsonProperty("functionDeclarations")]
        public List<FunctionDeclaration>? FunctionDeclarations;
    }
}
