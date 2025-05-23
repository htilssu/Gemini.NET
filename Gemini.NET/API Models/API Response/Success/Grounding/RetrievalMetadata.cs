using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class RetrievalMetadata
    {
        [JsonProperty("googleSearchDynamicRetrievalScore")]
        public double? GoogleSearchDynamicRetrievalScore { get; set; }
    }
}
