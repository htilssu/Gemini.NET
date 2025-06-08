using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.ApiRequest.Configurations;
using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools;
using GeminiDotNET.ApiModels.ApiRequest.Configurations.Tools.FunctionCalling;
using GeminiDotNET.ApiModels.Enums;
using GeminiDotNET.ApiModels.Request;
using GeminiDotNET.ApiModels.Response.Success.FunctionCalling;
using GeminiDotNET.ApiModels.Shared;
using GeminiDotNET.ClientModels;
using GeminiDotNET.Helpers;

namespace GeminiDotNET
{
    /// <summary>
    /// Builder class for constructing API requests to the Gemini service.
    /// Provides methods to set various parameters and configurations for the request.
    /// </summary>
    public class ApiRequestBuilder
    {
        private string? _prompt;
        private string? _systemInstruction;
        private GenerationConfig? _config;
        private FileData? _file;
        private IEnumerable<Tool>? _tools;

        private IEnumerable<SafetySetting>? _safetySettings;
        private IEnumerable<ImageData>? _images;
        private IEnumerable<FunctionResponse>? _functionResponses;
        private FunctionCallingMode _functionCallingMode;

        private List<Content>? _chatHistory;

        public ApiRequestBuilder WithFunctionResponses(IEnumerable<FunctionResponse> functionResponses)
        {
            if (functionResponses == null || !functionResponses.Any())
            {
                throw new ArgumentNullException(nameof(functionResponses), "Function calling results can't be null or empty.");
            }
            _functionResponses = functionResponses;
            return this;
        }

        public ApiRequestBuilder WithSystemInstruction(string systemInstruction)
        {
            if (string.IsNullOrEmpty(systemInstruction))
            {
                throw new ArgumentNullException(nameof(systemInstruction), "System instruction can't be an empty string.");
            }

            _systemInstruction = systemInstruction.Trim();
            return this;
        }

        public ApiRequestBuilder WithGenerationConfig(GenerationConfig config)
        {
            if (config.Temperature < 0.0F || config.Temperature > 2.0F)
            {
                throw new ArgumentOutOfRangeException(nameof(config), "Temperature must be between 0.0 and 2.0.");
            }

            _config = config;
            return this;
        }

        public ApiRequestBuilder WithTools(ToolBuilder toolBuilder)
        {
            _tools = toolBuilder.Build();
            return this;
        }

        public ApiRequestBuilder SetFunctionCallingMode(FunctionCallingMode mode)
        {
            _functionCallingMode = mode;
            return this;
        }

        public ApiRequestBuilder WithSafetySettings(IEnumerable<SafetySetting> safetySettings)
        {
            _safetySettings = safetySettings;
            return this;
        }

        public ApiRequestBuilder DisableAllSafetySettings()
        {
            _safetySettings =
                [
                    new SafetySetting
                    {
                        Category = SafetySettingHarmCategory.DangerousContent.GetDescription(),
                    },
                    new SafetySetting
                    {
                        Category = SafetySettingHarmCategory.Harassment.GetDescription(),
                    },
                    new SafetySetting
                    {
                        Category = SafetySettingHarmCategory.CivicIntegrity.GetDescription(),
                    },
                    new SafetySetting
                    {
                        Category = SafetySettingHarmCategory.HateSpeech.GetDescription(),
                    },
                    new SafetySetting
                    {
                        Category = SafetySettingHarmCategory.SexuallyExplicit.GetDescription(),
                    },
                ];

            return this;
        }

        public ApiRequestBuilder WithDefaultGenerationConfig(float temperature = 1, int maxOutputTokens = 65536)
        {
            if (temperature < 0.0F || temperature > 2.0F)
            {
                throw new ArgumentOutOfRangeException(nameof(temperature), "Temperature must be between 0.0 and 2.0.");
            }

            if (maxOutputTokens < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxOutputTokens), "Max output tokens must be greater than 0.");
            }

            _config = new GenerationConfig
            {
                Temperature = temperature,
                MaxOutputTokens = maxOutputTokens
            };

            return this;
        }

        public ApiRequestBuilder WithPrompt(string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                throw new ArgumentNullException(nameof(prompt), "Prompt can't be an empty string.");
            }

            _prompt = prompt.Trim();
            return this;
        }

        public ApiRequestBuilder WithChatHistory(IEnumerable<ChatMessage> chatMessages)
        {
            if (chatMessages == null || !chatMessages.Any())
            {
                throw new ArgumentNullException(nameof(chatMessages), "Chat history can't be null or empty.");
            }

            _chatHistory = chatMessages
                .Select(message => new Content
                {
                    Parts =
                    [
                        new Part
                        {
                            Text = message.Content
                        }
                    ],
                    Role = message.Role.GetDescription(),
                })
                .ToList();

            return this;
        }

        public ApiRequestBuilder WithBase64Images(IEnumerable<string> base64Images)
        {
            _images = base64Images.Select(ImageHelper.Base64ToImageData);
            return this;
        }

        public ApiRequestBuilder WithImages(IEnumerable<string> filePaths)
        {
            _images = filePaths.Select(ImageHelper.ImagePathToBase64);
            return this;
        }

        public ApiRequestBuilder WithResponseSchema(object schema)
        {
            if (_config == null)
            {
                _config = new GenerationConfig();
            }

            _config.ResponseSchema = schema;
            _config.ResponseMimeType = ResponseMimeType.Json.GetDescription();
            return this;
        }

        public ApiRequestBuilder WithUploadedFile(string fileUri, MimeType mimeType)
        {
            _file = new FileData
            {
                MimeType = EnumHelper.GetDescription(mimeType),
                FileUri = fileUri
            };

            return this;
        }

        public ApiRequest Build()
        {
            var apiRequest = new ApiRequest
            {
                GenerationConfig = _config,
                SafetySettings = _safetySettings?.ToList(),
                Tools = _tools?.ToList(),
                ToolConfig = _tools != null && _tools.Any()
                    ? new ToolConfig
                    {
                        FunctionCallingConfig = new FunctionCallingConfig
                        {
                            Mode = _functionCallingMode,
                        }
                    }
                    : null
            };

            var contents = _chatHistory ?? [];

            if (_images != null && _images.Any())
            {
                contents.Add(new Content
                {
                    Parts = _images
                        .Select(i => new Part
                        {
                            InlineData = new InlineData
                            {
                                MimeType = i.MimeType.GetDescription(),
                                Data = i.Base64Data
                            }
                        })
                        .ToList(),
                    Role = Role.User.GetDescription(),
                });
            }

            if (_file != null)
            {
                contents.Add(new Content
                {
                    Parts = [ new Part
                    {
                        FileData = _file
                    } ],
                    Role = Role.User.GetDescription(),
                });
            }

            if (_functionResponses != null && _functionResponses.Any())
            {
                contents.Add(new Content
                {
                    Parts = [.. _functionResponses
                        .Select(c => new Part
                        {
                            FunctionResponse = c
                        })],
                    Role = Role.User.GetDescription(),
                });
            }

            if (!string.IsNullOrEmpty(_prompt))
            {
                contents.Add(new Content
                {
                    Parts =
                    [
                        new Part
                        {
                            Text = _prompt
                        }
                    ],
                    Role = Role.User.GetDescription(),
                });
            }

            apiRequest.Contents = contents;

            apiRequest.SystemInstruction = string.IsNullOrEmpty(_systemInstruction)
                ? null
                : new SystemInstruction
                {
                    Parts =
                    [
                        new Part
                        {
                            Text = _systemInstruction
                        }
                    ]
                };

            return apiRequest;
        }
    }
}
