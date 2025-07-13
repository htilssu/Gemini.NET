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
        [Description("gemini-2.0-flash-lite")]
        Gemini_20_Flash_Lite,

        [Description("gemini-2.0-flash")]
        Gemini_20_Flash,

        [Description("gemini-2.5-flash")]
        Gemini_25_Flash,

        [Description("gemini-2.5-pro")]
        Gemini_25_Pro,
    }
}
