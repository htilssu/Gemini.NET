using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Failed
{
    public class ApiResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; }
    }
}
