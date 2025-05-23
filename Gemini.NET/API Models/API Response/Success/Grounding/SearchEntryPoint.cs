using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class SearchEntryPoint
    {
        [JsonProperty("renderedContent")]
        public string? RenderedContent { get; set; }
    }
}
