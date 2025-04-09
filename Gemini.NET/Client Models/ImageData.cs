using GeminiDotNET.API_Models.Enums;

namespace GeminiDotNET.Client_Models
{
    /// <summary>
    /// Represents an image file that is sent as inline data
    /// </summary>
    public class ImageData
    {
        /// <summary>
        /// The media type of the file specified in the Inline Data
        /// </summary>
        public required MimeType MimeType { get; set; }

        /// <summary>
        /// The base64 encoded data of the image
        /// </summary>
        public required string Base64Data { get; set; }
    }
}
