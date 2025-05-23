using System.ComponentModel;

namespace GeminiDotNET.ApiModels.Enums
{
    /// <summary>
    /// The harm block threshold settings for content safety filtering.
    /// These thresholds determine how strictly the model filters potentially harmful content
    /// based on probability and severity scores.
    /// </summary>
    public enum SafetySettingHarmThreshold : sbyte
    {
        /// <summary>
        /// Blocks content when the probability score or severity score is LOW, MEDIUM, or HIGH.
        /// This is the most restrictive setting, suitable for applications requiring strict content control.
        /// Use this for scenarios where even mildly questionable content should be filtered.
        /// </summary>
        [Description("BLOCK_LOW_AND_ABOVE")]
        BlockLowAndAbove,

        /// <summary>
        /// Blocks content when the probability score or severity score is MEDIUM or HIGH.
        /// Provides moderate content filtering, allowing low-risk content through.
        /// Suitable for general-purpose applications requiring reasonable safety measures.
        /// </summary>
        [Description("BLOCK_MEDIUM_AND_ABOVE")]
        BlockMediumAndAbove,

        /// <summary>
        /// Blocks only HIGH severity content, allowing NEGLIGIBLE and LOW through.
        /// Provides minimal content filtering, only blocking the most concerning content.
        /// Suitable for applications where broader expression is desired while maintaining basic safety.
        /// </summary>
        [Description("BLOCK_ONLY_HIGH")]
        BlockOnlyHigh,

        /// <summary>
        /// Uses the model's default threshold for content blocking.
        /// The specific threshold is determined by the model's configuration.
        /// Use this when you want to rely on the model's built-in safety defaults.
        /// </summary>
        [Description("HARM_BLOCK_THRESHOLD_UNSPECIFIED")]
        HarmBlockThresholdUnspecified,

        /// <summary>
        /// Completely disables the safety filter for this category.
        /// WARNING: Use with caution as this may allow potentially harmful content.
        /// Only suitable for specialized applications with their own content filtering.
        /// </summary>
        [Description("OFF")]
        Off,

        /// <summary>
        /// Allows all content through without any blocking.
        /// Similar to OFF, but explicitly indicates that no content should be blocked.
        /// Use with caution and only in controlled environments.
        /// </summary>
        [Description("BLOCK_NONE")]
        BlockNone
    }
}
