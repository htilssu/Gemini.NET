namespace GeminiDotNET.ClientModels
{
    /// <summary>
    /// Represents a grounding source with a domain and URL.
    /// </summary>
    public class GroundingSource
    {
        /// <summary>
        /// The domain of the grounding source.
        /// </summary>
        public required string Domain { get; set; }

        /// <summary>
        /// The URL of the grounding source.
        /// </summary>
        public required string Url { get; set; }
    }
}
