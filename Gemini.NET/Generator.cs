using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ApiModels.Shared;
using GeminiDotNET.ClientModels;
using GeminiDotNET.Extensions;
using GeminiDotNET.Helpers;
using System.Net.Http.Headers;
using System.Text;

namespace GeminiDotNET
{
    public class Generator
    {
        private const string _apiEndpointPrefix = "https://generativelanguage.googleapis.com/v1beta/models";

        private readonly HttpClient _client;
        private readonly string? _apiKey;
        public List<Content>? HistoryContent { get; set; }
        public string RequestAsRawString { get; private set; } = string.Empty;
        public string ResponseAsRawString { get; private set; } = string.Empty;
        private int? _chatMessageLimit;

        /// <summary>
        /// Initializes a new instance of the <see cref="Generator"/> class using API key.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public Generator(string apiKey, double apiTimeOutInSecond = 120)
        {
            if (!Validator.CanBeValidApiKey(apiKey))
            {
                throw new InvalidOperationException("Invalid API key format.");
            }

            _apiKey = apiKey;
            _client = new()
            {
                Timeout = TimeSpan.FromSeconds(apiTimeOutInSecond)
            };
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

            _client = new();
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add(cloudProjectName, cloudProjectId);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            _client.Timeout = TimeSpan.FromSeconds(apiTimeOutInSecond);
        }

        /// <summary>
        /// Enables chat history for the generator, optionally setting a limit on the number of chat messages to retain.
        /// </summary>
        /// <param name="chatMessageLimit">An optional limit on the number of chat messages to retain. If specified, only the most recent  <paramref
        /// name="chatMessageLimit"/> messages will be kept. If null, no limit is applied.</param>
        /// <returns>The current <see cref="Generator"/> instance with chat history enabled, allowing for method chaining.</returns>
        public Generator EnableChatHistory(int? chatMessageLimit = null)
        {
            HistoryContent = [];
            if (chatMessageLimit.HasValue)
            {
                _chatMessageLimit = chatMessageLimit;
            }
            return this;
        }

        /// <summary>
        /// Disables the chat history by clearing its contents and message limit.
        /// </summary>
        /// <remarks>After calling this method, the chat history will no longer retain any previous
        /// messages or enforce a message limit. This method is typically used to reset the state of the chat
        /// generator.</remarks>
        /// <returns>The current <see cref="Generator"/> instance with chat history disabled, allowing for method chaining.</returns>
        public Generator DisableChatHistory()
        {
            HistoryContent = null;
            _chatMessageLimit = null;
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
        public async Task<ModelResponse> GenerateContentAsync(ApiRequest request, ModelVersion modelVersion = ModelVersion.Gemini_20_Flash_Lite)
        {
            return await GenerateContentAsync(request, modelVersion.GetDescription());
        }

        /// <summary>
        /// Generates content based on the provided API request and the specific model alias.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelAlias">The alias of the Gemini model</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ModelResponse> GenerateContentAsync(ApiRequest request, string modelAlias)
        {
            if (string.IsNullOrEmpty(modelAlias))
            {
                throw new ArgumentNullException(nameof(modelAlias), "Model alias is required.");
            }

            var endpoint = $"{_apiEndpointPrefix}/{modelAlias}:generateContent";
            if (!string.IsNullOrEmpty(_apiKey))
            {
                endpoint += $"?key={_apiKey}";
                _client.DefaultRequestHeaders.Clear();
            }

            try
            {
                SetChatHistory(request.Contents);
                if (HistoryContent != null && HistoryContent.Count > 0)
                {
                    request.Contents = HistoryContent;
                }
                RequestAsRawString = request.AsString();
                var body = new StringContent(RequestAsRawString, Encoding.UTF8, "application/json");
                var responseMessage = await _client.PostAsync(endpoint, body);
                ResponseAsRawString = await responseMessage.Content.ReadAsStringAsync();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    try
                    {
                        var modelFailedResponse = JsonHelper.AsObject<ApiModels.Response.Failed.ApiResponse>(ResponseAsRawString);

                        throw new GeminiException(modelFailedResponse == null
                                ? "Undefined"
                                : $"{modelFailedResponse.Error.Status} ({modelFailedResponse.Error.Code}): {modelFailedResponse.Error.Message}",
                            modelFailedResponse,
                            new Exception(ResponseAsRawString));
                    }
                    catch (Exception ex)
                    {
                        var dto = new ApiModels.Response.Failed.ApiResponse
                        {
                            Error = new ApiModels.Response.Failed.Error
                            {
                                Code = (int)responseMessage.StatusCode,
                                Message = $"{ex.Message}\n{ex.StackTrace}",
                            }
                        };

                        throw new GeminiException(dto == null
                                ? "Undefined"
                                : $"{dto.Error.Status} ({dto.Error.Code}): {dto.Error.Message}",
                            dto,
                            new Exception(ResponseAsRawString));
                    }
                }

                var modelSuccessResponse = JsonHelper.AsObject<ApiModels.Response.Success.ApiResponse>(ResponseAsRawString);

                var firstCandidate = modelSuccessResponse?.Candidates.FirstOrDefault();
                var parts = firstCandidate?.Content?.Parts;
                var groudingMetadata = firstCandidate?.GroundingMetadata;

                var texts = modelSuccessResponse?.Candidates
                    .Select(c => c.Content)
                    .SelectMany(c => c.Parts)
                    .Select(p => p.Text)
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();

                var modelResponse = new ModelResponse
                {
                    Content = string.Join("\n\n", texts),
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

                    FunctionCalls = parts != null && parts.Any(p => p.FunctionCall != null)
                        ? [.. parts.Where(p => p.FunctionCall != null).Select(p => p.FunctionCall!)]
                        : null,
                    FunctionResponses = parts != null && parts.Any(p => p.FunctionResponse != null)
                        ? [.. parts.Where(p => p.FunctionResponse != null).Select(p => p.FunctionResponse!)]
                        : null,
                };

                if (modelSuccessResponse != null && modelSuccessResponse.Candidates.Count > 0)
                {
                    SetChatHistory([.. modelSuccessResponse.Candidates.Select(c => c.Content)]);
                }

                return modelResponse;
            }
            catch (Exception ex)
            {
                SetChatHistory(
                        [
                            new Content
                            {
                                Parts = [new Part { Text = ex.Message }],
                                Role = "model"
                            }
                        ]);

                throw new InvalidOperationException($"{ex.Message}\n{ex.StackTrace}", ex.InnerException);
            }
        }

        private void SetChatHistory(List<Content?> contents)
        {
            if (HistoryContent == null || contents == null || contents.Count == 0)
            {
                return;
            }

            foreach (var content in contents)
            {
                if (content == null || content.Parts == null || content.Parts.Count == 0)
                {
                    continue;
                }

                HistoryContent.Add(content);
            }

            if (_chatMessageLimit.HasValue)
            {
                HistoryContent = [.. HistoryContent.TakeLast(_chatMessageLimit.Value)];
            }
        }
    }
}
