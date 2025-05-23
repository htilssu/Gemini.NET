using GeminiDotNET.ApiModels.Shared;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class Candidate
    {
        [JsonProperty("content")]
        public Content? Content { get; set; }

        [JsonProperty("finishReason")]
        public string? FinishReason { get; set; }

        [JsonProperty("index")]
        public int? Index { get; set; }

        [JsonProperty("groundingMetadata")]
        public GroundingMetadata? GroundingMetadata { get; set; }
    }
}
