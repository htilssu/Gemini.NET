using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class GroundingChunk
    {
        [JsonProperty("web")]
        public Web Web { get; set; }
    }
}
