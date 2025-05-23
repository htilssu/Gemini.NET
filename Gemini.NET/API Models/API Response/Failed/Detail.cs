using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Failed
{
    public class Detail
    {
        [JsonProperty("@type")]
        public string Type;

        [JsonProperty("fieldViolations")]
        public List<FieldViolation> FieldViolations;
    }
}
