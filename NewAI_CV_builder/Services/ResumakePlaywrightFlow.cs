using Microsoft.Playwright;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;

namespace NewAI_CV_builder.Services
{
    public static class ResumakePlaywrightFlow
    {
        /// <summary>
        /// Takes resume JSON (JSON Resume schema), renders it into a styled HTML
        /// document (ported from generate_resume.py) and prints it to a PDF at
        /// <paramref name="outputPdfFilePath"/> using headless Chromium via Playwright.
        /// All highlights from every job are always rendered — nothing is dropped.
        /// </summary>
        public static async Task GeneratePdfFromJsonAsync(
            string resumeJson,
            string outputPdfFilePath,
            bool headless = true)
        {
            if (string.IsNullOrWhiteSpace(resumeJson))
                throw new ArgumentException("Resume JSON is empty.", nameof(resumeJson));

            if (string.IsNullOrWhiteSpace(outputPdfFilePath))
                throw new ArgumentException("Output path is empty.", nameof(outputPdfFilePath));

            // Ensure we got a FILE path, not a folder
            var outputDir = Path.GetDirectoryName(outputPdfFilePath);
            if (string.IsNullOrWhiteSpace(outputDir))
                throw new ArgumentException("Output path must include a directory and filename (e.g. C:\\Users\\...\\Downloads\\resume.pdf)");

            Directory.CreateDirectory(outputDir);

            JsonDocument document;
            try
            {
                document = JsonDocument.Parse(resumeJson);
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Resume JSON could not be parsed.");
                throw new ArgumentException("Resume JSON is not valid JSON.", nameof(resumeJson), ex);
            }

            try
            {
                var html = RenderHtml(document.RootElement);

                Log.Information("Rendering resume PDF via headless Chromium to {OutputPath}...", outputPdfFilePath);
                using var playwright = await Playwright.CreateAsync().ConfigureAwait(false);

                // PDF generation requires headless Chromium.
                await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = headless
                }).ConfigureAwait(false);

                var page = await browser.NewPageAsync().ConfigureAwait(false);

                await page.SetContentAsync(html, new PageSetContentOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                }).ConfigureAwait(false);

                // The @page rule in the HTML defines A4 + margins; honour it via PreferCSSPageSize.
                await page.PdfAsync(new PagePdfOptions
                {
                    Path = outputPdfFilePath,
                    PrintBackground = true,
                    PreferCSSPageSize = true
                }).ConfigureAwait(false);

                await browser.CloseAsync().ConfigureAwait(false);

                var fi = new FileInfo(outputPdfFilePath);
                if (!fi.Exists || fi.Length == 0)
                    throw new IOException("Generated PDF is empty (0 bytes).");

                Log.Information("Resume PDF written ({Bytes} bytes) to {OutputPath}.", fi.Length, outputPdfFilePath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in GeneratePdfFromJsonAsync method.");
                throw;
            }
            finally
            {
                document.Dispose();
            }
        }

        /// <summary>HTML-escape a value (ports the Python <c>e()</c> helper).</summary>
        private static string E(string? s) => WebUtility.HtmlEncode(s ?? string.Empty);

        /// <summary>Reads a string property off an object element, returning "" when absent/null.</summary>
        private static string Str(JsonElement el, string property)
        {
            if (el.ValueKind == JsonValueKind.Object
                && el.TryGetProperty(property, out var value)
                && value.ValueKind == JsonValueKind.String)
            {
                return value.GetString() ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>Enumerates an array property, returning an empty sequence when absent.</summary>
        private static IEnumerable<JsonElement> Arr(JsonElement el, string property)
        {
            if (el.ValueKind == JsonValueKind.Object
                && el.TryGetProperty(property, out var value)
                && value.ValueKind == JsonValueKind.Array)
            {
                return value.EnumerateArray();
            }
            return Enumerable.Empty<JsonElement>();
        }

        /// <summary>Reads an array of strings (ignoring null/non-string entries).</summary>
        private static List<string> StrArr(JsonElement el, string property) =>
            Arr(el, property)
                .Where(x => x.ValueKind == JsonValueKind.String)
                .Select(x => x.GetString() ?? string.Empty)
                .ToList();

        /// <summary>
        /// Builds the polished resume HTML from the parsed JSON.
        /// Direct port of <c>render_html</c> in generate_resume.py.
        /// </summary>
        private static string RenderHtml(JsonElement data)
        {
            var basics = data.ValueKind == JsonValueKind.Object && data.TryGetProperty("basics", out var b)
                ? b
                : default;

            var name = Str(basics, "name");
            var website = Str(basics, "website");
            var contacts = new[] { Str(basics, "email"), Str(basics, "phone") }
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

            var sb = new StringBuilder();

            sb.Append($$"""
<!doctype html>
<html lang="en"><head><meta charset="utf-8">
<title>{{E(name)}} — Resume</title>
<style>
  @page { size: A4; margin: 11mm 13mm; }
  * { box-sizing: border-box; }
  body {
    font-family: "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif;
    color: #1f2933; font-size: 9.5pt; line-height: 1.3; margin: 0;
    -webkit-print-color-adjust: exact; print-color-adjust: exact;
  }
  a { color: #1f6feb; text-decoration: none; }
  header { border-bottom: 2.5px solid #1f6feb; padding-bottom: 6px; margin-bottom: 10px; }
  h1 { font-size: 20pt; margin: 0; letter-spacing: .5px; color: #0b2545; }
  .contact { margin-top: 4px; font-size: 9pt; color: #52606d; }
  .contact span { white-space: nowrap; }
  .sep { color: #cbd2d9; margin: 0 6px; }
  section { margin-bottom: 9px; }
  h2 {
    font-size: 10pt; text-transform: uppercase; letter-spacing: 1.2px;
    color: #1f6feb; margin: 0 0 5px; padding-bottom: 2px;
    border-bottom: 1px solid #e4e7eb;
  }
  .entry { margin-bottom: 7px; page-break-inside: avoid; }
  .entry-head { display: flex; justify-content: space-between; align-items: baseline; }
  .role { font-weight: 600; color: #0b2545; font-size: 10pt; }
  .company { font-weight: 600; color: #334e68; }
  .meta { color: #627d98; font-size: 8.8pt; white-space: nowrap; padding-left: 10px; }
  .subhead { display: flex; justify-content: space-between; font-size: 9pt; margin: 0 0 3px; }
  ul { margin: 3px 0 0; padding-left: 16px; }
  li { margin-bottom: 1.5px; }
  .skills-grid { display: grid; grid-template-columns: 1fr; gap: 3px; }
  .skill-row { display: flex; gap: 8px; align-items: baseline; }
  .skill-name { font-weight: 600; color: #334e68; min-width: 165px; }
  .tags span {
    display: inline-block; background: #eef4ff; color: #1f4e8c;
    border-radius: 4px; padding: 1px 6px; margin: 0 3px 3px 0; font-size: 8.6pt;
  }
  .edu-line, .award { margin-bottom: 5px; page-break-inside: avoid; }
  .award-title { font-weight: 600; color: #0b2545; }
  .award-summary { font-size: 8.8pt; color: #52606d; margin-top: 1px; }
</style></head><body>
""");

            // Header
            var contactHtml = new StringBuilder();
            if (contacts.Count > 0)
            {
                contactHtml.Append($"<span>{E(contacts[0])}</span>");
                foreach (var c in contacts.Skip(1))
                    contactHtml.Append($"<span class=\"sep\">|</span><span>{E(c)}</span>");
            }
            if (!string.IsNullOrWhiteSpace(website))
            {
                var label = website.Replace("https://", "").Replace("http://", "");
                contactHtml.Append($"<span class=\"sep\">|</span><span><a href=\"{E(website)}\">{E(label)}</a></span>");
            }
            sb.Append($"<header><h1>{E(name)}</h1><div class=\"contact\">{contactHtml}</div></header>");

            // Work
            sb.Append("<section><h2>Experience</h2>");
            foreach (var job in Arr(data, "work"))
            {
                sb.Append("<div class=\"entry\">");
                sb.Append($"<div class=\"entry-head\"><span class=\"role\">{E(Str(job, "position"))}</span>"
                    + $"<span class=\"meta\">{E(Str(job, "startDate"))} – {E(Str(job, "endDate"))}</span></div>");
                sb.Append($"<div class=\"subhead\"><span class=\"company\">{E(Str(job, "company"))}</span>"
                    + $"<span class=\"meta\">{E(Str(job, "location"))}</span></div>");
                var bullets = StrArr(job, "highlights").Where(h => !string.IsNullOrWhiteSpace(h)).ToList();
                if (bullets.Count > 0)
                    sb.Append("<ul>" + string.Concat(bullets.Select(h => $"<li>{E(h)}</li>")) + "</ul>");
                sb.Append("</div>");
            }
            sb.Append("</section>");

            // Skills
            var skills = Arr(data, "skills")
                .Where(s => StrArr(s, "keywords").Any(k => !string.IsNullOrWhiteSpace(k)))
                .ToList();
            if (skills.Count > 0)
            {
                sb.Append("<section><h2>Skills</h2><div class=\"skills-grid\">");
                foreach (var s in skills)
                {
                    var kws = StrArr(s, "keywords").Where(k => !string.IsNullOrWhiteSpace(k));
                    var tags = string.Concat(kws.Select(k => $"<span>{E(k)}</span>"));
                    sb.Append($"<div class=\"skill-row\"><span class=\"skill-name\">{E(Str(s, "name"))}</span>"
                        + $"<span class=\"tags\">{tags}</span></div>");
                }
                sb.Append("</div></section>");
            }

            // Education
            var education = Arr(data, "education").ToList();
            if (education.Count > 0)
            {
                sb.Append("<section><h2>Education</h2>");
                foreach (var ed in education)
                {
                    sb.Append("<div class=\"edu-line\">"
                        + $"<div class=\"entry-head\"><span class=\"role\">{E(Str(ed, "studyType"))}, {E(Str(ed, "area"))}</span>"
                        + $"<span class=\"meta\">{E(Str(ed, "startDate"))} – {E(Str(ed, "endDate"))}</span></div>"
                        + $"<div class=\"subhead\"><span class=\"company\">{E(Str(ed, "institution"))}</span>"
                        + $"<span class=\"meta\">{E(Str(ed, "location"))}</span></div></div>");
                }
                sb.Append("</section>");
            }

            // Awards / Certifications
            var awards = Arr(data, "awards").ToList();
            if (awards.Count > 0)
            {
                var headings = data.ValueKind == JsonValueKind.Object && data.TryGetProperty("headings", out var h)
                    ? h
                    : default;
                var awardHeading = Str(headings, "awards");
                if (string.IsNullOrWhiteSpace(awardHeading))
                    awardHeading = "Certifications";

                sb.Append($"<section><h2>{E(awardHeading)}</h2>");
                foreach (var a in awards)
                {
                    sb.Append("<div class=\"award\">"
                        + $"<div class=\"entry-head\"><span class=\"award-title\">{E(Str(a, "title"))}</span>"
                        + $"<span class=\"meta\">{E(Str(a, "awarder"))} · {E(Str(a, "date"))}</span></div>"
                        + $"<div class=\"award-summary\">{E(Str(a, "summary"))}</div></div>");
                }
                sb.Append("</section>");
            }

            sb.Append("</body></html>");
            return sb.ToString();
        }
    }
}
