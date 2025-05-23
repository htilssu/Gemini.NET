using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success.FunctionCalling
{
    public class FunctionResponse
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("response")]
        public Response Response;
    }
}
