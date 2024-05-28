namespace DebugWindow
{
    partial class frmDebugWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClearOutput = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.chkDebugSQL = new System.Windows.Forms.CheckBox();
            this.gbDebugOptions = new System.Windows.Forms.GroupBox();
            this.chkDebugSubmission = new System.Windows.Forms.CheckBox();
            this.chkDebugEAI = new System.Windows.Forms.CheckBox();
            this.chkDebugValidation = new System.Windows.Forms.CheckBox();
            this.gbDebugOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClearOutput
            // 
            this.btnClearOutput.Location = new System.Drawing.Point(12, 12);
            this.btnClearOutput.Name = "btnClearOutput";
            this.btnClearOutput.Size = new System.Drawing.Size(178, 23);
            this.btnClearOutput.TabIndex = 7;
            this.btnClearOutput.Text = "Clear Output";
            this.btnClearOutput.UseVisualStyleBackColor = true;
            this.btnClearOutput.Click += new System.EventHandler(this.btnClearOutput_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(196, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(576, 538);
            this.txtOutput.TabIndex = 6;
            // 
            // chkDebugSQL
            // 
            this.chkDebugSQL.AutoSize = true;
            this.chkDebugSQL.Location = new System.Drawing.Point(16, 25);
            this.chkDebugSQL.Name = "chkDebugSQL";
            this.chkDebugSQL.Size = new System.Drawing.Size(82, 17);
            this.chkDebugSQL.TabIndex = 3;
            this.chkDebugSQL.Text = "Debug SQL";
            this.chkDebugSQL.UseVisualStyleBackColor = true;
            this.chkDebugSQL.CheckedChanged += new System.EventHandler(this.chkDebugSQL_CheckedChanged);
            // 
            // gbDebugOptions
            // 
            this.gbDebugOptions.Controls.Add(this.chkDebugSubmission);
            this.gbDebugOptions.Controls.Add(this.chkDebugEAI);
            this.gbDebugOptions.Controls.Add(this.chkDebugValidation);
            this.gbDebugOptions.Controls.Add(this.chkDebugSQL);
            this.gbDebugOptions.Location = new System.Drawing.Point(12, 53);
            this.gbDebugOptions.Name = "gbDebugOptions";
            this.gbDebugOptions.Size = new System.Drawing.Size(178, 181);
            this.gbDebugOptions.TabIndex = 8;
            this.gbDebugOptions.TabStop = false;
            this.gbDebugOptions.Text = "Debug Options";
            // 
            // chkDebugSubmission
            // 
            this.chkDebugSubmission.AutoSize = true;
            this.chkDebugSubmission.Location = new System.Drawing.Point(16, 117);
            this.chkDebugSubmission.Name = "chkDebugSubmission";
            this.chkDebugSubmission.Size = new System.Drawing.Size(114, 17);
            this.chkDebugSubmission.TabIndex = 5;
            this.chkDebugSubmission.Text = "Debug Submission";
            this.chkDebugSubmission.UseVisualStyleBackColor = true;
            this.chkDebugSubmission.CheckedChanged += new System.EventHandler(this.chkDebugSubmission_CheckedChanged);
            // 
            // chkDebugEAI
            // 
            this.chkDebugEAI.AutoSize = true;
            this.chkDebugEAI.Location = new System.Drawing.Point(16, 140);
            this.chkDebugEAI.Name = "chkDebugEAI";
            this.chkDebugEAI.Size = new System.Drawing.Size(78, 17);
            this.chkDebugEAI.TabIndex = 4;
            this.chkDebugEAI.Text = "Debug EAI";
            this.chkDebugEAI.UseVisualStyleBackColor = true;
            this.chkDebugEAI.CheckedChanged += new System.EventHandler(this.chkDebugEAI_CheckedChanged);
            // 
            // chkDebugValidation
            // 
            this.chkDebugValidation.AutoSize = true;
            this.chkDebugValidation.Location = new System.Drawing.Point(16, 94);
            this.chkDebugValidation.Name = "chkDebugValidation";
            this.chkDebugValidation.Size = new System.Drawing.Size(107, 17);
            this.chkDebugValidation.TabIndex = 6;
            this.chkDebugValidation.Text = "Debug Validation";
            this.chkDebugValidation.UseVisualStyleBackColor = true;
            this.chkDebugValidation.CheckedChanged += new System.EventHandler(this.chkDebugValidation_CheckedChanged);
            // 
            // frmDebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btnClearOutput);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.gbDebugOptions);
            this.Name = "frmDebugWindow";
            this.Text = "frmDebugWindow X";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDebugWindow_FormClosing);
            this.gbDebugOptions.ResumeLayout(false);
            this.gbDebugOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnClearOutput;
        internal System.Windows.Forms.TextBox txtOutput;
        internal System.Windows.Forms.CheckBox chkDebugSQL;
        internal System.Windows.Forms.GroupBox gbDebugOptions;
        internal System.Windows.Forms.CheckBox chkDebugSubmission;
        internal System.Windows.Forms.CheckBox chkDebugEAI;
        internal System.Windows.Forms.CheckBox chkDebugValidation;
    }
}