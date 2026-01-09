using NewAI_CV_builder.Services;
using NewAI_CV_builder.Utilities;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace NewAI_CV_builder
{
    public partial class Form1 : Form
    {
        private readonly string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        private readonly string? claudeApiKey = Environment.GetEnvironmentVariable("CLAUDIUS_API_KEY");
        private readonly System.Windows.Forms.Timer _debounceTimer = new System.Windows.Forms.Timer();
        private bool _isUpdating;
        private readonly List<string> _jobTitles;

        public Form1()
        {
            InitializeComponent();
            _jobTitles = new List<string> { "-- Select a job title --", "Web Developer", "Desktop Developer", "Technical Writer" };
            Jobs_List.DataSource = _jobTitles;
            Jobs_List.SelectedIndex = 0;
            _debounceTimer.Interval = 300; // ms
            _debounceTimer.Tick += DebounceTimer_Tick;
        }
        private static readonly HttpClient _http = new HttpClient
        {
            //BaseAddress = new Uri("https://api.openai.com/")
            BaseAddress = new Uri("https://api.anthropic.com/")
        };

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();

            if (_isUpdating) return;

            _isUpdating = true;
            JsonCV.Text = StripMarkdownCodeFence(TextOutput.Text);
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
                using var resp = await _http.SendAsync(msg).ConfigureAwait(false);
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
                using var resp = await _http.SendAsync(msg).ConfigureAwait(false);
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

            using var resp = await _http.SendAsync(msg).ConfigureAwait(false);
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

            //Call the OpenAI API asynchronously and update the UI when done

            SendBtn.Enabled = false;
            TextOutput.Text = "Loading...";
            //CallOpenAiAsync(TextInput.Text, apiKey).ContinueWith(task =>
            //{
            //    // Update the UI on the main thread
            // call claude api asynchronously and update the UI when done

            string resumeJson = File.ReadAllText(Environment.GetEnvironmentVariable("BASE_RESUME_FILE_NAME"));

            string prompt = AtsResumePromptBuilder.Build(TextInput.Text, resumeJson);

            CallClaudeAsync(prompt, claudeApiKey).ContinueWith(task =>
            {
                // Update the UI on the main thread
                this.Invoke((Action)(() =>
                {
                    TextOutput.Text = task.Result;
                    SendBtn.Enabled = true;
                }));
            });
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
                        MessageBox.Show(this, "Failed to generate resume:\n" + task.Exception?.GetBaseException().Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        Log.Error("Resume generation failed: {0}", task.Exception?.GetBaseException().Message);
                    }
                    else
                    {
                        MessageBox.Show(this, "Resume generated successfully",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Log.Information("Resume generated successfully at {0}", downloadsFolder);
                    }
                }));
            });
        }

        private static string StripMarkdownCodeFence(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var text = input.Trim();

            // Fast path: if it doesn't even contain a fence, return as-is
            if (!text.Contains("```"))
                return text;

            // Remove opening fence: ```json, ```JSON, ``` etc (at start)
            text = Regex.Replace(text, @"^\s*```[a-zA-Z0-9_-]*\s*\r?\n", string.Empty);

            // Remove closing fence (at end)
            text = Regex.Replace(text, @"\r?\n\s*```\s*$", string.Empty);

            return text.Trim();
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
            //CallOpenAiAsync(TextInput.Text, apiKey).ContinueWith(task =>
            //{
            //    // Update the UI on the main thread
            // call claude api asynchronously and update the UI when done

            var selectedJob = Jobs_List.SelectedValue.ToString();

            string prompt = AtsResumePromptBuilder.BuildUpwork(UptextInput.Text, selectedJob);

            CallClaudeAsync(prompt, claudeApiKey).ContinueWith(task =>
            {
                // Update the UI on the main thread
                this.Invoke((Action)(() =>
                {
                    UptextOutput.Text = task.Result;
                    Upwk_btn.Enabled = true;
                }));
            });

        }
    }
}
