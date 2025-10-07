using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Request;

public class ThinkingConfig
{
    [JsonProperty("thinkingBudget")]
    public int ThinkingBudget { get; set; } = -1;
}