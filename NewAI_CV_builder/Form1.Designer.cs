namespace NewAI_CV_builder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            headerPanel = new Panel();
            appTitleLabel = new Label();
            appSubtitleLabel = new Label();
            providerFlow = new FlowLayoutPanel();
            providerLabel = new Label();
            openAICheckBox = new RadioButton();
            claudeCheckBox = new RadioButton();
            contentPanel = new Panel();
            splitContainer1 = new SplitContainer();
            resumeGroup = new GroupBox();
            resumeLayout = new TableLayoutPanel();
            ResumeLabl = new Label();
            TextInput = new TextBox();
            sourcePanel = new Panel();
            label1 = new Label();
            jsonResumeCheck = new CheckBox();
            textBox1 = new TextBox();
            actionsFlow = new FlowLayoutPanel();
            Generate_Rsme = new Button();
            SendBtn = new Button();
            StopResumeBtn = new Button();
            lblAiOutput = new Label();
            lblJsonCv = new Label();
            TextOutput = new TextBox();
            JsonCV = new TextBox();
            upworkGroup = new GroupBox();
            upworkLayout = new TableLayoutPanel();
            UpworkLabl = new Label();
            UptextInput = new TextBox();
            upworkOptionsLayout = new TableLayoutPanel();
            jobTitlePanel = new Panel();
            lblJobTitle = new Label();
            Jobs_List = new ComboBox();
            Upwk_btn = new Button();
            StopProposalBtn = new Button();
            rulesPanel = new Panel();
            lblRules = new Label();
            MoreRulesBox = new CheckedListBox();
            lblUpOutput = new Label();
            UptextOutput = new TextBox();
            statusStrip1 = new StatusStrip();
            statusProgress = new ToolStripProgressBar();
            statusLabel = new ToolStripStatusLabel();
            openFileDialog1 = new OpenFileDialog();
            headerPanel.SuspendLayout();
            providerFlow.SuspendLayout();
            contentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            resumeGroup.SuspendLayout();
            resumeLayout.SuspendLayout();
            sourcePanel.SuspendLayout();
            actionsFlow.SuspendLayout();
            upworkGroup.SuspendLayout();
            upworkLayout.SuspendLayout();
            upworkOptionsLayout.SuspendLayout();
            jobTitlePanel.SuspendLayout();
            rulesPanel.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            //
            // headerPanel
            //
            headerPanel.BackColor = Color.FromArgb(17, 24, 39);
            headerPanel.Controls.Add(appTitleLabel);
            headerPanel.Controls.Add(appSubtitleLabel);
            headerPanel.Controls.Add(providerFlow);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(0, 0);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(1220, 64);
            headerPanel.TabIndex = 0;
            //
            // appTitleLabel
            //
            appTitleLabel.AutoSize = true;
            appTitleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            appTitleLabel.ForeColor = Color.White;
            appTitleLabel.Location = new Point(16, 8);
            appTitleLabel.Name = "appTitleLabel";
            appTitleLabel.Size = new Size(132, 25);
            appTitleLabel.TabIndex = 0;
            appTitleLabel.Text = "AI CV Builder";
            //
            // appSubtitleLabel
            //
            appSubtitleLabel.AutoSize = true;
            appSubtitleLabel.Font = new Font("Segoe UI", 9F);
            appSubtitleLabel.ForeColor = Color.FromArgb(156, 163, 175);
            appSubtitleLabel.Location = new Point(18, 38);
            appSubtitleLabel.Name = "appSubtitleLabel";
            appSubtitleLabel.Size = new Size(340, 15);
            appSubtitleLabel.TabIndex = 1;
            appSubtitleLabel.Text = "Tailor your resume for ATS and draft Upwork proposals with AI";
            //
            // providerFlow
            //
            providerFlow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            providerFlow.Controls.Add(providerLabel);
            providerFlow.Controls.Add(openAICheckBox);
            providerFlow.Controls.Add(claudeCheckBox);
            providerFlow.Location = new Point(890, 15);
            providerFlow.Name = "providerFlow";
            providerFlow.Size = new Size(318, 34);
            providerFlow.TabIndex = 2;
            //
            // providerLabel
            //
            providerLabel.AutoSize = true;
            providerLabel.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            providerLabel.ForeColor = Color.White;
            providerLabel.Location = new Point(3, 9);
            providerLabel.Margin = new Padding(3, 9, 3, 0);
            providerLabel.Name = "providerLabel";
            providerLabel.Size = new Size(80, 17);
            providerLabel.TabIndex = 0;
            providerLabel.Text = "AI Provider:";
            //
            // openAICheckBox
            //
            openAICheckBox.AutoSize = true;
            openAICheckBox.Font = new Font("Segoe UI", 9.75F);
            openAICheckBox.ForeColor = Color.White;
            openAICheckBox.Location = new Point(98, 7);
            openAICheckBox.Margin = new Padding(12, 7, 3, 0);
            openAICheckBox.Name = "openAICheckBox";
            openAICheckBox.Size = new Size(74, 21);
            openAICheckBox.TabIndex = 1;
            openAICheckBox.Text = "OpenAI";
            openAICheckBox.UseVisualStyleBackColor = true;
            openAICheckBox.CheckedChanged += openAICheckBox_CheckedChanged;
            //
            // claudeCheckBox
            //
            claudeCheckBox.AutoSize = true;
            claudeCheckBox.Checked = true;
            claudeCheckBox.Font = new Font("Segoe UI", 9.75F);
            claudeCheckBox.ForeColor = Color.White;
            claudeCheckBox.Location = new Point(187, 7);
            claudeCheckBox.Margin = new Padding(12, 7, 3, 0);
            claudeCheckBox.Name = "claudeCheckBox";
            claudeCheckBox.Size = new Size(70, 21);
            claudeCheckBox.TabIndex = 2;
            claudeCheckBox.TabStop = true;
            claudeCheckBox.Text = "Claude";
            claudeCheckBox.UseVisualStyleBackColor = true;
            claudeCheckBox.CheckedChanged += claudeCheckBox_CheckedChanged;
            //
            // contentPanel
            //
            contentPanel.Controls.Add(splitContainer1);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(0, 64);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(12, 10, 12, 10);
            contentPanel.Size = new Size(1220, 614);
            contentPanel.TabIndex = 1;
            //
            // splitContainer1
            //
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(12, 10);
            splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            splitContainer1.Panel1.Controls.Add(resumeGroup);
            splitContainer1.Panel1.Padding = new Padding(0, 0, 4, 0);
            //
            // splitContainer1.Panel2
            //
            splitContainer1.Panel2.Controls.Add(upworkGroup);
            splitContainer1.Panel2.Padding = new Padding(4, 0, 0, 0);
            splitContainer1.Panel1MinSize = 400;
            splitContainer1.Panel2MinSize = 400;
            splitContainer1.Size = new Size(1196, 614);
            splitContainer1.SplitterDistance = 594;
            splitContainer1.SplitterWidth = 8;
            splitContainer1.TabIndex = 0;
            //
            // resumeGroup
            //
            resumeGroup.Controls.Add(resumeLayout);
            resumeGroup.Dock = DockStyle.Fill;
            resumeGroup.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            resumeGroup.ForeColor = Color.FromArgb(31, 41, 55);
            resumeGroup.Location = new Point(0, 0);
            resumeGroup.Name = "resumeGroup";
            resumeGroup.Padding = new Padding(10, 4, 10, 10);
            resumeGroup.Size = new Size(684, 714);
            resumeGroup.TabIndex = 0;
            resumeGroup.TabStop = false;
            resumeGroup.Text = "Resume Tailoring (ATS)";
            //
            // resumeLayout
            //
            resumeLayout.ColumnCount = 2;
            resumeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            resumeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            resumeLayout.Controls.Add(ResumeLabl, 0, 0);
            resumeLayout.Controls.Add(TextInput, 0, 1);
            resumeLayout.Controls.Add(sourcePanel, 0, 2);
            resumeLayout.Controls.Add(actionsFlow, 1, 2);
            resumeLayout.Controls.Add(lblAiOutput, 0, 3);
            resumeLayout.Controls.Add(lblJsonCv, 1, 3);
            resumeLayout.Controls.Add(TextOutput, 0, 4);
            resumeLayout.Controls.Add(JsonCV, 1, 4);
            resumeLayout.Dock = DockStyle.Fill;
            resumeLayout.Font = new Font("Segoe UI", 9F);
            resumeLayout.Location = new Point(10, 25);
            resumeLayout.Name = "resumeLayout";
            resumeLayout.RowCount = 5;
            resumeLayout.RowStyles.Add(new RowStyle());
            resumeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            resumeLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
            resumeLayout.RowStyles.Add(new RowStyle());
            resumeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            resumeLayout.SetColumnSpan(ResumeLabl, 2);
            resumeLayout.SetColumnSpan(TextInput, 2);
            resumeLayout.Size = new Size(664, 679);
            resumeLayout.TabIndex = 0;
            //
            // ResumeLabl
            //
            ResumeLabl.Anchor = AnchorStyles.Left;
            ResumeLabl.AutoSize = true;
            ResumeLabl.ForeColor = Color.FromArgb(107, 114, 128);
            ResumeLabl.Margin = new Padding(3, 6, 3, 2);
            ResumeLabl.Name = "ResumeLabl";
            ResumeLabl.Size = new Size(200, 15);
            ResumeLabl.TabIndex = 0;
            ResumeLabl.Text = "Step 1 — Paste the job description";
            //
            // TextInput
            //
            TextInput.BorderStyle = BorderStyle.FixedSingle;
            TextInput.Dock = DockStyle.Fill;
            TextInput.Location = new Point(3, 26);
            TextInput.Margin = new Padding(3, 3, 3, 10);
            TextInput.Multiline = true;
            TextInput.Name = "TextInput";
            TextInput.PlaceholderText = "Paste the job description here…";
            TextInput.ScrollBars = ScrollBars.Both;
            TextInput.Size = new Size(658, 250);
            TextInput.TabIndex = 1;
            //
            // sourcePanel
            //
            sourcePanel.Controls.Add(label1);
            sourcePanel.Controls.Add(jsonResumeCheck);
            sourcePanel.Controls.Add(textBox1);
            sourcePanel.Dock = DockStyle.Fill;
            sourcePanel.Location = new Point(3, 289);
            sourcePanel.Name = "sourcePanel";
            sourcePanel.Size = new Size(326, 72);
            sourcePanel.TabIndex = 2;
            //
            // label1
            //
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(107, 114, 128);
            label1.Location = new Point(0, 2);
            label1.Name = "label1";
            label1.Size = new Size(160, 15);
            label1.TabIndex = 0;
            label1.Text = "Step 2 — Resume source";
            //
            // jsonResumeCheck
            //
            jsonResumeCheck.AutoSize = true;
            jsonResumeCheck.Checked = true;
            jsonResumeCheck.CheckState = CheckState.Checked;
            jsonResumeCheck.Location = new Point(0, 22);
            jsonResumeCheck.Name = "jsonResumeCheck";
            jsonResumeCheck.Size = new Size(170, 19);
            jsonResumeCheck.TabIndex = 1;
            jsonResumeCheck.Text = "Use base resume from .env";
            jsonResumeCheck.UseVisualStyleBackColor = true;
            //
            // textBox1
            //
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Dock = DockStyle.Bottom;
            textBox1.Location = new Point(0, 46);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "…or double-click to browse for a JSON resume file";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(320, 23);
            textBox1.TabIndex = 2;
            textBox1.DoubleClick += textBox1_DoubleClick;
            //
            // actionsFlow
            //
            actionsFlow.Controls.Add(Generate_Rsme);
            actionsFlow.Controls.Add(SendBtn);
            actionsFlow.Controls.Add(StopResumeBtn);
            actionsFlow.Dock = DockStyle.Fill;
            actionsFlow.FlowDirection = FlowDirection.RightToLeft;
            actionsFlow.Location = new Point(335, 289);
            actionsFlow.Name = "actionsFlow";
            actionsFlow.Size = new Size(326, 72);
            actionsFlow.TabIndex = 3;
            actionsFlow.WrapContents = false;
            //
            // Generate_Rsme
            //
            Generate_Rsme.BackColor = Color.FromArgb(22, 163, 74);
            Generate_Rsme.FlatAppearance.BorderSize = 0;
            Generate_Rsme.FlatStyle = FlatStyle.Flat;
            Generate_Rsme.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            Generate_Rsme.ForeColor = Color.White;
            Generate_Rsme.Location = new Point(173, 20);
            Generate_Rsme.Margin = new Padding(4, 20, 2, 3);
            Generate_Rsme.Name = "Generate_Rsme";
            Generate_Rsme.Size = new Size(105, 38);
            Generate_Rsme.TabIndex = 1;
            Generate_Rsme.Text = "Generate PDF";
            Generate_Rsme.UseVisualStyleBackColor = false;
            Generate_Rsme.Click += Generate_Rsme_Click;
            //
            // SendBtn
            //
            SendBtn.BackColor = Color.FromArgb(37, 99, 235);
            SendBtn.FlatAppearance.BorderSize = 0;
            SendBtn.FlatStyle = FlatStyle.Flat;
            SendBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            SendBtn.ForeColor = Color.White;
            SendBtn.Location = new Point(15, 20);
            SendBtn.Margin = new Padding(4, 20, 2, 3);
            SendBtn.Name = "SendBtn";
            SendBtn.Size = new Size(105, 38);
            SendBtn.TabIndex = 0;
            SendBtn.Text = "Tailor with AI";
            SendBtn.UseVisualStyleBackColor = false;
            SendBtn.Click += SendBtn_Click;
            //
            // StopResumeBtn
            //
            StopResumeBtn.BackColor = Color.FromArgb(220, 38, 38);
            StopResumeBtn.Enabled = false;
            StopResumeBtn.FlatAppearance.BorderSize = 0;
            StopResumeBtn.FlatStyle = FlatStyle.Flat;
            StopResumeBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            StopResumeBtn.ForeColor = Color.White;
            StopResumeBtn.Location = new Point(3, 20);
            StopResumeBtn.Margin = new Padding(4, 20, 2, 3);
            StopResumeBtn.Name = "StopResumeBtn";
            StopResumeBtn.Size = new Size(56, 38);
            StopResumeBtn.TabIndex = 2;
            StopResumeBtn.Text = "Stop";
            StopResumeBtn.UseVisualStyleBackColor = false;
            StopResumeBtn.Visible = false;
            StopResumeBtn.Click += StopResumeBtn_Click;
            //
            // lblAiOutput
            //
            lblAiOutput.Anchor = AnchorStyles.Left;
            lblAiOutput.AutoSize = true;
            lblAiOutput.ForeColor = Color.FromArgb(107, 114, 128);
            lblAiOutput.Margin = new Padding(3, 6, 3, 2);
            lblAiOutput.Name = "lblAiOutput";
            lblAiOutput.Size = new Size(60, 15);
            lblAiOutput.TabIndex = 4;
            lblAiOutput.Text = "AI output";
            //
            // lblJsonCv
            //
            lblJsonCv.Anchor = AnchorStyles.Left;
            lblJsonCv.AutoSize = true;
            lblJsonCv.ForeColor = Color.FromArgb(107, 114, 128);
            lblJsonCv.Margin = new Padding(3, 6, 3, 2);
            lblJsonCv.Name = "lblJsonCv";
            lblJsonCv.Size = new Size(220, 15);
            lblJsonCv.TabIndex = 5;
            lblJsonCv.Text = "Resume JSON (sent to PDF generator)";
            //
            // TextOutput
            //
            TextOutput.BorderStyle = BorderStyle.FixedSingle;
            TextOutput.Dock = DockStyle.Fill;
            TextOutput.Font = new Font("Consolas", 9F);
            TextOutput.Location = new Point(3, 386);
            TextOutput.Multiline = true;
            TextOutput.Name = "TextOutput";
            TextOutput.PlaceholderText = "The tailored resume from the AI appears here…";
            TextOutput.ReadOnly = true;
            TextOutput.ScrollBars = ScrollBars.Both;
            TextOutput.Size = new Size(326, 290);
            TextOutput.TabIndex = 6;
            TextOutput.TextChanged += TextOutput_TextChanged;
            //
            // JsonCV
            //
            JsonCV.BorderStyle = BorderStyle.FixedSingle;
            JsonCV.Dock = DockStyle.Fill;
            JsonCV.Font = new Font("Consolas", 9F);
            JsonCV.Location = new Point(335, 386);
            JsonCV.Multiline = true;
            JsonCV.Name = "JsonCV";
            JsonCV.PlaceholderText = "Optimised resume JSON appears here — double-click to load a JSON file manually";
            JsonCV.ReadOnly = true;
            JsonCV.ScrollBars = ScrollBars.Both;
            JsonCV.Size = new Size(326, 290);
            JsonCV.TabIndex = 7;
            JsonCV.WordWrap = false;
            JsonCV.DoubleClick += JsonCV_DoubleClick;
            //
            // upworkGroup
            //
            upworkGroup.Controls.Add(upworkLayout);
            upworkGroup.Dock = DockStyle.Fill;
            upworkGroup.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            upworkGroup.ForeColor = Color.FromArgb(31, 41, 55);
            upworkGroup.Location = new Point(4, 0);
            upworkGroup.Name = "upworkGroup";
            upworkGroup.Padding = new Padding(10, 4, 10, 10);
            upworkGroup.Size = new Size(676, 714);
            upworkGroup.TabIndex = 0;
            upworkGroup.TabStop = false;
            upworkGroup.Text = "Upwork Proposal Generator";
            //
            // upworkLayout
            //
            upworkLayout.ColumnCount = 1;
            upworkLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            upworkLayout.Controls.Add(UpworkLabl, 0, 0);
            upworkLayout.Controls.Add(UptextInput, 0, 1);
            upworkLayout.Controls.Add(upworkOptionsLayout, 0, 2);
            upworkLayout.Controls.Add(lblUpOutput, 0, 3);
            upworkLayout.Controls.Add(UptextOutput, 0, 4);
            upworkLayout.Dock = DockStyle.Fill;
            upworkLayout.Font = new Font("Segoe UI", 9F);
            upworkLayout.Location = new Point(10, 25);
            upworkLayout.Name = "upworkLayout";
            upworkLayout.RowCount = 5;
            upworkLayout.RowStyles.Add(new RowStyle());
            upworkLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            upworkLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            upworkLayout.RowStyles.Add(new RowStyle());
            upworkLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            upworkLayout.Size = new Size(656, 679);
            upworkLayout.TabIndex = 0;
            //
            // UpworkLabl
            //
            UpworkLabl.Anchor = AnchorStyles.Left;
            UpworkLabl.AutoSize = true;
            UpworkLabl.ForeColor = Color.FromArgb(107, 114, 128);
            UpworkLabl.Margin = new Padding(3, 6, 3, 2);
            UpworkLabl.Name = "UpworkLabl";
            UpworkLabl.Size = new Size(210, 15);
            UpworkLabl.TabIndex = 0;
            UpworkLabl.Text = "Step 1 — Paste the Upwork job post";
            //
            // UptextInput
            //
            UptextInput.BorderStyle = BorderStyle.FixedSingle;
            UptextInput.Dock = DockStyle.Fill;
            UptextInput.Location = new Point(3, 26);
            UptextInput.Margin = new Padding(3, 3, 3, 10);
            UptextInput.Multiline = true;
            UptextInput.Name = "UptextInput";
            UptextInput.PlaceholderText = "Paste the Upwork job post here…";
            UptextInput.ScrollBars = ScrollBars.Both;
            UptextInput.Size = new Size(650, 250);
            UptextInput.TabIndex = 1;
            //
            // upworkOptionsLayout
            //
            upworkOptionsLayout.ColumnCount = 2;
            upworkOptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            upworkOptionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            upworkOptionsLayout.Controls.Add(jobTitlePanel, 0, 0);
            upworkOptionsLayout.Controls.Add(rulesPanel, 1, 0);
            upworkOptionsLayout.Dock = DockStyle.Fill;
            upworkOptionsLayout.Location = new Point(3, 289);
            upworkOptionsLayout.Name = "upworkOptionsLayout";
            upworkOptionsLayout.RowCount = 1;
            upworkOptionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            upworkOptionsLayout.Size = new Size(650, 144);
            upworkOptionsLayout.TabIndex = 2;
            //
            // jobTitlePanel
            //
            jobTitlePanel.Controls.Add(lblJobTitle);
            jobTitlePanel.Controls.Add(Jobs_List);
            jobTitlePanel.Controls.Add(Upwk_btn);
            jobTitlePanel.Controls.Add(StopProposalBtn);
            jobTitlePanel.Dock = DockStyle.Fill;
            jobTitlePanel.Location = new Point(3, 3);
            jobTitlePanel.Name = "jobTitlePanel";
            jobTitlePanel.Size = new Size(286, 138);
            jobTitlePanel.TabIndex = 0;
            //
            // lblJobTitle
            //
            lblJobTitle.AutoSize = true;
            lblJobTitle.ForeColor = Color.FromArgb(107, 114, 128);
            lblJobTitle.Location = new Point(0, 2);
            lblJobTitle.Name = "lblJobTitle";
            lblJobTitle.Size = new Size(150, 15);
            lblJobTitle.TabIndex = 0;
            lblJobTitle.Text = "Step 2 — Select a job title";
            //
            // Jobs_List
            //
            Jobs_List.DropDownStyle = ComboBoxStyle.DropDownList;
            Jobs_List.FormattingEnabled = true;
            Jobs_List.Location = new Point(0, 22);
            Jobs_List.Name = "Jobs_List";
            Jobs_List.Size = new Size(240, 23);
            Jobs_List.TabIndex = 1;
            Jobs_List.SelectedIndexChanged += Jobs_List_SelectedIndexChanged;
            //
            // Upwk_btn
            //
            Upwk_btn.BackColor = Color.FromArgb(37, 99, 235);
            Upwk_btn.FlatAppearance.BorderSize = 0;
            Upwk_btn.FlatStyle = FlatStyle.Flat;
            Upwk_btn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            Upwk_btn.ForeColor = Color.White;
            Upwk_btn.Location = new Point(0, 62);
            Upwk_btn.Name = "Upwk_btn";
            Upwk_btn.Size = new Size(180, 38);
            Upwk_btn.TabIndex = 2;
            Upwk_btn.Text = "Generate Proposal";
            Upwk_btn.UseVisualStyleBackColor = false;
            Upwk_btn.Click += Upwk_btn_Click;
            //
            // StopProposalBtn
            //
            StopProposalBtn.BackColor = Color.FromArgb(220, 38, 38);
            StopProposalBtn.Enabled = false;
            StopProposalBtn.FlatAppearance.BorderSize = 0;
            StopProposalBtn.FlatStyle = FlatStyle.Flat;
            StopProposalBtn.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            StopProposalBtn.ForeColor = Color.White;
            StopProposalBtn.Location = new Point(186, 62);
            StopProposalBtn.Name = "StopProposalBtn";
            StopProposalBtn.Size = new Size(90, 38);
            StopProposalBtn.TabIndex = 3;
            StopProposalBtn.Text = "Stop";
            StopProposalBtn.UseVisualStyleBackColor = false;
            StopProposalBtn.Visible = false;
            StopProposalBtn.Click += StopProposalBtn_Click;
            //
            // rulesPanel
            //
            rulesPanel.Controls.Add(lblRules);
            rulesPanel.Controls.Add(MoreRulesBox);
            rulesPanel.Dock = DockStyle.Fill;
            rulesPanel.Location = new Point(295, 3);
            rulesPanel.Name = "rulesPanel";
            rulesPanel.Size = new Size(352, 138);
            rulesPanel.TabIndex = 1;
            //
            // lblRules
            //
            lblRules.AutoSize = true;
            lblRules.ForeColor = Color.FromArgb(107, 114, 128);
            lblRules.Location = new Point(0, 2);
            lblRules.Name = "lblRules";
            lblRules.Size = new Size(140, 15);
            lblRules.TabIndex = 0;
            lblRules.Text = "Optional proposal rules";
            //
            // MoreRulesBox
            //
            MoreRulesBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MoreRulesBox.BorderStyle = BorderStyle.FixedSingle;
            MoreRulesBox.CheckOnClick = true;
            MoreRulesBox.FormattingEnabled = true;
            MoreRulesBox.Location = new Point(0, 22);
            MoreRulesBox.Name = "MoreRulesBox";
            MoreRulesBox.Size = new Size(352, 112);
            MoreRulesBox.TabIndex = 1;
            //
            // lblUpOutput
            //
            lblUpOutput.Anchor = AnchorStyles.Left;
            lblUpOutput.AutoSize = true;
            lblUpOutput.ForeColor = Color.FromArgb(107, 114, 128);
            lblUpOutput.Margin = new Padding(3, 6, 3, 2);
            lblUpOutput.Name = "lblUpOutput";
            lblUpOutput.Size = new Size(320, 15);
            lblUpOutput.TabIndex = 3;
            lblUpOutput.Text = "Generated proposal — copied to your clipboard automatically";
            //
            // UptextOutput
            //
            UptextOutput.BorderStyle = BorderStyle.FixedSingle;
            UptextOutput.Dock = DockStyle.Fill;
            UptextOutput.Location = new Point(3, 461);
            UptextOutput.Multiline = true;
            UptextOutput.Name = "UptextOutput";
            UptextOutput.PlaceholderText = "Your proposal appears here and is copied to the clipboard…";
            UptextOutput.ReadOnly = true;
            UptextOutput.ScrollBars = ScrollBars.Both;
            UptextOutput.Size = new Size(650, 215);
            UptextOutput.TabIndex = 4;
            //
            // statusStrip1
            //
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusProgress, statusLabel });
            statusStrip1.Location = new Point(0, 678);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1220, 22);
            statusStrip1.TabIndex = 2;
            //
            // statusProgress
            //
            statusProgress.Name = "statusProgress";
            statusProgress.Size = new Size(140, 16);
            statusProgress.Style = ProgressBarStyle.Marquee;
            statusProgress.Visible = false;
            //
            // statusLabel
            //
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(39, 17);
            statusLabel.Text = "Ready";
            //
            // openFileDialog1
            //
            openFileDialog1.FileName = "openFileDialog1";
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 244, 246);
            ClientSize = new Size(1220, 700);
            Controls.Add(contentPanel);
            Controls.Add(statusStrip1);
            Controls.Add(headerPanel);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(1020, 640);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AI CV Builder";
            headerPanel.ResumeLayout(false);
            headerPanel.PerformLayout();
            providerFlow.ResumeLayout(false);
            providerFlow.PerformLayout();
            contentPanel.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            resumeGroup.ResumeLayout(false);
            resumeLayout.ResumeLayout(false);
            resumeLayout.PerformLayout();
            sourcePanel.ResumeLayout(false);
            sourcePanel.PerformLayout();
            actionsFlow.ResumeLayout(false);
            upworkGroup.ResumeLayout(false);
            upworkLayout.ResumeLayout(false);
            upworkLayout.PerformLayout();
            upworkOptionsLayout.ResumeLayout(false);
            jobTitlePanel.ResumeLayout(false);
            jobTitlePanel.PerformLayout();
            rulesPanel.ResumeLayout(false);
            rulesPanel.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel headerPanel;
        private Label appTitleLabel;
        private Label appSubtitleLabel;
        private FlowLayoutPanel providerFlow;
        private Label providerLabel;
        private RadioButton openAICheckBox;
        private RadioButton claudeCheckBox;
        private Panel contentPanel;
        private SplitContainer splitContainer1;
        private GroupBox resumeGroup;
        private TableLayoutPanel resumeLayout;
        private Label ResumeLabl;
        private TextBox TextInput;
        private Panel sourcePanel;
        private Label label1;
        private CheckBox jsonResumeCheck;
        private TextBox textBox1;
        private FlowLayoutPanel actionsFlow;
        private Button SendBtn;
        private Button Generate_Rsme;
        private Button StopResumeBtn;
        private Label lblAiOutput;
        private Label lblJsonCv;
        private TextBox TextOutput;
        private TextBox JsonCV;
        private GroupBox upworkGroup;
        private TableLayoutPanel upworkLayout;
        private Label UpworkLabl;
        private TextBox UptextInput;
        private TableLayoutPanel upworkOptionsLayout;
        private Panel jobTitlePanel;
        private Label lblJobTitle;
        private ComboBox Jobs_List;
        private Button Upwk_btn;
        private Button StopProposalBtn;
        private Panel rulesPanel;
        private Label lblRules;
        private CheckedListBox MoreRulesBox;
        private Label lblUpOutput;
        private TextBox UptextOutput;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar statusProgress;
        private ToolStripStatusLabel statusLabel;
        private OpenFileDialog openFileDialog1;
    }
}
