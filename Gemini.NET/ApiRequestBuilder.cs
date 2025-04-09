using Gemini.NET.API_Models.API_Request;
using GeminiDotNET.API_Models.API_Request;
using GeminiDotNET.API_Models.Enums;
using GeminiDotNET.Client_Models;
using GeminiDotNET.Helpers;
using Models.Enums;
using Models.Request;
using Models.Shared;

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
        private bool _useGrounding = false;
        private IEnumerable<SafetySetting>? _safetySettings;
        private List<Content>? _chatHistory;
        private IEnumerable<ImageData>? _images;
        private FileData? _file;

        /// <summary>
        /// Sets the system instruction for the API request.
        /// </summary>
        /// <param name="systemInstruction">The system instruction to set.</param>
        /// <returns>The current instance of <see cref="ApiRequestBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the system instruction is null or empty.</exception>
        public ApiRequestBuilder WithSystemInstruction(string systemInstruction)
        {
            if (string.IsNullOrEmpty(systemInstruction))
            {
                throw new ArgumentNullException(nameof(systemInstruction), "System instruction can't be an empty string.");
            }

            _systemInstruction = systemInstruction.Trim();
            return this;
        }

        /// <summary>
        /// Sets the generation configuration for the API request.
        /// </summary>
        /// <param name="config">The generation configuration to set.</param>
        /// <returns>The current instance of <see cref="ApiRequestBuilder"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the temperature is out of the valid range (0.0 to 2.0).</exception>
        public ApiRequestBuilder WithGenerationConfig(GenerationConfig config)
        {
            if (config.Temperature < 0.0F || config.Temperature > 2.0F)
            {
                throw new ArgumentOutOfRangeException(nameof(config), "Temperature must be between 0.0 and 2.0.");
            }

            _config = config;
            return this;
        }

        /// <summary>
        /// Enables grounding for the API request.
        /// </summary>
        /// <returns>The current instance of <see cref="ApiRequestBuilder"/>.</returns>
        public ApiRequestBuilder EnableGrounding()
        {
            _useGrounding = true;
            return this;
        }

        /// <summary>
        /// Sets the safety settings for the API request.
        /// </summary>
        /// <param name="safetySettings">The list of safety settings to set.</param>
        /// <returns>The current instance of <see cref="ApiRequestBuilder"/>.</returns>
        public ApiRequestBuilder WithSafetySettings(IEnumerable<SafetySetting> safetySettings)
        {
            _safetySettings = safetySettings;
            return this;
        }

        /// <summary>
        /// Disables all safety settings for the API request.
        /// </summary>
        /// <returns>The current instance of <see cref="ApiRequestBuilder"/>.</returns>
        public ApiRequestBuilder DisableAllSafetySettings()
        {
            _safetySettings =
                [
                    new SafetySetting
                    {
                        Category = EnumHelper.GetDescription(SafetySettingHarmCategory.DangerousContent),
                    },
                    new SafetySetting
                    {
                        Category = EnumHelper.GetDescription(SafetySettingHarmCategory.Harassment),
                    },
                    new SafetySetting
                    {
                        Category = EnumHelper.GetDescription(SafetySettingHarmCategory.CivicIntegrity),
                    },
                    new SafetySetting
                    {
                        Category = EnumHelper.GetDescription(SafetySettingHarmCategory.HateSpeech),
                    },
                    new SafetySetting
                    {
                        Category = EnumHelper.GetDescription(SafetySettingHarmCategory.SexuallyExplicit),
                    },
                ];

            return this;
        }

        /// <summary>
        /// Sets the default generation configuration for the API request.
        /// </summary>
        /// <param name="temperature">The sampling temperature to set.</param>
        /// <param name="maxOutputTokens">The maximum number of tokens in the generated output.</param>
        /// <returns>The current instance of <see cref="ApiRequestBuilder"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the temperature is out of the valid range (0.0 to 2.0).</exception>
        public ApiRequestBuilder WithDefaultGenerationConfig(float temperature = 1, int maxOutputTokens = 8192)
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

        /// <summary>
        /// Sets the prompt for the API request.
        /// </summary>
        /// <param name="prompt">The prompt to set.</param>
        /// <returns>The current instance of <see cref="ApiRequestBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the prompt is null or empty.</exception>
        public ApiRequestBuilder WithPrompt(string prompt)
        {
            if (string.IsNullOrEmpty(prompt))
            {
                throw new ArgumentNullException(nameof(prompt), "Prompt can't be an empty string.");
            }

            _prompt = prompt.Trim();
            return this;
        }

        /// <summary>
        /// Sets the chat history for the API request.
        /// </summary>
        /// <param name="chatMessages"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
                    Role = EnumHelper.GetDescription(message.Role),
                })
                .ToList();

            return this;
        }

        /// <summary>
        /// Sets the Base64 images for the API request.
        /// </summary>
        /// <returns></returns>
        public ApiRequestBuilder WithBase64Images(IEnumerable<string> images)
        {
            _images = images.Select(ImageHelper.AsImageData);
            return this;
        }

        /// <summary>
        /// Sets the response schema for the API request that strictly follows the OpenAPI specification.
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        public ApiRequestBuilder WithResponseSchema(object schema)
        {
            if (_config == null)
            {
                _config = new GenerationConfig();
            }

            _config.ResponseSchema = schema;
            _config.ResponseMimeType = EnumHelper.GetDescription(ResponseMimeType.Json);
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

        /// <summary>
        /// Builds the API request with the configured parameters.
        /// </summary>
        /// <returns>The constructed <see cref="ApiRequest"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the prompt is null or empty.</exception>
        public ApiRequest Build()
        {
            if (string.IsNullOrEmpty(_prompt))
            {
                throw new ArgumentNullException(nameof(_prompt), "Prompt can't be an empty string.");
            }

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
                                MimeType = EnumHelper.GetDescription(i.MimeType),
                                Data = i.Base64Data
                            }
                        })
                        .ToList(),
                    Role = EnumHelper.GetDescription(Role.User),
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
                    Role = EnumHelper.GetDescription(Role.User),
                });
            }

            contents.Add(new Content
            {
                Parts =
                [
                    new Part
                    {
                        Text = _prompt
                    }
                ],
                Role = EnumHelper.GetDescription(Role.User),
            });

            return new ApiRequest
            {
                Contents = contents,
                GenerationConfig = _config,
                SafetySettings = _safetySettings?.ToList(),
                Tools = _useGrounding
                    ?
                    [
                            new Tool
                            {
                                GoogleSearch = new GoogleSearch()
                            }
                    ]
                    : null,
                SystemInstruction = string.IsNullOrEmpty(_systemInstruction)
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
                    }
            };
        }
    }
}
