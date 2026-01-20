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
            TextInput = new TextBox();
            TextOutput = new TextBox();
            SendBtn = new Button();
            JsonCV = new TextBox();
            Generate_Rsme = new Button();
            splitContainer1 = new SplitContainer();
            ResumeLabl = new Label();
            UpworkLabl = new Label();
            Jobs_List = new ComboBox();
            Upwk_btn = new Button();
            UptextOutput = new TextBox();
            UptextInput = new TextBox();
            MoreRulesBox = new CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // TextInput
            // 
            TextInput.Location = new Point(12, 135);
            TextInput.Multiline = true;
            TextInput.Name = "TextInput";
            TextInput.ScrollBars = ScrollBars.Both;
            TextInput.Size = new Size(427, 153);
            TextInput.TabIndex = 0;
            // 
            // TextOutput
            // 
            TextOutput.Location = new Point(12, 477);
            TextOutput.Multiline = true;
            TextOutput.Name = "TextOutput";
            TextOutput.ReadOnly = true;
            TextOutput.ScrollBars = ScrollBars.Both;
            TextOutput.Size = new Size(561, 243);
            TextOutput.TabIndex = 1;
            TextOutput.TextChanged += TextOutput_TextChanged;
            // 
            // SendBtn
            // 
            SendBtn.Location = new Point(28, 333);
            SendBtn.Name = "SendBtn";
            SendBtn.Size = new Size(112, 34);
            SendBtn.TabIndex = 2;
            SendBtn.Text = "Send";
            SendBtn.UseVisualStyleBackColor = true;
            SendBtn.Click += SendBtn_Click;
            // 
            // JsonCV
            // 
            JsonCV.Location = new Point(526, 135);
            JsonCV.Multiline = true;
            JsonCV.Name = "JsonCV";
            JsonCV.ReadOnly = true;
            JsonCV.ScrollBars = ScrollBars.Both;
            JsonCV.Size = new Size(236, 160);
            JsonCV.TabIndex = 3;
            JsonCV.WordWrap = false;
            JsonCV.DoubleClick += JsonCV_DoubleClick;
            // 
            // Generate_Rsme
            // 
            Generate_Rsme.Location = new Point(582, 301);
            Generate_Rsme.Name = "Generate_Rsme";
            Generate_Rsme.Size = new Size(180, 50);
            Generate_Rsme.TabIndex = 4;
            Generate_Rsme.Text = "Generate Resume";
            Generate_Rsme.UseVisualStyleBackColor = true;
            Generate_Rsme.Click += Generate_Rsme_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.Fixed3D;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(25, 21);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(ResumeLabl);
            splitContainer1.Panel1.Controls.Add(JsonCV);
            splitContainer1.Panel1.Controls.Add(Generate_Rsme);
            splitContainer1.Panel1.Controls.Add(TextOutput);
            splitContainer1.Panel1.Controls.Add(SendBtn);
            splitContainer1.Panel1.Controls.Add(TextInput);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(MoreRulesBox);
            splitContainer1.Panel2.Controls.Add(UpworkLabl);
            splitContainer1.Panel2.Controls.Add(Jobs_List);
            splitContainer1.Panel2.Controls.Add(Upwk_btn);
            splitContainer1.Panel2.Controls.Add(UptextOutput);
            splitContainer1.Panel2.Controls.Add(UptextInput);
            splitContainer1.Size = new Size(1543, 746);
            splitContainer1.SplitterDistance = 831;
            splitContainer1.TabIndex = 5;
            // 
            // ResumeLabl
            // 
            ResumeLabl.AutoSize = true;
            ResumeLabl.Location = new Point(69, 34);
            ResumeLabl.Name = "ResumeLabl";
            ResumeLabl.Size = new Size(157, 25);
            ResumeLabl.TabIndex = 5;
            ResumeLabl.Text = "Configure Resume";
            // 
            // UpworkLabl
            // 
            UpworkLabl.AutoSize = true;
            UpworkLabl.Location = new Point(85, 34);
            UpworkLabl.Name = "UpworkLabl";
            UpworkLabl.Size = new Size(157, 25);
            UpworkLabl.TabIndex = 6;
            UpworkLabl.Text = "Configure Upwork";
            // 
            // Jobs_List
            // 
            Jobs_List.FormattingEnabled = true;
            Jobs_List.Location = new Point(509, 323);
            Jobs_List.Name = "Jobs_List";
            Jobs_List.Size = new Size(182, 33);
            Jobs_List.TabIndex = 3;
            Jobs_List.SelectedIndexChanged += Jobs_List_SelectedIndexChanged;
            // 
            // Upwk_btn
            // 
            Upwk_btn.Location = new Point(61, 321);
            Upwk_btn.Name = "Upwk_btn";
            Upwk_btn.Size = new Size(148, 34);
            Upwk_btn.TabIndex = 2;
            Upwk_btn.Text = "Send Prompt";
            Upwk_btn.UseVisualStyleBackColor = true;
            Upwk_btn.Click += Upwk_btn_Click;
            // 
            // UptextOutput
            // 
            UptextOutput.Location = new Point(23, 477);
            UptextOutput.Multiline = true;
            UptextOutput.Name = "UptextOutput";
            UptextOutput.ReadOnly = true;
            UptextOutput.ScrollBars = ScrollBars.Both;
            UptextOutput.Size = new Size(469, 243);
            UptextOutput.TabIndex = 1;
            // 
            // UptextInput
            // 
            UptextInput.Location = new Point(61, 123);
            UptextInput.Multiline = true;
            UptextInput.Name = "UptextInput";
            UptextInput.ScrollBars = ScrollBars.Both;
            UptextInput.Size = new Size(469, 153);
            UptextInput.TabIndex = 0;
            // 
            // MoreRulesBox
            // 
            MoreRulesBox.FormattingEnabled = true;
            MoreRulesBox.Location = new Point(511, 370);
            MoreRulesBox.Name = "MoreRulesBox";
            MoreRulesBox.Size = new Size(180, 144);
            MoreRulesBox.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1601, 797);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TextBox TextInput;
        private TextBox TextOutput;
        private Button SendBtn;
        private TextBox JsonCV;
        private Button Generate_Rsme;
        private SplitContainer splitContainer1;
        private TextBox UptextInput;
        private TextBox UptextOutput;
        private Button Upwk_btn;
        private ComboBox Jobs_List;
        private Label ResumeLabl;
        private Label UpworkLabl;
        private CheckedListBox MoreRulesBox;
    }
}
