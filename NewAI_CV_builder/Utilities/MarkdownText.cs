using System.Text.RegularExpressions;

namespace NewAI_CV_builder.Utilities
{
    public static class MarkdownText
    {
        public static string StripCodeFence(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var text = input.Trim();

            if (!text.Contains("```"))
                return text;

            text = Regex.Replace(text, @"^\s*```[a-zA-Z0-9_-]*\s*\r?\n", string.Empty);
            text = Regex.Replace(text, @"\r?\n\s*```\s*$", string.Empty);

            return text.Trim();
        }
    }
}
