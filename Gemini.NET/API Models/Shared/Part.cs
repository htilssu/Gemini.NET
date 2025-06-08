using GeminiDotNET.ApiModels.ApiRequest;
using GeminiDotNET.ApiModels.Response.Success.FunctionCalling;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Shared
{
    /// <summary>
    /// Represents a single part of a content block in the API request/response.
    /// Parts are the basic building blocks of messages in the conversation,
    /// allowing for structured content delivery in the API communication.
    /// </summary>
    public class Part
    {
        /// <summary>
        /// The text content of this part.
        /// </summary>
        [JsonProperty("text")]
        public string? Text { get; set; }

        /// <summary>
        /// (Optional) The inline data for this part, contains the media content
        /// </summary>
        [JsonProperty("inline_data")]
        public InlineData? InlineData { get; set; }

        /// <summary>
        /// (Optional) The file data for this part, contains the media content
        /// </summary>
        [JsonProperty("file_data")]
        public FileData? FileData { get; set; }

        /// <summary>
        /// The function call name and arguments
        /// </summary>
        [JsonProperty("functionCall")]
        public FunctionCall? FunctionCall { get; set; }

        /// <summary>
        /// The response returned by the function.
        /// </summary>
        [JsonProperty("functionResponse")]
        public FunctionResponse? FunctionResponse { get; set; }
    }
}
