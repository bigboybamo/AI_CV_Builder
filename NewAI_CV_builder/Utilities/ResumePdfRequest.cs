namespace NewAI_CV_builder.Utilities
{
    public class ResumePdfRequest
    {
        public required string ResumeJson { get; init; }
        public required string OutputPdfFilePath { get; init; }
        public bool Headless { get; init; } = true;
        public string? AccentColor { get; init; }
    }
}
