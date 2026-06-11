namespace NewAI_CV_builder.Utilities
{
    public class UpworkProposalRequest
    {
        public required string JobDescription { get; init; }
        public required string JobType { get; init; }
        public string? LoomUrl { get; init; }
        public IEnumerable<string>? RuntimeRules { get; init; }
        public IEnumerable<ProjectHighlight>? ProjectHighlights { get; init; }
    }
}
