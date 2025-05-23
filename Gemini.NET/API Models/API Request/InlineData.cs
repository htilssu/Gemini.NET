using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest
{
    public class InlineData
    {
        [JsonProperty("mime_type")]
        public required string MimeType { get; set; }

        [JsonProperty("data")]
        public required string Data { get; set; }
    }
}
