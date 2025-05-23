namespace GeminiDotNET.ApiModels.Enums
{
    public enum FunctionCallingMode
    {
        /// <summary>
        /// The model decides whether to generate a natural language response or suggest a function call based on the prompt and context. This is the most flexible mode and recommended for most scenarios.
        /// </summary>
        AUTO,

        /// <summary>
        /// The model is constrained to always predict a function call and guarantee function schema adherence.
        /// </summary>
        ANY,

        /// <summary>
        /// The model is prohibited from making function calls. This is equivalent to sending a request without any function declarations. Use this to temporarily disable function calling without removing your tool definitions.
        /// </summary>
        NONE,
    }
}
