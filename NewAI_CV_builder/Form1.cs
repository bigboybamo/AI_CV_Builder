using NewAI_CV_builder.Services;
using NewAI_CV_builder.Utilities;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace NewAI_CV_builder
{
    public partial class Form1 : Form
    {
        private readonly string? developerLoomUrl = Environment.GetEnvironmentVariable("DEVELOPER_LOOM_URL");
        private readonly string? technicalWriterLoomUrl = Environment.GetEnvironmentVariable("TECHNICAL_WRITER_LOOM_URL");
        private readonly string? aiDeveloperLoomUrl = Environment.GetEnvironmentVariable("AI_DEVELOPER_LOOM_URL");
        private readonly string? modakWebPic = Environment.GetEnvironmentVariable("MODAK_WEB_PIC");
        private readonly string? modakWebDesc = Environment.GetEnvironmentVariable("MODAK_WEB_DESC");
        private readonly string? helpMeRadBridgePic = Environment.GetEnvironmentVariable("HELP_ME_RAD_BRIDGE_PIC");
        private readonly string? helpMeRadBridgeDesc = Environment.GetEnvironmentVariable("HELP_ME_RAD_BRIDGE_DESC");
        private readonly string? jobSearchBuilderPic = Environment.GetEnvironmentVariable("JOB_SEARCH_BUILDER_PIC");
        private readonly string? jobSearchBuilderDesc = Environment.GetEnvironmentVariable("JOB_SEARCH_BUILDER_DESC");
        private readonly System.Windows.Forms.Timer _debounceTimer = new System.Windows.Forms.Timer();
        private bool _isUpdating;
        private readonly List<string> _jobTitles;

        // Kept separate so stopping the resume flow never touches an in-flight
        // Upwork proposal, and vice versa. Both are only ever touched on the UI thread.
        private CancellationTokenSource? _resumeCancellationTokenSource;
        private CancellationTokenSource? _proposalCancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
            _jobTitles = new List<string> { "-- Select a job title --", "Web Developer", "Desktop Developer", "Technical Writer", "AI Developer" };

            MoreRulesBox.DisplayMember = nameof(CheckBoxRuleItem.Text);
            MoreRulesBox.ValueMember = nameof(CheckBoxRuleItem.Value);
            MoreRulesBox.DataSource = CheckBoxRuleCatalog.JobTypeRules;

            Jobs_List.DataSource = _jobTitles;
            Jobs_List.SelectedIndex = 0;
            _debounceTimer.Interval = 300; // ms
            _debounceTimer.Tick += DebounceTimer_Tick;
        }
        private static readonly HttpClient _httpClaude = new HttpClient
        {
            BaseAddress = new Uri("https://api.anthropic.com/")
        };

        private static readonly HttpClient _httpOpenAI = new HttpClient
        {
            BaseAddress = new Uri("https://api.openai.com/")
        };

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();

            if (_isUpdating) return;

            _isUpdating = true;
            JsonCV.Text = MarkdownText.StripCodeFence(TextOutput.Text);
            _isUpdating = false;

            Generate_Rsme.PerformClick();
        }

        public static async Task<string> CallOpenAiAsync(string prompt, string apiKey, CancellationToken cancellationToken = default)
        {
            Log.Information("CallOpenAiAsync started. PromptLength={PromptLength}", prompt?.Length ?? 0);

            var preview = (prompt ?? string.Empty).Replace("\r", " ").Replace("\n", " ");
            if (preview.Length > 200)
                preview = preview.Substring(0, 200) + "...";
            Log.Debug("Prompt preview: {PromptPreview}", preview);

            var request = new
            {
                model = "gpt-4.1-mini",
                input = prompt
            };

            using var msg = new HttpRequestMessage(HttpMethod.Post, "v1/responses");
            msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = JsonSerializer.Serialize(request);
            msg.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            Log.Debug("Sending HTTP request to OpenAI Responses API. PayloadSize={PayloadSizeBytes}", payload?.Length ?? 0);

            try
            {
                using var resp = await _httpOpenAI.SendAsync(msg, cancellationToken).ConfigureAwait(false);
                var json = await resp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                Log.Debug("HTTP response received. StatusCode={StatusCode}, ResponseSize={ResponseSizeBytes}", resp.StatusCode, json?.Length ?? 0);

                if (!resp.IsSuccessStatusCode)
                {
                    Log.Error("OpenAI API returned non-success status. StatusCode={StatusCode} Reason={ReasonPhrase}", (int)resp.StatusCode, resp.ReasonPhrase);
                    Log.Debug("OpenAI error body: {ResponseBody}", json);
                    return $"Error: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}";
                }

                // The Responses API includes a convenient aggregated string in `output_text` in many SDKs,
                using var doc = JsonDocument.Parse(json);
                Log.Debug("Parsed JSON response.");

                // Try to extract: output[0].content[0].text
                if (doc.RootElement.TryGetProperty("output", out var output) &&
                    output.ValueKind == JsonValueKind.Array &&
                    output.GetArrayLength() > 0)
                {
                    var content = output[0].GetProperty("content");
                    if (content.ValueKind == JsonValueKind.Array && content.GetArrayLength() > 0)
                    {
                        var first = content[0];
                        if (first.TryGetProperty("text", out var textEl))
                        {
                            var result = textEl.GetString() ?? "";
                            Log.Information("CallOpenAiAsync completed successfully. OutputLength={OutputLength}", result.Length);
                            return result;
                        }
                    }
                }

                Log.Warning("Response did not contain expected 'output[0].content[0].text'; returning raw JSON.");
                return json;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // User pressed Stop — expected, not a failure. An HttpClient timeout also
                // surfaces as OperationCanceledException, so the token guard keeps the two apart.
                Log.Information("CallOpenAiAsync cancelled by the user.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while calling OpenAI API");
                throw;
            }
        }

        public static async Task<string> CallClaudeAsync(string prompt, string apiKey, CancellationToken cancellationToken = default)
        {
            Log.Information("CallClaudeAsync started. PromptLength={PromptLength}", prompt?.Length ?? 0);
            var preview = (prompt ?? string.Empty).Replace("\r", " ").Replace("\n", " ");
            if (preview.Length > 200)
                preview = preview.Substring(0, 200) + "...";
            Log.Debug("Prompt preview: {PromptPreview}", preview);

            var request = new
            {
                model = "claude-sonnet-4-5-20250929",
                max_tokens = 4096,
                messages = new[]
                {
            new
            {
                role = "user",
                content = prompt
            }
        }
            };

            using var msg = new HttpRequestMessage(HttpMethod.Post, "v1/messages");
            msg.Headers.Add("x-api-key", apiKey);
            msg.Headers.Add("anthropic-version", "2023-06-01");

            var payload = JsonSerializer.Serialize(request);
            msg.Content = new StringContent(payload, Encoding.UTF8, "application/json");

            Log.Debug("Sending HTTP request to Claude API. PayloadSize={PayloadSizeBytes}", payload?.Length ?? 0);

            try
            {
                using var resp = await _httpClaude.SendAsync(msg, cancellationToken).ConfigureAwait(false);
                var json = await resp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                Log.Debug("HTTP response received. StatusCode={StatusCode}, ResponseSize={ResponseSizeBytes}", resp.StatusCode, json?.Length ?? 0);

                if (!resp.IsSuccessStatusCode)
                {
                    Log.Error("Claude API returned non-success status. StatusCode={StatusCode} Reason={ReasonPhrase}", (int)resp.StatusCode, resp.ReasonPhrase);
                    Log.Debug("Claude error body: {ResponseBody}", json);
                    return $"Error: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}";
                }

                using var doc = JsonDocument.Parse(json);
                Log.Debug("Parsed JSON response.");

                // Claude API response structure: content[0].text
                if (doc.RootElement.TryGetProperty("content", out var content) &&
                    content.ValueKind == JsonValueKind.Array &&
                    content.GetArrayLength() > 0)
                {
                    var first = content[0];
                    if (first.TryGetProperty("text", out var textEl))
                    {
                        var result = textEl.GetString() ?? "";
                        Log.Information("CallClaudeAsync completed successfully. OutputLength={OutputLength}", result.Length);
                        return result;
                    }
                }

                Log.Warning("Response did not contain expected 'content[0].text'; returning raw JSON.");
                return json;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // User pressed Stop — expected, not a failure. An HttpClient timeout also
                // surfaces as OperationCanceledException, so the token guard keeps the two apart.
                Log.Information("CallClaudeAsync cancelled by the user.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while calling Claude API");
                throw;
            }
        }


        public static async Task<string> ListModelsAsync(string apiKey)
        {
            // GET /v1/models :contentReference[oaicite:5]{index=5}
            using var msg = new HttpRequestMessage(HttpMethod.Get, "v1/models");
            msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            using var resp = await _httpClaude.SendAsync(msg).ConfigureAwait(false);
            var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode)
                return $"Error: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{json}";

            return json;
        }

        private async void SendBtn_Click(object sender, EventArgs e)
        {
            //check if textbox is empty 
            if (string.IsNullOrWhiteSpace(TextInput.Text))
            {
                MessageBox.Show("Please enter a prompt before sending.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text) && !jsonResumeCheck.Checked)
            {
                MessageBox.Show("Please select a json resume before sending.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!openAICheckBox.Checked && !claudeCheckBox.Checked)
            {
                MessageBox.Show("Please select an AI model (OpenAI or Claude).", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var useOpenAi = openAICheckBox.Checked;
            var apiKeySetting = ApiKeySettingFor(useOpenAi);

            // The base resume path only matters when the user asked for the base resume;
            // otherwise the path comes from textBox1, which is already validated above.
            var required = jsonResumeCheck.Checked
                ? new[] { apiKeySetting, "BASE_RESUME_FILE_NAME" }
                : new[] { apiKeySetting };

            if (!EnsureSettings(required))
                return;

            var apiKey = RequiredSettings.Get(apiKeySetting);

            _resumeCancellationTokenSource?.Dispose();
            _resumeCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _resumeCancellationTokenSource.Token;

            BeginResumeWork();
            TextOutput.Text = "Loading...";
            SetBusy(useOpenAi
                ? "Tailoring resume with OpenAI…"
                : "Tailoring resume with Claude…");

            try
            {
                var resumePath = jsonResumeCheck.Checked
                    ? RequiredSettings.Get("BASE_RESUME_FILE_NAME")
                    : textBox1.Text;

                var resumeJson = await File.ReadAllTextAsync(resumePath, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
                var prompt = AtsResumePromptBuilder.Build(TextInput.Text, resumeJson);

                cancellationToken.ThrowIfCancellationRequested();
                var result = useOpenAi
                    ? await CallOpenAiAsync(prompt, apiKey, cancellationToken)
                    : await CallClaudeAsync(prompt, apiKey, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
                RenderAiResult(result, TextOutput,
                    "Resume tailored — generating PDF…", "Resume tailoring failed");
            }
            catch (OperationCanceledException)
            {
                Log.Information("Resume tailoring stopped by the user.");
                DiscardResumeOutput();
                SetIdle("Resume generation stopped.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Resume tailoring failed");
                TextOutput.Text = string.Empty;
                SetIdle($"Resume tailoring failed: {ex.Message}", isError: true);
            }
            finally
            {
                EndResumeWork();
            }
        }

        /// <summary>
        /// Puts the resume half of the form into its running state: both start buttons off,
        /// the stop button showing. Paired with <see cref="EndResumeWork"/> in a finally block.
        /// </summary>
        private void BeginResumeWork()
        {
            SendBtn.Enabled = false;
            Generate_Rsme.Enabled = false;
            StopResumeBtn.Visible = true;
            StopResumeBtn.Enabled = true;
        }

        /// <summary>
        /// Restores the resume controls and releases the resume cancellation source, whichever
        /// way the flow ended.
        /// </summary>
        private void EndResumeWork()
        {
            _resumeCancellationTokenSource?.Dispose();
            _resumeCancellationTokenSource = null;

            StopResumeBtn.Enabled = false;
            StopResumeBtn.Visible = false;
            SendBtn.Enabled = true;
            Generate_Rsme.Enabled = true;
        }

        /// <summary>
        /// Clears the half-finished tailoring output and cancels the debounce tick that the
        /// clearing itself queued, so a stop never rolls on into PDF generation.
        /// </summary>
        private void DiscardResumeOutput()
        {
            TextOutput.Text = string.Empty;
            JsonCV.Text = string.Empty;
            _debounceTimer.Stop();
        }

        /// <summary>
        /// Renders a completed AI call into the output box and reflects success/failure in the
        /// status bar. Called on the UI thread after awaiting the API helper, so no marshalling
        /// is needed; re-enabling the triggering button is the caller's <c>finally</c> block.
        /// </summary>
        private void RenderAiResult(string result, TextBox output,
            string successStatus, string failureStatus, bool copyToClipboard = false)
        {
            output.Text = result;
            var failed = result.StartsWith("Error:");

            if (!failed && copyToClipboard && !string.IsNullOrEmpty(result))
            {
                try
                {
                    Clipboard.SetText(result);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Failed to copy AI output to clipboard");
                }
            }

            SetIdle(failed ? $"{failureStatus} — see the output box for details." : successStatus, isError: failed);
        }

        /// <summary>
        /// Guards a workflow behind the environment variables it needs, naming every missing one.
        /// Returns false when the caller should stop before doing any work; once it returns true,
        /// <see cref="RequiredSettings.Get"/> is safe for each of those names.
        /// </summary>
        private bool EnsureSettings(params string[] names)
        {
            var missing = RequiredSettings.FindMissing(names);

            if (missing.Count == 0)
                return true;

            Log.Warning("Action blocked — missing configuration: {MissingSettings}", string.Join(", ", missing));
            MessageBox.Show(this, RequiredSettings.DescribeMissing(missing),
                "Configuration Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return false;
        }

        /// <summary>
        /// Names the API key setting for the model the user picked.
        /// </summary>
        private static string ApiKeySettingFor(bool useOpenAi) =>
            useOpenAi ? "OPENAI_API_KEY" : "CLAUDIUS_API_KEY";

        private void SetBusy(string message)
        {
            statusLabel.ForeColor = SystemColors.ControlText;
            statusLabel.Text = message;
            statusProgress.Visible = true;
        }

        private void SetIdle(string message, bool isError = false)
        {
            statusProgress.Visible = false;
            statusLabel.ForeColor = isError ? Color.Firebrick : SystemColors.ControlText;
            statusLabel.Text = message;
        }

        private void JsonCV_DoubleClick(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Select a JSON file";
                ofd.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                ofd.Multiselect = false;
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;

                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    // Read and render file contents
                    var json = File.ReadAllText(ofd.FileName, Encoding.UTF8);
                    JsonCV.Text = json;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Failed to load file:\n" + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void Generate_Rsme_Click(object sender, EventArgs e)
        {
            // check if JsonCV is empty
            if (string.IsNullOrWhiteSpace(JsonCV.Text) && !_isUpdating)
            {
                MessageBox.Show("Please load a JSON CV before generating the resume.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (JsonCV.Text == "Loading...") return;

            // Checked before Path.Combine: a blank value there throws ArgumentNullException
            // outside the try below, which would take the whole app down from an async void handler.
            if (!EnsureSettings("RESUME_DOWNLOAD_PATH", "RESUME_FILE_NAME"))
                return;

            var downloadsFolder = Path.Combine(
                RequiredSettings.Get("RESUME_DOWNLOAD_PATH"),
                RequiredSettings.Get("RESUME_FILE_NAME"));

            _resumeCancellationTokenSource?.Dispose();
            _resumeCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _resumeCancellationTokenSource.Token;

            BeginResumeWork();
            SetBusy("Generating PDF via resumake.io…");

            try
            {
                await ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(
                    JsonCV.Text,
                    downloadsFolder,
                    headless: true,
                    cancellationToken);

                SetIdle($"PDF saved to {downloadsFolder}");
                MessageBox.Show(this, "Resume generated successfully",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.Information("Resume generated successfully at {OutputPath}", downloadsFolder);
            }
            catch (OperationCanceledException)
            {
                Log.Information("PDF generation stopped by the user.");
                SetIdle("PDF generation stopped.");
            }
            catch (Exception ex)
            {
                SetIdle("PDF generation failed: " + ex.Message, isError: true);
                MessageBox.Show(this, "Failed to generate resume:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Log.Error(ex, "Resume generation failed");
            }
            finally
            {
                EndResumeWork();
            }
        }

        private void StopResumeBtn_Click(object sender, EventArgs e)
        {
            // Disable first: the flow's finally block hides the button, but that only runs once
            // the cancellation has actually unwound, so this blocks repeat clicks meanwhile.
            StopResumeBtn.Enabled = false;
            Log.Information("Stop requested for the resume flow.");
            _resumeCancellationTokenSource?.Cancel();
        }

        private void StopProposalBtn_Click(object sender, EventArgs e)
        {
            StopProposalBtn.Enabled = false;
            Log.Information("Stop requested for the Upwork proposal flow.");
            _proposalCancellationTokenSource?.Cancel();
        }

        private void TextOutput_TextChanged(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void Jobs_List_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void Upwk_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UptextInput.Text))
            {
                MessageBox.Show("Please enter a prompt before sending.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Jobs_List.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a job title.");
                return;
            }
            if (!openAICheckBox.Checked && !claudeCheckBox.Checked)
            {
                MessageBox.Show("Please select an AI model (OpenAI or Claude).", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var useOpenAi = openAICheckBox.Checked;
            var apiKeySetting = ApiKeySettingFor(useOpenAi);

            if (!EnsureSettings(apiKeySetting))
                return;

            var apiKey = RequiredSettings.Get(apiKeySetting);

            _proposalCancellationTokenSource?.Dispose();
            _proposalCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _proposalCancellationTokenSource.Token;

            Upwk_btn.Enabled = false;
            StopProposalBtn.Visible = true;
            StopProposalBtn.Enabled = true;
            UptextOutput.Text = "Loading...";
            SetBusy(useOpenAi
                ? "Generating proposal with OpenAI…"
                : "Generating proposal with Claude…");

            IEnumerable<string> runtimeRules = MoreRulesBox.CheckedItems.Cast<CheckBoxRuleItem>().Select(x => x.Value);

            var selectedJob = Jobs_List.SelectedValue.ToString();

            var loomUrl = selectedJob switch
            {
                "Technical Writer" => technicalWriterLoomUrl,
                "AI Developer" => aiDeveloperLoomUrl,
                _ => developerLoomUrl
            };

            var projectHighlights = selectedJob switch
            {
                "AI Developer" => new List<ProjectHighlight>
                {
                    new() { Name = "Modak Web", PictureUrl = modakWebPic, Description = modakWebDesc },
                    new() { Name = "Help Me Rad Bridge", PictureUrl = helpMeRadBridgePic, Description = helpMeRadBridgeDesc },
                    new() { Name = "Job Search Builder", PictureUrl = jobSearchBuilderPic, Description = jobSearchBuilderDesc }
                },
                "Desktop Developer" => new List<ProjectHighlight>
                {
                    new() { Name = "Job Search Builder", PictureUrl = jobSearchBuilderPic, Description = jobSearchBuilderDesc }
                },
                _ => null
            };

            try
            {
                string prompt = AtsResumePromptBuilder.BuildUpwork(new UpworkProposalRequest
                {
                    JobDescription = UptextInput.Text,
                    JobType = selectedJob,
                    LoomUrl = loomUrl,
                    RuntimeRules = runtimeRules,
                    ProjectHighlights = projectHighlights
                });

                cancellationToken.ThrowIfCancellationRequested();
                var result = useOpenAi
                    ? await CallOpenAiAsync(prompt, apiKey, cancellationToken)
                    : await CallClaudeAsync(prompt, apiKey, cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();
                RenderAiResult(result, UptextOutput,
                    "Proposal generated and copied to your clipboard.", "Proposal generation failed",
                    copyToClipboard: true);
            }
            catch (OperationCanceledException)
            {
                Log.Information("Proposal generation stopped by the user.");

                // Clear only the placeholder — any proposal already in the box is the user's.
                if (UptextOutput.Text == "Loading...")
                    UptextOutput.Text = string.Empty;

                SetIdle("Proposal generation stopped.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Proposal generation failed");
                UptextOutput.Text = string.Empty;
                SetIdle($"Proposal generation failed: {ex.Message}", isError: true);
            }
            finally
            {
                _proposalCancellationTokenSource?.Dispose();
                _proposalCancellationTokenSource = null;

                StopProposalBtn.Enabled = false;
                StopProposalBtn.Visible = false;
                Upwk_btn.Enabled = true;
            }
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Prefer the designer OpenFileDialog if it exists, otherwise create a transient one.
                var dialog = openFileDialog1 ?? new OpenFileDialog();

                dialog.Title = "Select a file or link";
                dialog.Filter = "All files (*.*)|*.*";
                dialog.Multiselect = false;
                dialog.CheckFileExists = false;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                // Set the chosen path/link to the textbox
                textBox1.Text = dialog.FileName;
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to select file/link in textBox1 double-click");
                MessageBox.Show(this, "Failed to select file or link:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void claudeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (claudeCheckBox.Checked)
            {
                openAICheckBox.Checked = false;
            }
        }

        private void openAICheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(openAICheckBox.Checked)
            {
                claudeCheckBox.Checked = false;
            }
        }
    }
}
