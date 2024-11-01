namespace HubExample
{
    partial class Form1
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
            if (disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.outputSettings = new System.Windows.Forms.GroupBox();
			this.phidgetInfoBox1 = new Phidget22.ExampleUtils.PhidgetInfoBox();
			this.SuspendLayout();
			// 
			// outputSettings
			// 
			this.outputSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outputSettings.AutoSize = true;
			this.outputSettings.Location = new System.Drawing.Point(12, 117);
			this.outputSettings.Name = "outputSettings";
			this.outputSettings.Size = new System.Drawing.Size(467, 298);
			this.outputSettings.TabIndex = 87;
			this.outputSettings.TabStop = false;
			this.outputSettings.Text = "Settings";
			this.outputSettings.Visible = false;
			// 
			// phidgetInfoBox1
			// 
			this.phidgetInfoBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.phidgetInfoBox1.Location = new System.Drawing.Point(12, 12);
			this.phidgetInfoBox1.MinimumSize = new System.Drawing.Size(396, 84);
			this.phidgetInfoBox1.Name = "phidgetInfoBox1";
			this.phidgetInfoBox1.Size = new System.Drawing.Size(467, 99);
			this.phidgetInfoBox1.TabIndex = 86;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(487, 423);
			this.Controls.Add(this.outputSettings);
			this.Controls.Add(this.phidgetInfoBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1000, 1000);
			this.Name = "Form1";
			this.Text = "Hub";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private Phidget22.ExampleUtils.PhidgetInfoBox phidgetInfoBox1;
        private System.Windows.Forms.GroupBox outputSettings;
	}
}

