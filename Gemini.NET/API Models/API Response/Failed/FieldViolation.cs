using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Failed
{
    public class FieldViolation
    {
        [JsonProperty("field")]
        public string Field;

        [JsonProperty("description")]
        public string Description;
    }
}
