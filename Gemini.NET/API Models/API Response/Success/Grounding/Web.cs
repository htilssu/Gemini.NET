using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class Web
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
