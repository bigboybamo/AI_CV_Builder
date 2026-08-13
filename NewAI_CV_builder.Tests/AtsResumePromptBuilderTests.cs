using NewAI_CV_builder.Utilities;

namespace NewAI_CV_builder.Tests;

public class AtsResumePromptBuilderTests
{
    [Test]
    public void Build_WhenJobDescriptionIsEmpty_Throws()
    {
        //Arrange
        const string jobDescription = "";
        const string resumeJson = "{\"basics\":{}}";

        //Act
        var exception = Assert.Throws<ArgumentException>(() =>
            AtsResumePromptBuilder.Build(jobDescription, resumeJson));

        //Assert
        Assert.That(exception?.ParamName, Is.EqualTo("jobDescription"));
    }

    [Test]
    public void Build_WhenResumeJsonIsEmpty_Throws()
    {
        //Arrange
        const string jobDescription = "Build a .NET API";
        const string resumeJson = "";

        //Act
        var exception = Assert.Throws<ArgumentException>(() =>
            AtsResumePromptBuilder.Build(jobDescription, resumeJson));

        //Assert
        Assert.That(exception?.ParamName, Is.EqualTo("resumeJson"));
    }

    [Test]
    public void Build_IncludesResumeJobDescriptionAndJsonOutputRules()
    {
        //Arrange
        const string jobDescription = "Need a .NET developer with Azure and PostgreSQL experience.";
        const string resumeJson = "{\"skills\":[{\"name\":\"Backend\",\"keywords\":[\"[LANGUAGE_1]\"]}]}";

        //Act
        var prompt = AtsResumePromptBuilder.Build(jobDescription, resumeJson);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(prompt, Does.Contain("Need a .NET developer with Azure and PostgreSQL experience."));
            Assert.That(prompt, Does.Contain("\"[LANGUAGE_1]\""));
            Assert.That(prompt, Does.Contain("Return ONLY the full edited JSON"));
            Assert.That(prompt, Does.Contain("The final JSON MUST NOT contain any square-bracket placeholder tokens"));
        });
    }

    [Test]
    public void BuildUpwork_InsertsRuntimeRulesAndRemovesTemplateTokens()
    {
        //Arrange
        var request = new UpworkProposalRequest
        {
            JobDescription = "Need a WinForms developer to maintain a desktop reporting tool.",
            JobType = "Desktop Developer",
            RuntimeRules = new[] { "Mention maintenance experience early." },
            ProjectHighlights = new[]
            {
                new ProjectHighlight
                {
                    Name = "Job Search Builder",
                    Description = "A desktop automation tool for managing job applications.",
                    PictureUrl = "https://example.com/job-search-builder.png"
                }
            }
        };

        //Act
        var prompt = AtsResumePromptBuilder.BuildUpwork(request);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(prompt, Does.Contain("Need a WinForms developer to maintain a desktop reporting tool."));
            Assert.That(prompt, Does.Contain("Mention maintenance experience early."));
            Assert.That(prompt, Does.Contain("A desktop automation tool for managing job applications."));
            Assert.That(prompt, Does.Not.Contain("<<GEN_RULES>>"));
            Assert.That(prompt, Does.Not.Contain("<<REVERSE_HOOK>>"));
            Assert.That(prompt, Does.Not.Contain("<<JOB_SEARCH_BUILDER>>"));
        });
    }
}
