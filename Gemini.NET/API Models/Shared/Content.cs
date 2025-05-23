using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Shared
{
    /// <summary>
    /// Represents a content block in the API request/response, consisting of a role and message parts.
    /// Content blocks are used to structure conversations and provide context for the model's responses.
    /// Each content block represents a single turn in the conversation.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// The role of the content (e.g., "user" or "model").
        /// This is a required field that defines the source or purpose of the content.
        /// The role helps maintain conversation structure and context.
        /// </summary>
        [JsonProperty("role")]
        public required string Role { get; set; }

        /// <summary>
        /// The list of message parts that make up this content block.
        /// This is a required field that contains the actual content, such as text or other data.
        /// Multiple parts can be combined to form a complete message.
        /// </summary>
        [JsonProperty("parts")]
        public required List<Part> Parts { get; set; }
    }
}
