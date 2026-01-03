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
            SuspendLayout();
            // 
            // TextInput
            // 
            TextInput.Location = new Point(54, 58);
            TextInput.Multiline = true;
            TextInput.Name = "TextInput";
            TextInput.Size = new Size(427, 115);
            TextInput.TabIndex = 0;
            // 
            // TextOutput
            // 
            TextOutput.Location = new Point(54, 290);
            TextOutput.Multiline = true;
            TextOutput.Name = "TextOutput";
            TextOutput.ReadOnly = true;
            TextOutput.Size = new Size(561, 243);
            TextOutput.TabIndex = 1;
            TextOutput.TextChanged += TextOutput_TextChanged;
            // 
            // SendBtn
            // 
            SendBtn.Location = new Point(54, 211);
            SendBtn.Name = "SendBtn";
            SendBtn.Size = new Size(112, 34);
            SendBtn.TabIndex = 2;
            SendBtn.Text = "Send";
            SendBtn.UseVisualStyleBackColor = true;
            SendBtn.Click += SendBtn_Click;
            // 
            // JsonCV
            // 
            JsonCV.Location = new Point(788, 58);
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
            Generate_Rsme.Location = new Point(810, 350);
            Generate_Rsme.Name = "Generate_Rsme";
            Generate_Rsme.Size = new Size(180, 50);
            Generate_Rsme.TabIndex = 4;
            Generate_Rsme.Text = "Generate Resume";
            Generate_Rsme.UseVisualStyleBackColor = true;
            Generate_Rsme.Click += Generate_Rsme_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1140, 664);
            Controls.Add(Generate_Rsme);
            Controls.Add(JsonCV);
            Controls.Add(SendBtn);
            Controls.Add(TextOutput);
            Controls.Add(TextInput);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TextInput;
        private TextBox TextOutput;
        private Button SendBtn;
        private TextBox JsonCV;
        private Button Generate_Rsme;
    }
}
