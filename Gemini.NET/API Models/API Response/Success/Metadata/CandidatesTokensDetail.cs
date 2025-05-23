using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response
{
    /// <summary>
    /// Represents details about the candidates tokens.
    /// </summary>
    public class CandidatesTokensDetail
    {
        /// <summary>
        /// The modality of the candidates tokens.
        /// </summary>
        [JsonProperty("modality")]
        public string? Modality { get; set; }

        /// <summary>
        /// The token count of the candidates tokens.
        /// </summary>
        [JsonProperty("tokenCount")]
        public int? TokenCount { get; set; }
    }
}
