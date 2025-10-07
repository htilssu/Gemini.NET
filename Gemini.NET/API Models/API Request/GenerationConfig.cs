using Gemini.NET.API_Models.API_Request;
using GeminiDotNET.Helpers;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Request
{
    /// <summary>
    /// Represents configuration parameters for controlling the model's text generation behavior.
    /// Contains settings that affect the creativity, diversity, and length of generated content.
    /// </summary>
    public class GenerationConfig
    {
        /// <summary>
        /// The sampling temperature.
        /// Controls the randomness of the output. Higher values (e.g., 0.8) make the output more random,
        /// while lower values (e.g., 0.2) make it more focused and deterministic.
        /// Default value is 1.0.
        /// </summary>
        [JsonProperty("temperature")]
        public float Temperature { get; set; } = 1;

        /// <summary>
        /// The top-k sampling parameter.
        /// Limits the cumulative probability of tokens considered for generation to the k most likely ones.
        /// Higher values allow more diverse word choices.
        /// Default value is 40.
        /// </summary>
        [JsonProperty("topK")]
        public sbyte TopK { get; set; } = 40;

        /// <summary>
        /// The nucleus sampling parameter.
        /// Limits the cumulative probability of tokens considered for generation.
        /// Higher values (e.g., 0.95) allow more diverse word choices.
        /// Default value is 0.95.
        /// </summary>
        [JsonProperty("topP")]
        public float TopP { get; set; } = 0.95F;

        /// <summary>
        /// The maximum number of tokens in the generated output.
        /// Limits the length of the model's response.
        /// Default value is 8192.
        /// </summary>
        [JsonProperty("maxOutputTokens")]
        public int? MaxOutputTokens { get; set; }

        /// <summary>
        /// The MIME type of the expected response.
        /// Determines the format of the generated content (e.g., text/plain, text/markdown).
        /// Default value is text/plain.
        /// </summary>
        [JsonProperty("responseMimeType")]
        internal string ResponseMimeType { get; set; } = Enums.ResponseMimeType.PlainText.GetDescription();

        [JsonProperty("responseSchema")]
        public object? ResponseSchema { get; set; }
        
        [JsonProperty("thinkingConfig")]
        public ThinkingConfig? ThinkingConfig { get; set; }
    }
}