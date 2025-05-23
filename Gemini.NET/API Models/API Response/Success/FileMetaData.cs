using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Response.Success
{
    public class FileMetaData
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("displayName")]
        public string DisplayName;

        [JsonProperty("mimeType")]
        public string MimeType;

        [JsonProperty("sizeBytes")]
        public string SizeBytes;

        [JsonProperty("createTime")]
        public DateTime CreateTime;

        [JsonProperty("updateTime")]
        public DateTime UpdateTime;

        [JsonProperty("expirationTime")]
        public string ExpirationTime;

        [JsonProperty("sha256Hash")]
        public string Sha256Hash;

        [JsonProperty("uri")]
        public string Uri;

        [JsonProperty("state")]
        public string State;

        [JsonProperty("source")]
        public string Source;
    }
}
