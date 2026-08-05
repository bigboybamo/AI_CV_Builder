using NewAI_CV_builder.Utilities;

namespace NewAI_CV_builder.Tests;

public class AtsResumePromptBuilderTests
{
    [Test]
    public void Build_WhenJobDescriptionIsEmpty_Throws()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            AtsResumePromptBuilder.Build("", "{\"basics\":{}}"));

        Assert.That(exception?.ParamName, Is.EqualTo("jobDescription"));
    }

    [Test]
    public void Build_WhenResumeJsonIsEmpty_Throws()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            AtsResumePromptBuilder.Build("Build a .NET API", ""));

        Assert.That(exception?.ParamName, Is.EqualTo("resumeJson"));
    }

    [Test]
    public void Build_IncludesResumeJobDescriptionAndJsonOutputRules()
    {
        var prompt = AtsResumePromptBuilder.Build(
            "Need a .NET developer with Azure and PostgreSQL experience.",
            "{\"skills\":[{\"name\":\"Backend\",\"keywords\":[\"[LANGUAGE_1]\"]}]}");

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
        var prompt = AtsResumePromptBuilder.BuildUpwork(new UpworkProposalRequest
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
        });

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
