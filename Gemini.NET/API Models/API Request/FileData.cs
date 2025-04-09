using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Request
{
    public class FileData
    {
        [JsonProperty("mime_type")]
        public required string MimeType { get; set; }

        [JsonProperty("file_uri")]
        public required string FileUri { get; set; }
    }
}
