using System.Text.Json;

namespace NewAI_CV_builder.Utilities
{
    public static class AiResponseParser
    {
        public static string ParseOpenAi(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (TryGetNonEmptyString(root, "status", out var status) &&
                    !string.Equals(status, "completed", StringComparison.OrdinalIgnoreCase))
                {
                    var reason = GetNestedString(root, "incomplete_details", "reason")
                        ?? GetNestedString(root, "error", "message")
                        ?? status;

                    return $"Error: OpenAI response was {status} ({reason}).";
                }

                if (TryGetNonEmptyString(root, "output_text", out var outputText))
                    return outputText;

                if (!root.TryGetProperty("output", out var output) || output.ValueKind != JsonValueKind.Array)
                    return "Error: OpenAI response did not contain any output.";

                var textBlocks = new List<string>();

                foreach (var item in output.EnumerateArray())
                {
                    if (!TryGetNonEmptyString(item, "type", out var itemType) || itemType != "message")
                        continue;

                    if (!item.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
                        continue;

                    foreach (var block in content.EnumerateArray())
                    {
                        TryGetNonEmptyString(block, "type", out var blockType);

                        if (blockType == "refusal")
                        {
                            var refusal = TryGetNonEmptyString(block, "refusal", out var refusalText)
                                ? refusalText
                                : "The model declined the request";

                            return $"Error: OpenAI refused the request. {refusal}";
                        }

                        if (blockType == "output_text" && TryGetNonEmptyString(block, "text", out var text))
                            textBlocks.Add(text);
                    }
                }

                return textBlocks.Count > 0
                    ? string.Join(Environment.NewLine, textBlocks)
                    : "Error: OpenAI response did not contain a text message.";
            }
            catch (JsonException)
            {
                return "Error: OpenAI returned an invalid JSON response.";
            }
        }

        public static string ParseClaude(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (TryGetNonEmptyString(root, "stop_reason", out var stopReason))
                {
                    if (stopReason == "max_tokens")
                        return "Error: Claude response was truncated because the maximum token limit was reached.";

                    if (stopReason == "refusal")
                    {
                        var category = GetNestedString(root, "stop_details", "category");
                        return category is null
                            ? "Error: Claude refused the request."
                            : $"Error: Claude refused the request ({category}).";
                    }
                }

                if (!root.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
                    return "Error: Claude response did not contain any content.";

                var textBlocks = new List<string>();

                foreach (var block in content.EnumerateArray())
                {
                    if (TryGetNonEmptyString(block, "type", out var blockType) &&
                        blockType == "text" &&
                        TryGetNonEmptyString(block, "text", out var text))
                    {
                        textBlocks.Add(text);
                    }
                }

                return textBlocks.Count > 0
                    ? string.Join(Environment.NewLine, textBlocks)
                    : "Error: Claude response did not contain a text block.";
            }
            catch (JsonException)
            {
                return "Error: Claude returned an invalid JSON response.";
            }
        }

        private static bool TryGetNonEmptyString(JsonElement element, string propertyName, out string value)
        {
            value = string.Empty;

            if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.String)
                return false;

            value = property.GetString() ?? string.Empty;
            return !string.IsNullOrWhiteSpace(value);
        }

        private static string? GetNestedString(JsonElement element, string objectName, string propertyName)
        {
            return element.TryGetProperty(objectName, out var nested) &&
                   nested.ValueKind == JsonValueKind.Object &&
                   TryGetNonEmptyString(nested, propertyName, out var value)
                ? value
                : null;
        }
    }
}
