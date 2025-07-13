using Gemini.NET.API_Models.API_Response.Success;
using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ApiModels.Shared;
using GeminiDotNET.ClientModels;
using GeminiDotNET.Extensions;
using GeminiDotNET.Helpers;
using System.Net.Http.Headers;
using System.Text;

namespace Gemini.NET
{
    public class GeminiCodeAssistGenerator
    {
        private const string _apiEndpointPrefix = "https://cloudcode-pa.googleapis.com/v1internal";
        public List<Content>? HistoryContent { get; set; }
        public string RequestAsRawString { get; private set; } = string.Empty;
        public string ResponseAsRawString { get; private set; } = string.Empty;
        private int? _chatMessageLimit { get; set; } = null;
        public required string CloudProjectId { get; set; }

        public GeminiCodeAssistGenerator EnableChatHistory(int? chatMessageLimit = null)
        {
            HistoryContent = [];
            if (chatMessageLimit.HasValue)
            {
                _chatMessageLimit = chatMessageLimit;
            }

            return this;
        }
        public GeminiCodeAssistGenerator DisableChatHistory()
        {
            HistoryContent = null;
            _chatMessageLimit = null;
            return this;
        }

        public async Task<ModelResponse> GenerateContentAsync(string bearer, ApiRequest request, ModelVersion modelVersion = ModelVersion.Gemini_25_Flash, double timeOutInSeconds = 120)
        {
            if (modelVersion != ModelVersion.Gemini_25_Flash && modelVersion != ModelVersion.Gemini_25_Pro)
            {
                throw new ArgumentException("Invalid model version. Only Gemini 2.5 Flash and Pro are supported.");
            }

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);
            client.Timeout = TimeSpan.FromSeconds(timeOutInSeconds);

            try
            {
                SetChatHistory(request.Contents);
                if (HistoryContent != null && HistoryContent.Count > 0)
                {
                    request.Contents = HistoryContent;
                }
                RequestAsRawString = request.AsString();

                var codeAssistRequest = new
                {
                    project = CloudProjectId,
                    model = modelVersion.GetDescription(),
                    request
                };

                var body = new StringContent(codeAssistRequest.AsString(), Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync($"{_apiEndpointPrefix}:generateContent", body);
                ResponseAsRawString = await responseMessage.Content.ReadAsStringAsync();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    try
                    {
                        var modelFailedResponse = JsonHelper.AsObject<GeminiDotNET.ApiModels.Response.Failed.ApiResponse>(ResponseAsRawString);

                        throw new GeminiException(modelFailedResponse == null
                                ? "Undefined"
                                : $"{modelFailedResponse.Error.Status} ({modelFailedResponse.Error.Code}): {modelFailedResponse.Error.Message}",
                            modelFailedResponse,
                            new Exception(ResponseAsRawString));
                    }
                    catch (Exception ex)
                    {
                        var dto = new GeminiDotNET.ApiModels.Response.Failed.ApiResponse
                        {
                            Error = new GeminiDotNET.ApiModels.Response.Failed.Error
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

                var modelSuccessResponse = JsonHelper.AsObject<GeminiCodeAssistApiResponse>(ResponseAsRawString);

                var firstCandidate = modelSuccessResponse?.Response?.Candidates.FirstOrDefault();
                var parts = firstCandidate?.Content?.Parts;
                var groundingMetadata = firstCandidate?.GroundingMetadata;

                var texts = modelSuccessResponse?.Response?.Candidates
                    .Select(c => c.Content)
                    .SelectMany(c => c.Parts)
                    .Select(p => p.Text)
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();

                var modelResponse = new ModelResponse
                {
                    Content = string.Join("\n\n", texts),
                    GroundingDetail = groundingMetadata != null
                        ? new GroundingDetail
                        {
                            RenderedContentAsHtml = groundingMetadata?.SearchEntryPoint?.RenderedContent ?? null,
                            SearchSuggestions = groundingMetadata?.WebSearchQueries,
                            ReliableInformation = groundingMetadata?.GroundingSupports?
                                .OrderByDescending(s => s.ConfidenceScores.Max())
                                .Select(s => s.Segment.Text),
                            Sources = groundingMetadata?.GroundingChunks?
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

                if (modelSuccessResponse != null && modelSuccessResponse?.Response.Candidates.Count > 0)
                {
                    SetChatHistory([.. modelSuccessResponse?.Response.Candidates.Select(c => c.Content)]);
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

        public async Task SetPrivacySettingAsync(string bearer, bool enableDataCollection)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var request = new
            {
                cloudaicompanionProject = CloudProjectId,
                freeTierDataCollectionOptin = enableDataCollection,
            };

            var body = new StringContent(request.AsString(), Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync($"{_apiEndpointPrefix}:setCodeAssistGlobalUserSetting", body);

            responseMessage.EnsureSuccessStatusCode();

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
