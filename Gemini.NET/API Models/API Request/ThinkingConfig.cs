using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Request;

public class ThinkingConfig
{
    /// <summary>
    /// The thinking budget for thinking models
    /// Set -1 for disabling thinking
    /// </summary>
    [JsonProperty("thinkingBudget")]
    public int ThinkingBudget { get; set; } = -1;
}