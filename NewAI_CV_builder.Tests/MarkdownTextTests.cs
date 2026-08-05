using NewAI_CV_builder.Utilities;

namespace NewAI_CV_builder.Tests;

public class MarkdownTextTests
{
    [Test]
    public void StripCodeFence_WhenInputHasJsonFence_ReturnsInnerText()
    {
        var result = MarkdownText.StripCodeFence("""
            ```json
            {"name":"Ola"}
            ```
            """);

        Assert.That(result, Is.EqualTo("{\"name\":\"Ola\"}"));
    }

    [Test]
    public void StripCodeFence_WhenInputHasUppercaseFence_ReturnsInnerText()
    {
        var result = MarkdownText.StripCodeFence("""
            ```JSON
            {"skills":[]}
            ```
            """);

        Assert.That(result, Is.EqualTo("{\"skills\":[]}"));
    }

    [Test]
    public void StripCodeFence_WhenInputHasNoFence_TrimsAndReturnsInput()
    {
        var result = MarkdownText.StripCodeFence("  plain output  ");

        Assert.That(result, Is.EqualTo("plain output"));
    }

    [Test]
    public void StripCodeFence_WhenInputIsWhitespace_ReturnsEmptyString()
    {
        var result = MarkdownText.StripCodeFence("   ");

        Assert.That(result, Is.Empty);
    }
}
