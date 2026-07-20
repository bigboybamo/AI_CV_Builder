namespace NewAI_CV_builder.Utilities.ReversePrompts
{
    public static class ReverseHookCatalog
    {
        public static List<ReverseHookItem> Hooks => new()
        {
            new ReverseHookItem
            {
                Name = "Selective positioning",
                Hook = "You probably shouldn't hire me if…",
                Content = "Explain the type of client or project you are best suited for."
            },
            new ReverseHookItem
            {
                Name = "Recommend the cheaper option",
                Hook = "You may not need the full service yet.",
                Content = "Suggest a smaller, lower-risk solution before offering a bigger project."
            },
            new ReverseHookItem
            {
                Name = "Remove pressure",
                Hook = "You may already have this covered.",
                Content = "Briefly explain where you could still add value without pushing for a call."
            },
            new ReverseHookItem
            {
                Name = "Let them do it themselves",
                Hook = "You could handle this internally.",
                Content = "Explain what they would need to do and where your expertise would save time or reduce risk."
            },
            new ReverseHookItem
            {
                Name = "Limited availability",
                Hook = "I'm only taking on one more project this month.",
                Content = "Explain that you limit your workload so you can give each project proper attention."
            },
            new ReverseHookItem
            {
                Name = "Challenge the obvious solution",
                Hook = "You probably don't need another developer.",
                Content = "Explain that the real issue may be poor requirements, architecture, deployment or technical debt."
            },
            new ReverseHookItem
            {
                Name = "Challenge content volume",
                Hook = "You probably don't need more blog posts.",
                Content = "Explain that they need fewer, stronger articles that solve real customer problems."
            },
            new ReverseHookItem
            {
                Name = "Give only two options",
                Hook = "There are only two useful ways I can help here.",
                Content = "Present two clear service options instead of listing everything you can do."
            },
            new ReverseHookItem
            {
                Name = "Offer an observation instead of a pitch",
                Hook = "I noticed one thing that may be slowing your team down.",
                Content = "Share a specific insight about their product, hiring post, website or technical content."
            },
            new ReverseHookItem
            {
                Name = "Make the client qualify themselves",
                Hook = "This works best for teams that…",
                Content = "Describe the ideal client, problem and working relationship."
            },
            new ReverseHookItem
            {
                Name = "Emphasize opportunity cost",
                Hook = "Your developers could write the content themselves.",
                Content = "Explain that their developers' time may be better spent building the product."
            },
            new ReverseHookItem
            {
                Name = "Sell a pilot, not a commitment",
                Hook = "I wouldn't recommend committing to a large project yet.",
                Content = "Suggest one feature, audit or article as a small paid trial."
            }
        };
    }
}
