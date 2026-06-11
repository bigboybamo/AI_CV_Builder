using System;
using System.Collections.Generic;
using System.Text;

namespace NewAI_CV_builder.Utilities
{
    public static class UpworkPromptTemplates
    {
        private const string GenRulesToken = "<<GEN_RULES>>";
        private const string ProjectHighlightsToken = "<<PROJECT_HIGHLIGHTS>>";

        public const string WebDeveloperCoverLetter =
            @"Create an upwork proposal for a Web Developer role using the following job description:

            {0}

            Be sure to mention my experience building and consuming APIs for enterprise and production systems, including:

            FirstBank:
            https://firstdirect2.firstbanknigeria.com

            Fintellia (core banking platform):
            https://www.fintellia.com/core-banking/

            Novodes (MultiRepo Manager)

            Novo Health – Ijele HMO application:
            https://ijele.novohealthafrica.org/

            Keep it concise, avoid jargon, and emphasise the real-world systems and links. Make sure it looks like a proposal and NOT a Cover letter 
<<GEN_RULES>>

                Similar to

                Hey Ahmad/Team,

                I specialize in Software development (.NET) and I've applied these skills in enterprise environments such as Novo Health Africa (healthcare systems), First Bank Of Nigeria (Nigeria's top/ oldest bank) and several other Fintechs. Most of my work has revolved around working on and improving existing systems.

                I've worked on a few Winforms desktop applications in my time and even have my own personal pet project you can check out https://github.com/bigboybamo/Text2speeech

                I also, recently completed a job here on upwork with the same technologies, check that out here https://www.upwork.com/jobs/~021980188839600873740

                I've also written several guides on configuring installers for desktop applications;
                https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-visual-studio-and-advanced-4lln
                https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-clickonce-for-visual-studio-2022-3272
                https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-wix-for-visual-studio-2022-1c47

                I've worked on 2 or 3 Web API projects that used CQRS pattern. Though they properietary code, I am happy to discuss them during an interview.

                I am available for full time engagment in the coming weeks. I am also open to real time communication, so no issues there.

                Expect the following if you hire me:
                1. 100% effort
                2. Ownership of your work.
                3. Clean and quality code
                4. Clear and effective communication

                You can find out more about me using my online presence
                - GitHub - https://github.com/bigboybamo
                - Dev.to - https://dev.to/bigboybamo

                Best,
                Ola

                ";

        public const string DesktopDeveloperCoverLetter =
        @"Create a cover letter for a Desktop Developer role using the following job description:

            {0}

            Be sure to mention my work on the Product Match application:
            https://dataladder.com/products/product-match/

            My Text-to-Speech desktop application:
            https://github.com/bigboybamo/Text2speeech

            My contributions to the official .NET documentation:
            https://github.com/dotnet/docs/pulls?q=is%3Apr+is%3Aclosed+author%3Abigboybamo

            My articles on building Windows desktop installers:
            https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-clickonce-for-visual-studio-2022-3272
            https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-wix-for-visual-studio-2022-1c47
            https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-visual-studio-2022-installer-project-5nh

            Keep it short, use minimal jargon, and place strong emphasis on the links. 
<<GEN_RULES>>

            Similar to

            Hey Team,

            I specialize in Software development (.NET) and I've applied these skills in enterprise environments such as Novo Health Africa (healthcare systems), First Bank Of Nigeria (Nigeria's top/ oldest bank) and several other Fintechs. Most of my work has revolved around working on and improving existing systems.

            I've worked on a few Winforms desktop applications in my time and even have my own personal pet project you can check out https://github.com/bigboybamo/Text2speeech

            I've also written several guides on configuring installers for desktop applications;
            https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-visual-studio-and-advanced-4lln
            https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-clickonce-for-visual-studio-2022-3272
            https://dev.to/bigboybamo/how-to-create-an-installer-for-a-winforms-application-using-wix-for-visual-studio-2022-1c47

            I also regularly make contributions to the official .NET documentation in my free time https://github.com/dotnet/docs/pulls?q=is%3Apr+is%3Aclosed+author%3Abigboybamo.

            Expect the following if you hire me:
            1. 100% effort
            2. Ownership of your work.
            3. Clean and quality code
            4. Clear and effective communication

            You can find out more about me using my online presence
            - GitHub - https://github.com/bigboybamo
            - Dev.to - https://dev.to/bigboybamo

            I'd love the chance to join your team and contribute my expertise. Please let me know what time works for you and we can get on a Zoom call here on Upwork.

            Best,
            Ola

            ";

        public const string TechnicalWriterCoverLetter =
             @"Create a cover letter for a Technical Writer role using the following job description:

            {0}

            Be sure to mention my documentation and how-to work for Shesha:
            https://docs.shesha.io/docs/get-started/Introduction/
            https://github.com/shesha-io/docs.shesha

            Mention how my personal blog focuses on step-by-step tutorials with runnable code examples:
            https://dev.to/bigboybamo

            Reference my GitHub, where I publish and maintain the example projects used in my articles:
            https://github.com/bigboybamo

            Highlight my technical how-to articles written for Devart:
            https://www.devart.com/dotconnect/sqlite/connect-sqlite-in-net.html
            https://www.devart.com/dotconnect/postgresql/connect-postgresql-in-net.html
            https://www.devart.com/dotconnect/mysql/maui-mysql-crud-operations.html

            Mention my contributions to the official .NET documentation, focused on clarity and completeness:
            https://github.com/dotnet/docs/pulls?q=is%3Apr+is%3Aclosed+author%3Abigboybamo

            Keep it concise, avoid heavy jargon, and prioritise clarity and links. 
<<GEN_RULES>>

                 Similar to

                    Hey there !!
                    My name is Ola, I'm an experienced Software Engineer and Tech writer.

                    When I'm not writing code, I'm usually writing about code.

                    I wrote most of the docs and how-to guides for Shesha https://docs.shesha.io/docs/get-started/Introduction/ , Gitub: https://github.com/shesha-io/docs.shesha

                    I have written several How-Tos for Devart:
                    https://www.devart.com/dotconnect/sqlite/connect-sqlite-in-net.html
                    https://www.devart.com/dotconnect/postgresql/connect-postgresql-in-net.html
                    https://www.devart.com/dotconnect/mysql/maui-mysql-crud-operations.html

                    I contribute to the official .NET documentation, ensuring clarity and completeness https://github.com/dotnet/docs/pulls?q=is%3Apr+is%3Aclosed+author%3Abigboybamo.

                    And I write on my blog as well https://dev.to/bigboybamo

                    I write software for a living, so I know how to write good software content as well.

                    Feel free to message me and we can kick off a conversion.

                    Best Regards,
                    Ola
";

        public const string AiDeveloperCoverLetter =
            @"Create a cover letter for an AI Engineer role using the following job description:

            {0}

            Lead with my ability to assess business problems and design practical AI solutions using LLMs, agents, RAG pipelines, and API integrations, not just implement what I'm told.

            Highlight my hands-on experience with Claude API and OpenAI API in production environments, including prompt design, structured outputs, response validation, and fallback handling.

            Mention my proficiency with AI-assisted coding tools, particularly Claude Code and OpenAI Codex, and how I use them to accelerate delivery, maintain code quality, and ship faster without cutting corners.

            Include my current hands-on experience operating AI-driven development workflows: managing multiple Claude Code terminal sessions simultaneously, reviewing and approving AI-generated tool calls, deploying to cloud platforms like Railway, Fly.io, and Google Cloud Run, triaging bugs from UAT, and knowing when to escalate versus act independently. This gives me an operator-level understanding of how AI coding agents behave in practice, not just theory.

            Emphasise my production discipline: observability, error handling, maintainable code, and clear documentation. I build things that last beyond the prototype.

<<PROJECT_HIGHLIGHTS>>

            Reference my GitHub and technical writing portfolio to demonstrate implementation depth and architecture thinking:
            https://github.com/bigboybamo
            https://dev.to/bigboybamo

            Keep the tone direct and grounded, no hype, no buzzword stacking. Focus on real-world AI delivery with measurable business value. Show I can work with stakeholders, translate ambiguous requirements into working systems, and ship AI features users can trust.
<<GEN_RULES>>

            Similar to

            Hey Team,
            I am a software engineer with hands-on experience designing and delivering AI-enabled solutions, from LLM integrations and RAG pipelines to agent-based workflows, using Claude API and OpenAI API in production environments.

            I also work extensively with AI-assisted coding tools, particularly Claude Code and OpenAI Codex. Beyond building with them, I currently operate AI-driven development workflows professionally (https://www.upwork.com/jobs/~022027119110980960386), giving me operator-level insight into how AI coding agents behave under real conditions, where they succeed, and where they need guardrails.

            Here are a few AI-powered applications I have recently designed and shipped:

            - Modak Web — [short project description]. You can see it here: [picture link]
            - Help Me Rad Bridge — [short project description]. You can see it here: [picture link]
            - Job Search Builder — [short project description]. You can see it here: [picture link]

            I know what it takes to ship AI features that hold up under real conditions, not just demos.

            My focus goes beyond integration: I assess business problems first, then design the right AI approach, whether that is structured prompting, retrieval-augmented generation, agent orchestration, or a simpler solution that gets the job done reliably.

            I make AI features production-ready: response validation, logging, fallback handling, observability, and maintainable code paths that teams can own long-term.

            You can review my public work here:
            - GitHub: https://github.com/bigboybamo
            - Technical articles: https://dev.to/bigboybamo

            I can help you move from AI prototype to a dependable feature your users and business can rely on.

            Best,
            Ola

            ";

        public static string Build(string template, UpworkProposalRequest request)
        {
            if (!template.Contains(GenRulesToken))
                throw new InvalidOperationException($"Template is missing the token: {GenRulesToken}");

            var formatted = string.Format(template, request.JobDescription)
                .Replace("\r\n", "\n");

            var rulesBlock = PromptRules.BuildRulesBlock(request.RuntimeRules);
            var result = formatted.Replace(GenRulesToken, rulesBlock);

            if (result.Contains(ProjectHighlightsToken))
                result = result.Replace(ProjectHighlightsToken, BuildProjectHighlightsBlock(request.ProjectHighlights));

            if (!string.IsNullOrWhiteSpace(request.LoomUrl))
            {
                var loomIntro = request.JobType == "AI Developer"
                    ? "Here's a short video of me so you can get a feel for how I communicate, as well as my AI agent workflow"
                    : "Here's a short video of me so you can get a feel for how I communicate";

                result += $"\n\nInclude the following Loom video link near the end of the proposal, just before the sign-off, with a natural sentence such as \"{loomIntro}: {request.LoomUrl}\"";
            }

            return result;
        }

        private static string BuildProjectHighlightsBlock(IEnumerable<ProjectHighlight>? highlights)
        {
            if (highlights is null)
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var highlight in highlights)
            {
                if (string.IsNullOrWhiteSpace(highlight.PictureUrl) && string.IsNullOrWhiteSpace(highlight.Description))
                    continue;

                sb.Append($"\n{highlight.Name}: {highlight.Description}");

                if (!string.IsNullOrWhiteSpace(highlight.PictureUrl))
                    sb.Append($"\nPicture: {highlight.PictureUrl}");

                sb.Append('\n');
            }

            if (sb.Length == 0)
                return string.Empty;

            return "Highlight the following AI projects I have personally built and shipped. " +
                   "Weave each description in naturally and include the picture link for each project so the client can see the application:\n"
                   + sb.ToString().TrimEnd();
        }
    }


}
