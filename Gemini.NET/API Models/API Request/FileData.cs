using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.ApiRequest
{
    public class FileData
    {
        [JsonProperty("mime_type")]
        public required string MimeType { get; set; }

        [JsonProperty("file_uri")]
        public required string FileUri { get; set; }
    }
}
