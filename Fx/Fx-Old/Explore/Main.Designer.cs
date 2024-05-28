namespace Explore
{
    partial class MainMenu
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
            this.btnLights = new System.Windows.Forms.Button();
            this.btnSounds = new System.Windows.Forms.Button();
            this.btnLightShow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLights
            // 
            this.btnLights.Location = new System.Drawing.Point(105, 48);
            this.btnLights.Name = "btnLights";
            this.btnLights.Size = new System.Drawing.Size(75, 23);
            this.btnLights.TabIndex = 0;
            this.btnLights.Text = "Lights";
            this.btnLights.UseVisualStyleBackColor = true;
            this.btnLights.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnSounds
            // 
            this.btnSounds.Location = new System.Drawing.Point(105, 95);
            this.btnSounds.Name = "btnSounds";
            this.btnSounds.Size = new System.Drawing.Size(75, 23);
            this.btnSounds.TabIndex = 1;
            this.btnSounds.Text = "Sounds";
            this.btnSounds.UseVisualStyleBackColor = true;
            this.btnSounds.Click += new System.EventHandler(this.btnSounds_Click);
            // 
            // btnLightShow
            // 
            this.btnLightShow.Location = new System.Drawing.Point(105, 176);
            this.btnLightShow.Name = "btnLightShow";
            this.btnLightShow.Size = new System.Drawing.Size(75, 23);
            this.btnLightShow.TabIndex = 2;
            this.btnLightShow.Text = "Light Show";
            this.btnLightShow.UseVisualStyleBackColor = true;
            this.btnLightShow.Click += new System.EventHandler(this.btnLightShow_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnLightShow);
            this.Controls.Add(this.btnSounds);
            this.Controls.Add(this.btnLights);
            this.Name = "MainMenu";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLights;
        private System.Windows.Forms.Button btnSounds;
        private System.Windows.Forms.Button btnLightShow;
    }
}