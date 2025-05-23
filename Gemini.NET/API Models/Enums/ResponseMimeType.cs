using System.ComponentModel;

namespace GeminiDotNET.ApiModels.Enums
{
    /// <summary>
    /// Enum representing different MIME types for model responses.
    /// Defines the format in which the model should return its generated content,
    /// allowing for flexible response handling based on application needs.
    /// </summary>
    public enum ResponseMimeType : sbyte
    {
        /// <summary>
        /// JSON response MIME type (application/json).
        /// Use this when the model's response should be structured data.
        /// Suitable for responses that need to be parsed and processed programmatically.
        /// </summary>
        [Description("application/json")]
        Json,

        /// <summary>
        /// Plain text response MIME type (text/plain).
        /// Use this for unformatted text responses.
        /// Suitable for human-readable content or when formatting is not needed.
        /// </summary>
        [Description("text/plain")]
        PlainText
    }
}
