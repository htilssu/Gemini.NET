using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class ApiResponse
    {
        [JsonProperty("candidates")]
        public List<Candidate> Candidates { get; set; } = [];

        [JsonProperty("usageMetadata")]
        public UsageMetadata? UsageMetadata { get; set; }

        [JsonProperty("modelVersion")]
        public string? ModelVersion { get; set; }
    }
}
