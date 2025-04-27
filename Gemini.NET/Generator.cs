using GeminiDotNET.Client_Models;
using GeminiDotNET.Helpers;
using Models.Enums;
using Models.Request;
using System.Net.Http.Headers;
using System.Text;

namespace GeminiDotNET
{
    public class Generator
    {
        private const string _apiEndpointPrefix = "https://generativelanguage.googleapis.com/v1beta/models";

        private readonly HttpClient _client = new();
        private readonly string? _apiKey;

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

            _apiKey = apiKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class using Google Cloud project credentials.
        /// </summary>
        /// <param name="cloudProjectName">The Google Cloud project name.</param>
        /// <param name="cloudProjectId">The Google Cloud project ID.</param>
        /// <param name="bearer">The Bearer token.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Generator(string cloudProjectName, string cloudProjectId, string bearer, double apiTimeOutInSecond = 120)
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
            _client.Timeout = TimeSpan.FromSeconds(apiTimeOutInSecond);
        }

        /// <summary>
        /// Generates content based on the provided API request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelVersion"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ModelResponse> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion = ModelVersion.Gemini_20_Flash_Lite, double apiTimeOutInSecond = 120)
        {
            return await GenerateContentAsync(request, EnumHelper.GetDescription(modelVersion), apiTimeOutInSecond);
        }

        /// <summary>
        /// Generates content based on the provided API request and the specific model alias.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelAlias">The alias of the Gemini model</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ModelResponse> GenerateContentAsync(ApiRequest request, string modelAlias, double apiTimeOutInSecond = 120)
        {
            if (string.IsNullOrEmpty(modelAlias))
            {
                throw new ArgumentNullException(nameof(modelAlias), "Model alias is required.");
            }

            try
            {
                _client.DefaultRequestHeaders.Clear();
                _client.Timeout = TimeSpan.FromSeconds(apiTimeOutInSecond);
                var json = JsonHelper.AsString(request);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($@"{_apiEndpointPrefix}/{modelAlias}:generateContent?key={_apiKey}", body);
                var responseData = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var dto = JsonHelper.AsObject<Models.Response.Failed.ApiResponse>(responseData);
                        throw new InvalidOperationException(dto == null ? "Undefined" : $"{dto.Error.Status} ({dto.Error.Code}): {dto.Error.Message}",
                            dto?.Error?.Details != null
                            ? new Exception(JsonHelper.AsString(dto?.Error?.Details))
                            : new Exception(responseData));
                    }
                    catch (Exception ex)
                    {
                        var dto = new Models.Response.Failed.ApiResponse
                        {
                            Error = new Models.Response.Failed.Error
                            {
                                Code = (int)response.StatusCode,
                                Message = ex.Message,
                            }
                        };

                        throw new InvalidOperationException($"{dto.Error.Status} ({dto.Error.Code}): {dto.Error.Message}", new Exception(responseData));
                    }
                }
                else
                {
                    try
                    {
                        var dto = JsonHelper.AsObject<Models.Response.Success.ApiResponse>(responseData);
                        var groudingMetadata = dto.Candidates[0]?.GroundingMetadata;

                        return new ModelResponse
                        {
                            Content = dto.Candidates[0].Content != null
                                ? dto.Candidates[0].Content?.Parts[0].Text
                                : "Failed to generate content",
                            GroundingDetail = groudingMetadata != null
                                ? new GroundingDetail
                                {
                                    RenderedContentAsHtml = groudingMetadata?.SearchEntryPoint?.RenderedContent ?? null,
                                    SearchSuggestions = groudingMetadata?.WebSearchQueries,
                                    ReliableInformation = groudingMetadata?.GroundingSupports?
                                        .OrderByDescending(s => s.ConfidenceScores.Max())
                                        .Select(s => s.Segment.Text),
                                    Sources = groudingMetadata?.GroundingChunks?
                                        .Select(c => new GroundingSource
                                        {
                                            Domain = c.Web.Title,
                                            Url = c.Web.Uri,
                                        }),
                                }
                                : null,
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Failed to parse response from JSON: {ex.Message}", ex.InnerException);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to send request to Gemini: {ex.Message}", ex.InnerException);
            }
        }
    }
}
