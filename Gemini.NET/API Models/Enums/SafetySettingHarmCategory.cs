using System.ComponentModel;

namespace GeminiDotNET.ApiModels.Enums
{
    /// <summary>
    /// The harm categories for content safety filtering.
    /// These categories define the types of potentially harmful content that can be filtered
    /// by the model's safety settings.
    /// </summary>
    public enum SafetySettingHarmCategory : sbyte
    {
        /// <summary>
        /// Dangerous content category - filters content that could cause physical or mental harm.
        /// Includes instructions for dangerous activities, self-harm, or illegal actions.
        /// Use this category to prevent generation of potentially harmful instructions or advice.
        /// </summary>
        [Description("HARM_CATEGORY_DANGEROUS_CONTENT")]
        DangerousContent,

        /// <summary>
        /// Harassment category - filters content that could constitute harassment or bullying.
        /// Includes personal attacks, hostile behavior, or intimidation.
        /// Use this category to maintain respectful and professional interactions.
        /// </summary>
        [Description("HARM_CATEGORY_HARASSMENT")]
        Harassment,

        /// <summary>
        /// Hate speech category - filters discriminatory or hateful content.
        /// Includes content targeting protected characteristics or promoting extremist views.
        /// Use this category to prevent generation of discriminatory or hostile content.
        /// </summary>
        [Description("HARM_CATEGORY_HATE_SPEECH")]
        HateSpeech,

        /// <summary>
        /// Sexually explicit content category - filters adult or inappropriate sexual content.
        /// Includes explicit sexual material or inappropriate sexual references.
        /// Use this category to maintain appropriate content standards.
        /// </summary>
        [Description("HARM_CATEGORY_SEXUALLY_EXPLICIT")]
        SexuallyExplicit,

        /// <summary>
        /// Civic integrity category - filters content that could undermine civic processes.
        /// Includes misinformation about elections, civic processes, or democratic institutions.
        /// Use this category to prevent generation of content that could harm civic discourse.
        /// </summary>
        [Description("HARM_CATEGORY_CIVIC_INTEGRITY")]
        CivicIntegrity,
    }
}
