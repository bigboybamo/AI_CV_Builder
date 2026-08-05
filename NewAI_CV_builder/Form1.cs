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
        private readonly string? openAIApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        private readonly string? claudeApiKey = Environment.GetEnvironmentVariable("CLAUDIUS_API_KEY");
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

        public static async Task<string> CallOpenAiAsync(string prompt, string apiKey)
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
                using var resp = await _httpOpenAI.SendAsync(msg).ConfigureAwait(false);
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

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
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred while calling OpenAI API");
                throw;
            }
        }

        public static async Task<string> CallClaudeAsync(string prompt, string apiKey)
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
                using var resp = await _httpClaude.SendAsync(msg).ConfigureAwait(false);
                var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
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

        private void SendBtn_Click(object sender, EventArgs e)
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

            SendBtn.Enabled = false;
            TextOutput.Text = "Loading...";
            SetBusy(openAICheckBox.Checked
                ? "Tailoring resume with OpenAI…"
                : "Tailoring resume with Claude…");

            string resumeJson = "";
            string prompt = "";

            if (jsonResumeCheck.Checked)
            {
                resumeJson = File.ReadAllText(Environment.GetEnvironmentVariable("BASE_RESUME_FILE_NAME"));
                prompt = AtsResumePromptBuilder.Build(TextInput.Text, resumeJson);
            }
            else
            {
                resumeJson = File.ReadAllText(textBox1.Text);
                prompt = AtsResumePromptBuilder.Build(TextInput.Text, resumeJson);
            }

            if (openAICheckBox.Checked)
            {
                CallOpenAiAsync(prompt, openAIApiKey).ContinueWith(task =>
                    HandleAiCompletion(task, TextOutput, SendBtn,
                        "Resume tailored — generating PDF…", "Resume tailoring failed"));
            }
            else if (claudeCheckBox.Checked)
            {
                CallClaudeAsync(prompt, claudeApiKey).ContinueWith(task =>
                    HandleAiCompletion(task, TextOutput, SendBtn,
                        "Resume tailored — generating PDF…", "Resume tailoring failed"));
            }
            else
            {
                MessageBox.Show("Please select an AI model (OpenAI or Claude).", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SendBtn.Enabled = true;
                TextOutput.Text = "";
                SetIdle("Ready");
                return;
            }
        }

        /// <summary>
        /// Marshals an AI call result back onto the UI thread, updates the output box,
        /// re-enables the triggering button and reflects success/failure in the status bar.
        /// </summary>
        private void HandleAiCompletion(Task<string> task, TextBox output, Button trigger,
            string successStatus, string failureStatus, bool copyToClipboard = false)
        {
            this.Invoke((Action)(() =>
            {
                if (task.IsFaulted)
                {
                    output.Text = string.Empty;
                    var message = task.Exception?.GetBaseException().Message;
                    SetIdle($"{failureStatus}: {message}", isError: true);
                }
                else
                {
                    output.Text = task.Result;
                    var failed = task.Result.StartsWith("Error:");
                    if (!failed && copyToClipboard && !string.IsNullOrEmpty(task.Result))
                    {
                        try
                        {
                            Clipboard.SetText(task.Result);
                        }
                        catch (Exception ex)
                        {
                            Log.Warning(ex, "Failed to copy AI output to clipboard");
                        }
                    }
                    SetIdle(failed ? $"{failureStatus} — see the output box for details." : successStatus, isError: failed);
                }
                trigger.Enabled = true;
            }));
        }

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

        private void Generate_Rsme_Click(object sender, EventArgs e)
        {
            // check if JsonCV is empty
            if (string.IsNullOrWhiteSpace(JsonCV.Text) && !_isUpdating)
            {
                MessageBox.Show("Please load a JSON CV before generating the resume.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (JsonCV.Text == "Loading...") return;

            var downloadfilePath = Environment.GetEnvironmentVariable("RESUME_DOWNLOAD_PATH");
            var resumeFileName = Environment.GetEnvironmentVariable("RESUME_FILE_NAME");

            var downloadsFolder = Path.Combine(downloadfilePath, resumeFileName);

            SetBusy("Generating PDF via resumake.io…");

            ResumakePlaywrightFlow.GeneratePdfFromJsonAsync(
                JsonCV.Text,
                downloadsFolder,
                headless: true).ContinueWith(task =>
            {
                // Update the UI on the main thread
                this.Invoke((Action)(() =>
                {
                    if (task.IsFaulted)
                    {
                        SetIdle("PDF generation failed: " + task.Exception?.GetBaseException().Message, isError: true);
                        MessageBox.Show(this, "Failed to generate resume:\n" + task.Exception?.GetBaseException().Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        Log.Error("Resume generation failed: {0}", task.Exception?.GetBaseException().Message);
                    }
                    else
                    {
                        SetIdle($"PDF saved to {downloadsFolder}");
                        MessageBox.Show(this, "Resume generated successfully",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Log.Information("Resume generated successfully at {0}", downloadsFolder);
                    }
                }));
            });
        }

        private void TextOutput_TextChanged(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void Jobs_List_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Upwk_btn_Click(object sender, EventArgs e)
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
            //Call the OpenAI API asynchronously and update the UI when done
            Upwk_btn.Enabled = false;
            UptextOutput.Text = "Loading...";
            SetBusy(openAICheckBox.Checked
                ? "Generating proposal with OpenAI…"
                : "Generating proposal with Claude…");

            // call claude api asynchronously and update the UI when done
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

            string prompt = AtsResumePromptBuilder.BuildUpwork(new UpworkProposalRequest
            {
                JobDescription = UptextInput.Text,
                JobType = selectedJob,
                LoomUrl = loomUrl,
                RuntimeRules = runtimeRules,
                ProjectHighlights = projectHighlights
            });

            if (openAICheckBox.Checked)
            {
                CallOpenAiAsync(prompt, openAIApiKey).ContinueWith(task =>
                    HandleAiCompletion(task, UptextOutput, Upwk_btn,
                        "Proposal generated and copied to your clipboard.", "Proposal generation failed",
                        copyToClipboard: true));
            }
            else if (claudeCheckBox.Checked)
            {
                CallClaudeAsync(prompt, claudeApiKey).ContinueWith(task =>
                    HandleAiCompletion(task, UptextOutput, Upwk_btn,
                        "Proposal generated and copied to your clipboard.", "Proposal generation failed",
                        copyToClipboard: true));
            }
            else
            {
                MessageBox.Show("Please select an AI model (OpenAI or Claude).", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Upwk_btn.Enabled = true;
                UptextOutput.Text = "";
                SetIdle("Ready");
                return;
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
