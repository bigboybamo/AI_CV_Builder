namespace NewAI_CV_builder.Utilities
{
    public static class CheckBoxRuleCatalog
    {
        public static List<CheckBoxRuleItem> JobTypeRules => new()
        {
            new CheckBoxRuleItem { Text = "Ask Question", Value = "Ask an insightful question or suggest a solution that demonstrates expertise. Example: 'Have you tried X? I suspect the issue is Y.'" },
            new CheckBoxRuleItem { Text = "Sugges Approach", Value = "Suggest an approach on how to solve this specific problem. Example:'I'll refactor your API and add caching to improve response times' " },
            new CheckBoxRuleItem { Text = "Call to Action", Value = "Include a call to action. Example: 'When would you like to schedule a call to discuss your technical requirements?'"}
        };
    }
}