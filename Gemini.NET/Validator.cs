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

            if (!apiKey.StartsWith("AIzaSy") || apiKey.Length != 39)
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
                var generator = new Generator(apiKey);
                var apiRequest = new ApiRequestBuilder()
                    .WithPrompt("Print out `Hello world`")
                    .DisableAllSafetySettings()
                    .WithDefaultGenerationConfig(0.2F, 400)
                    .Build();

                await generator.GenerateContentAsync(apiRequest);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
