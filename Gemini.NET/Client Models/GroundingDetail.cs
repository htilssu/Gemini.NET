namespace GeminiDotNET.ClientModels
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
        public IEnumerable<GroundingSource>? Sources { get; set; }

        /// <summary>
        /// The list of reliable information.
        /// </summary>
        public IEnumerable<string>? ReliableInformation { get; set; }

        /// <summary>
        /// The list of search suggestions.
        /// </summary>
        public IEnumerable<string>? SearchSuggestions { get; set; }
    }
}
