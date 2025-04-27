using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Response.Success
{
    public class FileUploaderResponse
    {
        [JsonProperty("files")]
        public List<FileMetaData>? Files;

        [JsonProperty("file")]
        public FileMetaData? File;
    }
}
