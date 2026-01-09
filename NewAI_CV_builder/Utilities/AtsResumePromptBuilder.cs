using System;
using System.Collections.Generic;
using System.Text;

namespace NewAI_CV_builder.Utilities
{
    public static class AtsResumePromptBuilder
    {
        public static string Build(string jobDescription, string resumeJson)
        {
            if (string.IsNullOrWhiteSpace(jobDescription))
                throw new ArgumentException("Job description cannot be empty.", nameof(jobDescription));

            if (string.IsNullOrWhiteSpace(resumeJson))
                throw new ArgumentException("Resume JSON cannot be empty.", nameof(resumeJson));

            // Normalise line endings for consistency
            jobDescription = jobDescription.Replace("\r\n", "\n");
            resumeJson = resumeJson.Replace("\r\n", "\n");

            return $"""
Task:
1. Act like a sophisticated modern Applicant Tracking System (ATS), trained on the filtering rules and logic used by Greenhouse, Lever, SmartRecruiters, Workable, BambooHR, BreezyHR and Taleo.
2. Rewrite the "work" -> "highlights" of the resume JSON to better align with the job description and achieve a high ATS ranking.

Optimisation goals:
- Increase keyword match rate with the job description
- Use UK-standard terminology and phrasing
- Emphasise production systems, testing, maintenance, and scalability
- Prefer concise, impact-focused bullets (1–2 lines each)

ATS-specific rules:
- Prefer exact keyword matches over synonyms when reasonable
- Reorder highlights so the most relevant bullets appear first
- Avoid US-centric phrasing; use UK-neutral or UK-standard language
- Keep achievements believable and consistent with senior-level experience

Output rules:
- Return ONLY the full edited JSON
- Do NOT wrap the output in a markdown code block
- Keep the JSON schema/structure the same
- Edit ONLY text inside work -> highlights

JOB DESCRIPTION:
<<<
{jobDescription}
>>>

CANDIDATE JSON RESUME:
<<<
{resumeJson}
>>>
""";
        }

        public static string BuildUpwork(string jobDescription, string jobtype)
        {
            if (string.IsNullOrWhiteSpace(jobDescription))
                throw new ArgumentException("Job description cannot be empty.", nameof(jobDescription));

            if (string.IsNullOrWhiteSpace(jobtype))
                throw new ArgumentException("Job type cannot be empty.", nameof(jobtype));

            // Normalise line endings
            jobDescription = jobDescription.Replace("\r\n", "\n");

            return jobtype switch
            {
                "Web Developer" =>
                    string.Format(UpworkPromptTemplates.WebDeveloperCoverLetter, jobDescription),

                "Desktop Developer" =>
                    string.Format(UpworkPromptTemplates.DesktopDeveloperCoverLetter, jobDescription),

                _ =>
                    string.Format(UpworkPromptTemplates.TechnicalWriterCoverLetter, jobDescription)
            };
        }
    }
}
