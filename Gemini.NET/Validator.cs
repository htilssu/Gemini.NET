using Models.Enums;

namespace GeminiDotNET
{
    /// <summary>
    /// Provides validation methods for Gemini model versions and API keys.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Determines if the specified model version supports grounding.
        /// </summary>
        /// <param name="modelVersion">The model version to check.</param>
        /// <returns>True if the model version supports grounding; otherwise, false.</returns>
        public static bool SupportsGrouding(ModelVersion modelVersion)
        {
            var supportedVersions = new List<ModelVersion>
                {
                    ModelVersion.Gemini_20_Flash,
                };

            return supportedVersions.Contains(modelVersion);
        }

        /// <summary>
        /// Determines if the specified model version supports JSON output.
        /// </summary>
        /// <param name="modelVersion">The model version to check.</param>
        /// <returns>True if the model version supports JSON output; otherwise, false.</returns>
        public static bool SupportsJsonOutput(ModelVersion modelVersion)
        {
            var notSupportedVersions = new List<ModelVersion>
                {
                    ModelVersion.Gemini_20_Flash_Thinking,
                };

            return !notSupportedVersions.Contains(modelVersion);
        }

        /// <summary>
        /// Validates if the provided API key is in a valid Gemini API KEY format.
        /// </summary>
        /// <param name="apiKey">The API key to validate.</param>
        /// <returns>True if the API key is valid; otherwise, false.</returns>
        public static bool CanBeValidApiKey(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                return false;
            }

            apiKey = apiKey.Trim();

            if (apiKey.Length != 39)
            {
                return false;
            }

            if (!apiKey.StartsWith("AIza"))
            {
                return false;
            }

            return true;
        }
    }
}
