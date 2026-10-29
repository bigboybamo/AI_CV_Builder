using NewAI_CV_builder.Utilities;

namespace NewAI_CV_builder.Tests;

public class AiResponseParserTests
{
    [Test]
    public void ParseOpenAi_WhenReasoningPrecedesMessage_ReturnsMessageText()
    {
        const string json = """
            {
              "status": "completed",
              "output": [
                { "type": "reasoning", "summary": [] },
                {
                  "type": "message",
                  "content": [
                    { "type": "output_text", "text": "Tailored resume JSON" }
                  ]
                }
              ]
            }
            """;

        var result = AiResponseParser.ParseOpenAi(json);

        Assert.That(result, Is.EqualTo("Tailored resume JSON"));
    }

    [Test]
    public void ParseOpenAi_WhenResponseIsIncomplete_ReturnsError()
    {
        const string json = """
            {
              "status": "incomplete",
              "incomplete_details": { "reason": "max_output_tokens" },
              "output_text": "Partial resume that must not be used",
              "output": []
            }
            """;

        var result = AiResponseParser.ParseOpenAi(json);

        Assert.That(result, Does.StartWith("Error:"));
        Assert.That(result, Does.Contain("max_output_tokens"));
        Assert.That(result, Does.Not.Contain("Partial resume"));
    }

    [Test]
    public void ParseOpenAi_WhenContentIsARefusal_ReturnsError()
    {
        const string json = """
            {
              "status": "completed",
              "output": [
                {
                  "type": "message",
                  "content": [
                    { "type": "refusal", "refusal": "I cannot help with that." }
                  ]
                }
              ]
            }
            """;

        var result = AiResponseParser.ParseOpenAi(json);

        Assert.That(result, Does.StartWith("Error:"));
        Assert.That(result, Does.Contain("refused"));
    }

    [Test]
    public void ParseOpenAi_WhenExpectedTextIsMissing_DoesNotReturnRawJson()
    {
        const string json = """{"status":"completed","output":[{"type":"reasoning"}]}""";

        var result = AiResponseParser.ParseOpenAi(json);

        Assert.That(result, Does.StartWith("Error:"));
        Assert.That(result, Is.Not.EqualTo(json));
    }

    [Test]
    public void ParseClaude_WhenThinkingPrecedesText_ReturnsOnlyTextBlocks()
    {
        const string json = """
            {
              "stop_reason": "end_turn",
              "content": [
                { "type": "thinking", "thinking": "Internal reasoning" },
                { "type": "text", "text": "First part" },
                { "type": "text", "text": "Second part" }
              ]
            }
            """;

        var result = AiResponseParser.ParseClaude(json);

        Assert.That(result, Is.EqualTo($"First part{Environment.NewLine}Second part"));
        Assert.That(result, Does.Not.Contain("Internal reasoning"));
    }

    [Test]
    public void ParseClaude_WhenMaxTokensReached_ReturnsErrorInsteadOfPartialText()
    {
        const string json = """
            {
              "stop_reason": "max_tokens",
              "content": [{ "type": "text", "text": "Partial resume" }]
            }
            """;

        var result = AiResponseParser.ParseClaude(json);

        Assert.That(result, Does.StartWith("Error:"));
        Assert.That(result, Does.Contain("truncated"));
        Assert.That(result, Does.Not.Contain("Partial resume"));
    }

    [Test]
    public void ParseClaude_WhenRefused_ReturnsClearError()
    {
        const string json = """
            {
              "stop_reason": "refusal",
              "stop_details": { "category": "general_harms" },
              "content": []
            }
            """;

        var result = AiResponseParser.ParseClaude(json);

        Assert.That(result, Does.StartWith("Error:"));
        Assert.That(result, Does.Contain("general_harms"));
    }

    [TestCase("OpenAI", "not-json")]
    [TestCase("Claude", "not-json")]
    public void Parse_WhenJsonIsInvalid_ReturnsError(string provider, string json)
    {
        var result = provider == "OpenAI"
            ? AiResponseParser.ParseOpenAi(json)
            : AiResponseParser.ParseClaude(json);

        Assert.That(result, Does.StartWith("Error:"));
        Assert.That(result, Does.Not.Contain(json));
    }
}
