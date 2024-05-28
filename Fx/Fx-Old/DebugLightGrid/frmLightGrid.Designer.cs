namespace DebugLightGrid
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
            this.ucLightGrid1 = new DebugLightGrid.ucLightGrid();
            this.SuspendLayout();
            // 
            // ucLightGrid1
            // 
            this.ucLightGrid1.Location = new System.Drawing.Point(21, 12);
            this.ucLightGrid1.Name = "ucLightGrid1";
            this.ucLightGrid1.Size = new System.Drawing.Size(333, 368);
            this.ucLightGrid1.TabIndex = 0;
            // 
            // frmLightGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 386);
            this.Controls.Add(this.ucLightGrid1);
            this.Name = "frmLightGrid";
            this.Text = "frmLightGrid";
            this.ResumeLayout(false);

        }

        #endregion

        private ucLightGrid ucLightGrid1;
    }
}