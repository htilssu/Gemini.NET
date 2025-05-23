using GeminiDotNET.ApiModels.Enums;

namespace GeminiDotNET.ClientModels
{
    public class ChatMessage
    {
        public required Role Role { get; set; }
        public required string Content { get; set; }
    }
}
