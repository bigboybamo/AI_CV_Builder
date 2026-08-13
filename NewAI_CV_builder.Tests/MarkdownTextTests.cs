using NewAI_CV_builder.Utilities;

namespace NewAI_CV_builder.Tests;

public class MarkdownTextTests
{
    [Test]
    public void StripCodeFence_WhenInputHasJsonFence_ReturnsInnerText()
    {
        //Arrange
        const string input = """
            ```json
            {"name":"Ola"}
            ```
            """;

        //Act
        var result = MarkdownText.StripCodeFence(input);

        //Assert
        Assert.That(result, Is.EqualTo("{\"name\":\"Ola\"}"));
    }

    [Test]
    public void StripCodeFence_WhenInputHasUppercaseFence_ReturnsInnerText()
    {
        //Arrange
        const string input = """
            ```JSON
            {"skills":[]}
            ```
            """;

        //Act
        var result = MarkdownText.StripCodeFence(input);

        //Assert
        Assert.That(result, Is.EqualTo("{\"skills\":[]}"));
    }

    [Test]
    public void StripCodeFence_WhenInputHasNoFence_TrimsAndReturnsInput()
    {
        //Arrange
        const string input = "  plain output  ";

        //Act
        var result = MarkdownText.StripCodeFence(input);

        //Assert
        Assert.That(result, Is.EqualTo("plain output"));
    }

    [Test]
    public void StripCodeFence_WhenInputIsWhitespace_ReturnsEmptyString()
    {
        //Arrange
        const string input = "   ";

        //Act
        var result = MarkdownText.StripCodeFence(input);

        //Assert
        Assert.That(result, Is.Empty);
    }
}
