using Newtonsoft.Json;

namespace Models.Request
{
    public class ResponseSchema(string type)
    {
        [JsonProperty("type")]
        public string Type { get; set; } = type;

        [JsonProperty("properties")]
        public Dictionary<string, ResponseSchema>? Properties { get; set; }

        [JsonProperty("required")]
        public List<string>? Required { get; set; }

        [JsonProperty("items")]
        public ResponseSchema? Items { get; set; }

        [JsonProperty("enum")]
        public List<string>? Enum { get; set; }

        [JsonProperty("default")]
        public object? Default { get; set; }

        public ResponseSchema() : this("object")
        {
        }
    }
}
