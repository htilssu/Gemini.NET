using GeminiDotNET.ApiModels.Shared;
using GeminiDotNET.Helpers;
using Newtonsoft.Json;

namespace GeminiDotNET.ApiModels.Request
{
    /// <summary>
    /// Represents system-level instructions that guide the model's behavior.
    /// System instructions can be used to set context, define roles, or establish constraints
    /// for the model's responses.
    /// </summary>
    public class SystemInstruction
    {
        /// <summary>
        /// The role for this instruction (e.g., user, assistant).
        /// Default value is "user".
        /// </summary>
        [JsonProperty("role")]
        public string Role { get; set; } = Enums.Role.User.GetDescription();

        /// <summary>
        /// The list of content parts that make up the system instruction.
        /// This is a required field and must contain at least one part.
        /// </summary>
        [JsonProperty("parts")]
        public required List<Part> Parts { get; set; }
    }
}
