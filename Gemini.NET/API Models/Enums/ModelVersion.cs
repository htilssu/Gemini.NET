using System.ComponentModel;

namespace GeminiDotNET.ApiModels.Enums
{
    /// <summary>
    /// Enum representing different Gemini model versions available through the API.
    /// Each version has specific capabilities, performance characteristics, and trade-offs
    /// in terms of speed, accuracy, and resource requirements.
    /// </summary>
    public enum ModelVersion : sbyte
    {
        /// <summary>
        /// Gemini 2.0 Flash Lite model version.
        /// Preview version of a lightweight, fast model.
        /// Ideal for simple tasks requiring minimal resource usage.
        /// </summary>
        [Description("gemini-2.0-flash-lite")]
        Gemini_20_Flash_Lite,

        /// <summary>
        /// Gemini 2.0 Flash model version.
        /// Latest stable flash model with balanced performance.
        /// Suitable for general-purpose applications requiring fast responses.
        /// </summary>
        [Description("gemini-2.0-flash")]
        Gemini_20_Flash,
    }
}
