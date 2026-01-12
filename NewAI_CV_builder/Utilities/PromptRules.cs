using System;
using System.Collections.Generic;
using System.Text;

namespace NewAI_CV_builder.Utilities
{
    public class PromptRules
    {

            public static readonly List<string> UpworkProposalBaseRules = new()
        {
            "Use plain, direct language",
            "Do NOT add sections beyond a greeting, body paragraphs, and a sign-off",
            "Do NOT add headings, bullet points, or emojis",
            "Do NOT invent experience or links",
            "Do NOT remove or modify any provided links",
            "Do NOT repeat the job description",
            "Keep the letter concise and professional",
            "Avoid buzzwords, marketing language, and exaggeration",
            "Output ONLY the final cover letter text"
        };

        public static string BuildRulesBlock(IEnumerable<string>? extraRules = null)
        {
            var rules = new List<string>(UpworkProposalBaseRules);

            if (extraRules != null)
            {
                int insertIndex = Math.Max(0, rules.Count - 1);
                rules.InsertRange(insertIndex, extraRules);
            }

            return "Generation rules (must follow exactly):\n- " +
                   string.Join("\n- ", rules);
        }
    }
}
