using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Response.Success.FunctionCalling
{
    public class FunctionCall
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("args")]
        public object? Args;
    }
}
