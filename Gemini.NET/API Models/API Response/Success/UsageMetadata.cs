using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    /// <summary>
    /// Represents metadata about the usage of the API.
    /// </summary>
    public class UsageMetadata
    {
        /// <summary>
        /// The prompt token count.
        /// </summary>
        [JsonProperty("promptTokenCount")]
        public int? PromptTokenCount { get; set; }

        /// <summary>
        /// The candidates token count.
        /// </summary>
        [JsonProperty("candidatesTokenCount")]
        public int? CandidatesTokenCount { get; set; }

        /// <summary>
        /// The total token count.
        /// </summary>
        [JsonProperty("totalTokenCount")]
        public int? TotalTokenCount { get; set; }

        /// <summary>
        /// The details of the prompt tokens.
        /// </summary>
        [JsonProperty("promptTokensDetails")]
        public List<PromptTokensDetail>? PromptTokensDetails { get; set; }

        /// <summary>
        /// The details of the candidates tokens.
        /// </summary>
        [JsonProperty("candidatesTokensDetails")]
        public List<CandidatesTokensDetail>? CandidatesTokensDetails { get; set; }
    }
}
