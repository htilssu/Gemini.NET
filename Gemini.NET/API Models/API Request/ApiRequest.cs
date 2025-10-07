using GeminiDotNET.ApiModels.ApiRequest.Configurations;
using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools;
using GeminiDotNET.ApiModels.Request;
using GeminiDotNET.ApiModels.Shared;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest
{
    /// <summary>
    /// Represents a request to the Gemini API for generating content or performing tasks.
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// The list of content parts that form the input to the model.
        /// This includes the user's message and any additional content like images.
        /// </summary>
        [JsonProperty("contents")]
        public List<Content> Contents { get; set; }

        /// <summary>
        /// The configuration for the generation process.
        /// This includes parameters like temperature, top-k, and maximum output tokens.
        /// </summary>
        [JsonProperty("generationConfig")]
        public GenerationConfig? GenerationConfig { get; set; }

        /// <summary>
        /// The configuration for the tools that the model can use.
        /// </summary>
        [JsonProperty("toolConfig")]
        public ToolConfig? ToolConfig { get; set; }

        /// <summary>
        /// The system instructions that guide the model's behavior.
        /// This is optional and can be used to set specific context or constraints.
        /// </summary>
        [JsonProperty("systemInstruction")]
        public SystemInstruction? SystemInstruction { get; set; }

        /// <summary>
        /// The list of tools available to the model.
        /// Tools can include features like web search or function calling capabilities.
        /// This is optional and depends on the model's capabilities.
        /// </summary>
        [JsonProperty("tools")]
        public List<Tool>? Tools { get; set; }

        /// <summary>
        /// The safety settings for content generation.
        /// These settings control the model's behavior regarding potentially harmful content.
        /// This is optional and uses default safety settings if not specified.
        /// </summary>
        [JsonProperty("safetySettings")]
        public List<SafetySetting>? SafetySettings { get; set; }
    }
}