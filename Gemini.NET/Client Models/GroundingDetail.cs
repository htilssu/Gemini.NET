namespace GeminiDotNET.Client_Models
{
    /// <summary>
    /// The details of grounding information.
    /// </summary>
    public class GroundingDetail
    {
        /// <summary>
        /// The rendered content as HTML.
        /// </summary>
        public required string? RenderedContentAsHtml { get; set; }

        /// <summary>
        /// The list of grounding sources.
        /// </summary>
        public List<GroundingSource>? Sources { get; set; }

        /// <summary>
        /// The list of reliable information.
        /// </summary>
        public List<string>? ReliableInformation { get; set; }

        /// <summary>
        /// The list of search suggestions.
        /// </summary>
        public List<string>? SearchSuggestions { get; set; }
    }
}
