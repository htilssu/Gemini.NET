using Models.Enums;

namespace GeminiDotNET
{
    /// <summary>
    /// Provides validation methods for Gemini model versions and API keys.
    /// </summary>
    public static class Validator
    {
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
            if (!CanBeValidApiKey(apiKey))
            {
                return false;
            }

            try
            {
                using var client = new HttpClient();
                using var response = await client.GetAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-lite?key={apiKey}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
