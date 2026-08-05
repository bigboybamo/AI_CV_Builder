using NewAI_CV_builder.Services;

namespace NewAI_CV_builder.Tests;

public class ResumakePlaywrightFlowTests
{
    [Test]
    public void GeneratePdfFromJsonAsync_WhenJsonIsInvalid_ThrowsArgumentException()
    {
        var outputPath = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            $"{Guid.NewGuid():N}.pdf");

        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync("{ invalid json", outputPath));

        Assert.Multiple(() =>
        {
            Assert.That(exception?.ParamName, Is.EqualTo("resumeJson"));
            Assert.That(exception?.Message, Does.Contain("not valid JSON"));
        });
    }
}
