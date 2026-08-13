using NewAI_CV_builder.Services;

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
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(InvalidResumeJson, outputPath));

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
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(ValidResumeJson, outputPath, headless: true, cts.Token));

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
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(ValidResumeJson, outputPath, headless: true, cts.Token));

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
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(InvalidResumeJson, outputPath, headless: true, cts.Token));

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
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(InvalidResumeJson, outputPath, headless: true, cts.Token));

        //Assert
        Assert.That(exception?.ParamName, Is.EqualTo("resumeJson"));
    }

    private const string InvalidResumeJson = "{ invalid json";

    private const string ValidResumeJson = """
        { "basics": { "name": "Test Person", "email": "test@example.com" } }
        """;

    private static string NewOutputPath() => Path.Combine(
        TestContext.CurrentContext.WorkDirectory,
        $"{Guid.NewGuid():N}.pdf");
}
