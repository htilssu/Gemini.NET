using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response
{
    /// <summary>
    /// Represents details about the prompt tokens.
    /// </summary>
    public class PromptTokensDetail
    {
        /// <summary>
        /// The modality of the prompt tokens.
        /// </summary>
        [JsonProperty("modality")]
        public string Modality { get; set; }

        /// <summary>
        /// The token count of the prompt tokens.
        /// </summary>
        [JsonProperty("tokenCount")]
        public int? TokenCount { get; set; }
    }
}
