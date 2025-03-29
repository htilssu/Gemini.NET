using Newtonsoft.Json;

namespace Gemini.NET.API_Models.API_Request
{
    public class OpenApiSchema
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "object";

        [JsonProperty("properties")]
        public Dictionary<string, OpenApiProperty> Properties { get; set; }

        public class OpenApiProperty
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("enum")]
            public List<string>? EnumValues { get; set; } // Hỗ trợ enum

            [JsonProperty("items")]
            public OpenApiSchema? Items { get; set; } // Dùng cho nested object hoặc danh sách
            public bool IsArray { get; set; } // Đánh dấu nếu là List<T>
            public bool IsNullable { get; set; } // Đánh dấu nếu là nullable (int?, decimal?, ...)
        }
    }
}
