using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools
{
    public class GoogleSearchRetrieval
    {
        [JsonProperty("dynamic_retrieval_config")]
        public required DynamicRetrievalConfig DynamicRetrievalConfig { get; set; }
    }
}
