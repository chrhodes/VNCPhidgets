namespace DebugLightGridApplication
{
    partial class frmLightGrid
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
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.ucLightGrid1 = new DebugLightGridApplication.ucLightGrid();
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(335, 25);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(597, 317);
            this.txtOutput.TabIndex = 1;
            // 
            // ucLightGrid1
            // 
            this.ucLightGrid1.Location = new System.Drawing.Point(12, 12);
            this.ucLightGrid1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ucLightGrid1.Name = "ucLightGrid1";
            this.ucLightGrid1.Size = new System.Drawing.Size(296, 342);
            this.ucLightGrid1.TabIndex = 2;
            // 
            // frmLightGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 363);
            this.Controls.Add(this.ucLightGrid1);
            this.Controls.Add(this.txtOutput);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "frmLightGrid";
            this.Text = "Debug LightGrid";
            this.Load += new System.EventHandler(this.frmLightGrid_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOutput;
        private ucLightGrid ucLightGrid1;

    }
}

