using Microsoft.Playwright;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace NewAI_CV_builder.Services
{
    public static class ResumakePlaywrightFlow
    {
        /// <summary>
        /// Takes resume JSON (e.g., JSON Resume schema), imports into resumake.io,
        /// clicks Make, and saves the generated PDF to outputPdfPath.
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

            var tempJsonPath = Path.Combine(Path.GetTempPath(), $"resumake-{Guid.NewGuid():N}.json");
            await File.WriteAllTextAsync(
                tempJsonPath,
                resumeJson,
                new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));

            try
            {
                Log.Information("Starting Playwright browser automation for resumake.io...");
                using var playwright = await Playwright.CreateAsync();

                await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = headless,
                    SlowMo = 200
                });

                var context = await browser.NewContextAsync(new BrowserNewContextOptions
                {
                    AcceptDownloads = true
                });

                var page = await context.NewPageAsync();

                await page.GotoAsync("https://resumake.io/", new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle,
                    Timeout = 60_000
                });

                var importButton = page.GetByText("Import JSON", new() { Exact = false });

                if (await importButton.CountAsync() == 0)
                {
                    await page.GotoAsync("https://latexresu.me/", new() { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 60_000 });
                    importButton = page.GetByText("Import JSON", new() { Exact = false });
                }

                // Arm chooser BEFORE click
                var fileChooser = await page.RunAndWaitForFileChooserAsync(async () =>
                {
                    await importButton.First.ClickAsync(new() { Timeout = 30_000 });
                }, new() { Timeout = 30_000 });

                await fileChooser.SetFilesAsync(tempJsonPath);

                if (!page.Url.Contains("/generator", StringComparison.OrdinalIgnoreCase))
                {
                    await page.GotoAsync("https://resumake.io/generator", new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.NetworkIdle,
                        Timeout = 60_000
                    });
                }

                var makeButtons = page.Locator("button[type='submit'][form='resume-form']");
                await makeButtons.First.WaitForAsync(new() { State = WaitForSelectorState.Attached, Timeout = 60_000 });

                var clicked = false;
                var count = await makeButtons.CountAsync();
                for (int i = 0; i < count; i++)
                {
                    var btn = makeButtons.Nth(i);
                    if (await btn.IsVisibleAsync() && await btn.IsEnabledAsync())
                    {
                        await btn.ClickAsync();
                        clicked = true;
                        break;
                    }
                }

                if (!clicked)
                    throw new InvalidOperationException("Could not find a visible/enabled 'Make' button to click.");

                // Wait for the PDF link to exist + become a real blob href (means generation finished)
                var pdfLink = page.Locator("a[download='resume.pdf']:has-text('PDF')");
                await pdfLink.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 60_000 });

                await WaitUntilPdfBlobReadyAsync(page, pdfLink, timeoutMs: 30_000, minBytes: 5_000);


                var downloadTask = page.WaitForDownloadAsync(new() { Timeout = 60_000 });
                await pdfLink.ClickAsync();
                var download = await downloadTask;

                // Save to file path (NOT folder)
                await download.SaveAsAsync(outputPdfFilePath);

                await context.CloseAsync();

                context = await browser.NewContextAsync();
                page = await context.NewPageAsync();

                var fi = new FileInfo(outputPdfFilePath);
                if (!fi.Exists || fi.Length == 0)
                    throw new IOException("Downloaded PDF is empty (0 bytes). Try increasing the post-generation wait or verify PDF link readiness.");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "Error in Playwright GeneratePdfFromJsonAsync method.");
                throw;
            }
            finally
            {
                try { if (File.Exists(tempJsonPath)) File.Delete(tempJsonPath); } catch { }
            }
        }

        private static async Task WaitUntilPdfBlobReadyAsync(
            IPage page,
            ILocator pdfLink,
            int timeoutMs = 50_000,
            int minBytes = 5_000,          
            int pollDelayMs = 500)
        {
            var sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                // Re-read href each time (even if it usually stays the same)
                var href = await pdfLink.GetAttributeAsync("href");
                if (!string.IsNullOrWhiteSpace(href) && href.StartsWith("blob:", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        // Fetch the blob and return its size
                        var size = await page.EvaluateAsync<int>(
                            @"async (url) => {
                        const res = await fetch(url);
                        if (!res.ok) return -1;
                        const b = await res.blob();
                        return b.size || 0;
                    }",
                            href
                        );

                        if (size >= minBytes)
                            return;
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Error while checking PDF blob size; will retry...");
                    }
                }

                await page.WaitForTimeoutAsync(pollDelayMs);
            }

            throw new TimeoutException("PDF blob never became ready (non-empty) within the timeout.");
        }
    }

}
