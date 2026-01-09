using System;
using System.Collections.Generic;
using System.Text;

namespace NewAI_CV_builder.Utilities
{
    public static class UpworkPromptTemplates
    {
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

            Keep it concise, avoid jargon, and emphasise the real-world systems and links. Make sure it looks like a proposal and NOT a Cover letter";


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

            Keep it short, use minimal jargon, and place strong emphasis on the links.";


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

            Keep it concise, avoid heavy jargon, and prioritise clarity and links.";

    }

}
