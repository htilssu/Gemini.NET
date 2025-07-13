using GeminiDotNET.ApiModels.Response.Success;
using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Response.Success
{
    public class GeminiCodeAssistApiResponse
    {
        [JsonProperty("response")]
        public ApiResponse Response { get; set; }
    }
}
