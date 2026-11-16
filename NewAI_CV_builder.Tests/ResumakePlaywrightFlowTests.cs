using NewAI_CV_builder.Services;
using NewAI_CV_builder.Utilities;
using System.Text.Json;

namespace NewAI_CV_builder.Tests;

public class ResumakePlaywrightFlowTests
{
    [Test]
    public void GeneratePdfFromJsonAsync_WhenJsonIsInvalid_ThrowsArgumentException()
    {
        //Arrange
        var outputPath = NewOutputPath();

        //Act
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(NewRequest(InvalidResumeJson, outputPath)));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception?.ParamName, Is.EqualTo("resumeJson"));
            Assert.That(exception?.Message, Does.Contain("not valid JSON"));
        });
    }

    [Test]
    public void GeneratePdfFromJsonAsync_WhenTokenIsAlreadyCancelled_ThrowsOperationCanceledException()
    {
        //Arrange
        var outputPath = NewOutputPath();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        //Act
        var exception = Assert.ThrowsAsync<OperationCanceledException>(() =>
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(NewRequest(ValidResumeJson, outputPath), cts.Token));

        //Assert
        Assert.That(exception, Is.Not.Null);
    }

    [Test]
    public void GeneratePdfFromJsonAsync_WhenCancelled_DoesNotWriteAPdf()
    {
        //Arrange
        var outputPath = NewOutputPath();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        //Act
        Assert.ThrowsAsync<OperationCanceledException>(() =>
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(NewRequest(ValidResumeJson, outputPath), cts.Token));

        //Assert
        Assert.That(File.Exists(outputPath), Is.False);
    }

    [Test]
    public void GeneratePdfFromJsonAsync_WhenCancelledAndJsonIsInvalid_ReportsCancellationNotAJsonError()
    {
        // Cancellation is checked ahead of the parse, so a user who pressed Stop is not
        // shown a JSON validation error for work that never ran.

        //Arrange
        var outputPath = NewOutputPath();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        //Act
        var exception = Assert.ThrowsAsync<OperationCanceledException>(() =>
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(NewRequest(InvalidResumeJson, outputPath), cts.Token));

        //Assert
        Assert.That(exception, Is.Not.Null);
    }

    [Test]
    public void GeneratePdfFromJsonAsync_WhenNotCancelled_DoesNotObserveTheToken()
    {
        // A live token must not short-circuit the flow: the invalid JSON still surfaces.

        //Arrange
        var outputPath = NewOutputPath();
        using var cts = new CancellationTokenSource();

        //Act
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(NewRequest(InvalidResumeJson, outputPath), cts.Token));

        //Assert
        Assert.That(exception?.ParamName, Is.EqualTo("resumeJson"));
    }

    [Test]
    public void GeneratePdfFromJsonAsync_WhenAccentColorIsInvalid_ThrowsArgumentException()
    {
        //Arrange
        var outputPath = NewOutputPath();

        //Act
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(
                NewRequest(ValidResumeJson, outputPath, accentColor: "red; } body { display: none")));

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Does.Contain("hex"));
            Assert.That(File.Exists(outputPath), Is.False);
        });
    }

    [Test]
    public void RenderHtml_WhenNoAccentColor_UsesDefaultBlue()
    {
        //Arrange
        using var document = JsonDocument.Parse(StyledResumeJson);

        //Act
        var html = ResumakePlaywrightFlow.RenderHtml(document.RootElement);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(html, Does.Contain("a { color: #1f6feb;"));
            Assert.That(html, Does.Contain("border-bottom: 2.5px solid #1f6feb;"));
            Assert.That(html, Does.Contain("color: #1f6feb; margin: 0 0 5px;"));
            Assert.That(html, Does.Contain("background: #eef4ff; color: #1f4e8c;"));
        });
    }

    [Test]
    public void RenderHtml_WhenNoAccentColor_MatchesExplicitDefault()
    {
        //Arrange
        using var document = JsonDocument.Parse(StyledResumeJson);

        //Act
        var implicitDefault = ResumakePlaywrightFlow.RenderHtml(document.RootElement);
        var explicitDefault = ResumakePlaywrightFlow.RenderHtml(document.RootElement, "#1F6FEB");

        //Assert
        Assert.That(implicitDefault, Is.EqualTo(explicitDefault));
    }

    [Test]
    public void RenderHtml_WhenAccentColorGiven_AppliesItToLinksHeadersAndUnderline()
    {
        //Arrange
        using var document = JsonDocument.Parse(StyledResumeJson);

        //Act
        var html = ResumakePlaywrightFlow.RenderHtml(document.RootElement, "#B91C1C");

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(html, Does.Contain("a { color: #b91c1c;"));
            Assert.That(html, Does.Contain("border-bottom: 2.5px solid #b91c1c;"));
            Assert.That(html, Does.Contain("color: #b91c1c; margin: 0 0 5px;"));
            Assert.That(html, Does.Not.Contain("#1f6feb"));
        });
    }

    [Test]
    public void RenderHtml_WhenAccentColorGiven_DerivesSkillTagColours()
    {
        //Arrange
        using var document = JsonDocument.Parse(StyledResumeJson);
        var palette = ResumeAccentColor.PaletteFor("#b91c1c");

        //Act
        var html = ResumakePlaywrightFlow.RenderHtml(document.RootElement, "#b91c1c");

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(html, Does.Contain($"background: {palette.TagBackground}; color: {palette.TagText};"));
            Assert.That(html, Does.Not.Contain("#eef4ff"));
            Assert.That(html, Does.Not.Contain("#1f4e8c"));
        });
    }

    private const string InvalidResumeJson = "{ invalid json";

    private const string StyledResumeJson = """
        {
          "basics": { "name": "Test Person", "email": "test@example.com", "website": "https://linkedin.com/in/test" },
          "skills": [ { "name": "Languages", "keywords": [ "C#", "Python" ] } ]
        }
        """;

    private const string ValidResumeJson = """
        { "basics": { "name": "Test Person", "email": "test@example.com" } }
        """;

    private static string NewOutputPath() => Path.Combine(
        TestContext.CurrentContext.WorkDirectory,
        $"{Guid.NewGuid():N}.pdf");

    private static ResumePdfRequest NewRequest(string resumeJson, string outputPath, string? accentColor = null) => new()
    {
        ResumeJson = resumeJson,
        OutputPdfFilePath = outputPath,
        AccentColor = accentColor
    };
}
