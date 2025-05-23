using System.ComponentModel;

namespace GeminiDotNET.ApiModels.Enums
{
    /// <summary>
    /// The role in the conversation with the model.
    /// Roles define the source and purpose of messages in the API communication,
    /// helping to maintain a clear conversation structure.
    /// </summary>
    public enum Role : sbyte
    {
        /// <summary>
        /// User role - represents messages from the human user.
        /// Used for input messages, questions, or commands sent to the model.
        /// </summary>
        [Description("user")]
        User,

        /// <summary>
        /// Model role - represents responses from the AI model.
        /// Used for generated content, answers, or any output from the model.
        /// </summary>
        [Description("model")]
        Model
    }
}
