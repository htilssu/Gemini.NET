using System.ComponentModel;

namespace Models.Enums
{
    /// <summary>
    /// Enum representing different Gemini model versions available through the API.
    /// Each version has specific capabilities, performance characteristics, and trade-offs
    /// in terms of speed, accuracy, and resource requirements.
    /// </summary>
    public enum ModelVersion : sbyte
    {
        /// <summary>
        /// Gemini 1.5 Flash model version.
        /// Optimized for fast responses while maintaining good quality.
        /// Suitable for real-time applications requiring quick responses.
        /// </summary>
        [Description("gemini-1.5-flash")]
        Gemini_15_Flash,

        /// <summary>
        /// Gemini 1.5 Flash 8B model version.
        /// Designed for lower intelligence tasks.
        /// </summary>
        [Description("gemini-1.5-flash-8b")]
        Gemini_15_Flash_8B,

        /// <summary>
        /// Gemini 1.5 Pro model version.
        /// Professional-grade model with enhanced capabilities and accuracy.
        /// Best for complex tasks requiring high-quality outputs.
        /// </summary>
        [Description("gemini-1.5-pro")]
        Gemini_15_Pro,

        /// <summary>
        /// Gemini 2.0 Flash Lite model version.
        /// Preview version of a lightweight, fast model.
        /// Ideal for simple tasks requiring minimal resource usage.
        /// </summary>
        [Description("gemini-2.0-flash-lite")]
        Gemini_20_Flash_Lite,

        /// <summary>
        /// Gemini 2.0 Flash Thinking model version.
        /// Experimental version optimized for reasoning tasks.
        /// Best for applications requiring complex analysis and decision-making.
        /// </summary>
        [Description("gemini-2.0-flash-thinking-exp-01-21")]
        Gemini_20_Flash_Thinking,

        /// <summary>
        /// Gemini 2.5 Pro model version.
        /// Best for complex tasks requiring high-quality outputs.
        /// </summary>
        [Description("gemini-2.5-pro-exp-03-25")]
        Gemini_25_Pro,

        /// <summary>
        /// Gemini 2.0 Flash model version.
        /// Latest stable flash model with balanced performance.
        /// Suitable for general-purpose applications requiring fast responses.
        /// </summary>
        [Description("gemini-2.0-flash")]
        Gemini_20_Flash,
    }
}
