using GeminiDotNET.ApiModels.Response.Success.FunctionCalling;

namespace GeminiDotNET.ClientModels
{
    /// <summary>
    /// The response with a result and optional grounding details.
    /// </summary>
    public class ModelResponse
    {
        /// <summary>
        /// The response content of Gemini.
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// The grounding details of the response.
        /// </summary>
        public GroundingDetail? GroundingDetail { get; set; }

        /// <summary>
        /// The information of the function to call
        /// </summary>
        public List<FunctionCall>? FunctionCalls { get; set; }

        /// <summary>
        /// The information of the function responses
        /// </summary>
        public List<FunctionResponse>? FunctionResponses { get; set; }
    }
}
