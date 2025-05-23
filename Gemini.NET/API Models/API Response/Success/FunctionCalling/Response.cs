using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success.FunctionCalling
{
    public class Response
    {
        [JsonProperty("output")]
        public string Output;
    }
}
