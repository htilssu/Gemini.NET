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

        /// <summary>
        /// Check if the provided API key is valid.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> IsValidApiKeyAsync(string apiKey)
        {
            try
            {
                using var client = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Head, $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-lite?key={apiKey}");
                using var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
