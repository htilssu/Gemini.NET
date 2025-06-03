using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.Helpers;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest.Configurations
{
    /// <summary>
    /// Represents a safety configuration that controls content filtering for specific harm categories.
    /// Safety settings allow fine-grained control over the model's content generation
    /// to prevent potentially harmful or inappropriate content.
    /// </summary>
    public class SafetySetting
    {
        /// <summary>
        /// The harm category to apply safety settings to.
        /// This is a required field that specifies which type of content to filter (e.g., hate speech, harassment).
        /// The value should correspond to a SafetySettingHarmCategory enum value.
        /// </summary>
        [JsonProperty("category")]
        public required string Category { get; set; }

        /// <summary>
        /// The threshold level for content filtering in this category.
        /// Determines how strictly to filter content, from BLOCK_NONE to BLOCK_ALL.
        /// Default value is BLOCK_NONE, allowing all content in this category.
        /// </summary>
        [JsonProperty("threshold")]
        public string Threshold { get; set; } = SafetySettingHarmThreshold.BlockNone.GetDescription();
    }
}
