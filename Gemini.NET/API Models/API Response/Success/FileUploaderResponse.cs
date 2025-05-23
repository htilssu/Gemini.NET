using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class FileUploaderResponse
    {
        [JsonProperty("files")]
        public List<FileMetaData>? Files;

        [JsonProperty("file")]
        public FileMetaData? File;
    }
}
