using Gemini.NET.Client_Models;
using Gemini.NET.Helpers;
using Models.Enums;
using Models.Request;
using System.Net.Http.Headers;
using System.Text;

namespace Gemini.NET
{
    public class Generator
    {
        private const string _apiEndpointPrefix = "https://generativelanguage.googleapis.com/v1beta/models";

        private readonly HttpClient _client = new();
        private readonly string? _apiKey;
        private bool _includesGroundingDetailInResponse = false;
        private bool _includesSearchEntryPointInResponse = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class using API key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Generator(string apiKey)
        {
            if (!Validator.CanBeValidApiKey(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey), "Invalid or expired API key.");
            }

            _apiKey = apiKey.Trim();
        }

        /// <summary>
        /// Check if the provided API key is valid.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsValidApiKeyAsync()
        {
            var apiRequest = new ApiRequestBuilder()
                .WithPrompt("Say `Hello World` to me!")
                .DisableAllSafetySettings()
                .WithDefaultGenerationConfig()
                .Build();

            try
            {
                await GenerateContentAsync(apiRequest, ModelVersion.Gemini_15_Flash).ConfigureAwait(false);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class using Google Cloud project credentials.
        /// </summary>
        /// <param name="cloudProjectName">The Google Cloud project name.</param>
        /// <param name="cloudProjectId">The Google Cloud project ID.</param>
        /// <param name="bearer">The Bearer token.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Generator(string cloudProjectName, string cloudProjectId, string bearer)
        {
            if (string.IsNullOrEmpty(cloudProjectName))
            {
                throw new ArgumentNullException(nameof(cloudProjectName), "Google Cloud project name is required.");
            }

            if (string.IsNullOrEmpty(cloudProjectId))
            {
                throw new ArgumentNullException(nameof(cloudProjectId), "Google Cloud project ID is required.");
            }

            if (string.IsNullOrEmpty(bearer))
            {
                throw new ArgumentNullException(nameof(bearer), "Bearer token is required.");
            }

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add(cloudProjectName, cloudProjectId);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
        }

        /// <summary>
        /// Includes grounding detail in the response.
        /// </summary>
        /// <returns></returns>
        public Generator IncludesGroundingDetailInResponse()
        {
            if (!_includesGroundingDetailInResponse)
            {
                _includesGroundingDetailInResponse = true;
            }
            return this;
        }

        /// <summary>
        /// Excludes grounding detail from the response.
        /// </summary>
        /// <returns></returns>
        public Generator ExcludesGroundingDetailFromResponse()
        {
            if (_includesGroundingDetailInResponse)
            {
                _includesGroundingDetailInResponse = false;
            }
            return this;
        }

        /// <summary>
        /// Includes search entry point in the response.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Generator IncludesSearchEntryPointInResponse()
        {
            if (!_includesGroundingDetailInResponse)
            {
                throw new InvalidOperationException("Grounding detail must be included in the response to include search entry point.");
            }

            if (!_includesSearchEntryPointInResponse)
            {
                _includesSearchEntryPointInResponse = true;
            }

            return this;
        }

        /// <summary>
        /// Excludes search entry point from the response.
        /// </summary>
        /// <returns></returns>
        public Generator ExcludesSearchEntryPointFromResponse()
        {
            if (_includesSearchEntryPointInResponse)
            {
                _includesSearchEntryPointInResponse = false;
            }

            return this;
        }

        /// <summary>
        /// Generates content based on the provided API request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelVersion"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ModelResponse> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion = ModelVersion.Gemini_20_Flash)
        {
            if (request.Tools != null && request.Tools.Count > 0 && !Validator.SupportsGrouding(modelVersion))
            {
                throw new ArgumentNullException(nameof(request), "Grounding is not supported for this model version.");
            }

            if (request.GenerationConfig.ResponseMimeType == EnumHelper.GetDescription(ResponseMimeType.Json) && !Validator.SupportsJsonOutput(modelVersion))
            {
                throw new ArgumentNullException(nameof(request), "JSON output is not supported for this model version.");
            }

            var endpoint = $@"{_apiEndpointPrefix}/{EnumHelper.GetDescription(modelVersion)}:generateContent";

            if (!string.IsNullOrEmpty(_apiKey))
            {
                endpoint += $"?key={_apiKey}";
                _client.DefaultRequestHeaders.Clear();
            }

            var json = JsonHelper.AsString(request);
            var body = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(endpoint, body).ConfigureAwait(false);
                var responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                try
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var dto = JsonHelper.AsObject<Models.Response.Failed.ApiResponse>(responseData);
                        throw new InvalidOperationException(dto == null ? "Undefined" : $"{dto.Error.Status} ({dto.Error.Code}): {dto.Error.Message}");
                    }
                    else
                    {
                        var dto = JsonHelper.AsObject<Models.Response.Success.ApiResponse>(responseData);
                        var groudingMetadata = dto.Candidates[0].GroundingMetadata;

                        Console.WriteLine(responseData);

                        return new ModelResponse
                        {
                            Result = dto.Candidates[0].Content != null
                                ? dto.Candidates[0].Content.Parts[0].Text.Trim()
                                : "Failed to generate content",
                            GroundingDetail = groudingMetadata != null && _includesGroundingDetailInResponse
                                ? new GroundingDetail
                                {
                                    RenderedContentAsHtml = _includesSearchEntryPointInResponse
                                        ? groudingMetadata?.SearchEntryPoint?.RenderedContent
                                        : null,
                                    SearchSuggestions = groudingMetadata?.WebSearchQueries,
                                    ReliableInformation = groudingMetadata?.GroundingSupports?
                                        .OrderByDescending(s => s.ConfidenceScores.Max())
                                        .Select(s => s.Segment.Text)
                                        .ToList(),
                                    Sources = groudingMetadata?.GroundingChunks?
                                        .Select(c => new GroundingSource
                                        {
                                            Domain = c.Web.Title,
                                            Url = c.Web.Uri,
                                        })
                                        .ToList(),
                                }
                                : null,
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Failed to parse response from JSON:\n{responseData}", ex);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send request to Gemini: {ex.Message}\n{json}", ex);
            }
        }


        /// <summary>
        /// Gets the latest stable model version of Gemini.
        /// </summary>
        /// <returns></returns>
        public static ModelVersion GetLatestStableModelVersion()
        {
            return Enum.GetValues(typeof(ModelVersion)).Cast<ModelVersion>().Max();
        }
    }
}
