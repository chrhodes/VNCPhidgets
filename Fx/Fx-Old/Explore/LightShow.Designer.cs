namespace Explore
{
    partial class LightShow
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
            this.btnLoadShow = new System.Windows.Forms.Button();
            this.txtShow = new System.Windows.Forms.TextBox();
            this.btnLoadShows = new System.Windows.Forms.Button();
            this.lstShows = new System.Windows.Forms.ListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtShowDuration = new System.Windows.Forms.TextBox();
            this.txtSetDelay = new System.Windows.Forms.TextBox();
            this.txtShowDescription = new System.Windows.Forms.TextBox();
            this.txtShowName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbShowDetails = new System.Windows.Forms.GroupBox();
            this.cbShowLocations = new System.Windows.Forms.ComboBox();
            this.btnFindShow = new System.Windows.Forms.Button();
            this.btnPresentShow = new System.Windows.Forms.Button();
            this.btnLightsOff = new System.Windows.Forms.Button();
            this.btnLightsOn = new System.Windows.Forms.Button();
            this.txtOnDelay = new System.Windows.Forms.TextBox();
            this.txtOffDelay = new System.Windows.Forms.TextBox();
            this.txtLoops = new System.Windows.Forms.TextBox();
            this.btnShow2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbUseLightGrid = new System.Windows.Forms.RadioButton();
            this.rbUsePhidgets = new System.Windows.Forms.RadioButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnPresentFxShow = new System.Windows.Forms.Button();
            this.gbShowDetails.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadShow
            // 
            this.btnLoadShow.Location = new System.Drawing.Point(306, 126);
            this.btnLoadShow.Name = "btnLoadShow";
            this.btnLoadShow.Size = new System.Drawing.Size(89, 23);
            this.btnLoadShow.TabIndex = 0;
            this.btnLoadShow.Text = "Load Show";
            this.btnLoadShow.UseVisualStyleBackColor = true;
            this.btnLoadShow.Click += new System.EventHandler(this.btnLoadShow_Click);
            // 
            // txtShow
            // 
            this.txtShow.Location = new System.Drawing.Point(12, 128);
            this.txtShow.Name = "txtShow";
            this.txtShow.Size = new System.Drawing.Size(288, 20);
            this.txtShow.TabIndex = 1;
            this.txtShow.Text = "Show1.xml";
            // 
            // btnLoadShows
            // 
            this.btnLoadShows.Location = new System.Drawing.Point(306, 32);
            this.btnLoadShows.Name = "btnLoadShows";
            this.btnLoadShows.Size = new System.Drawing.Size(89, 23);
            this.btnLoadShows.TabIndex = 2;
            this.btnLoadShows.Text = "Load Shows";
            this.btnLoadShows.UseVisualStyleBackColor = true;
            this.btnLoadShows.Click += new System.EventHandler(this.btnLoadShows_Click);
            // 
            // lstShows
            // 
            this.lstShows.FormattingEnabled = true;
            this.lstShows.Location = new System.Drawing.Point(12, 11);
            this.lstShows.Name = "lstShows";
            this.lstShows.Size = new System.Drawing.Size(288, 69);
            this.lstShows.TabIndex = 3;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtShowDuration
            // 
            this.txtShowDuration.Location = new System.Drawing.Point(80, 118);
            this.txtShowDuration.Name = "txtShowDuration";
            this.txtShowDuration.Size = new System.Drawing.Size(100, 20);
            this.txtShowDuration.TabIndex = 4;
            // 
            // txtSetDelay
            // 
            this.txtSetDelay.Location = new System.Drawing.Point(80, 92);
            this.txtSetDelay.Name = "txtSetDelay";
            this.txtSetDelay.Size = new System.Drawing.Size(99, 20);
            this.txtSetDelay.TabIndex = 5;
            // 
            // txtShowDescription
            // 
            this.txtShowDescription.Location = new System.Drawing.Point(80, 45);
            this.txtShowDescription.Multiline = true;
            this.txtShowDescription.Name = "txtShowDescription";
            this.txtShowDescription.Size = new System.Drawing.Size(274, 41);
            this.txtShowDescription.TabIndex = 6;
            // 
            // txtShowName
            // 
            this.txtShowName.Location = new System.Drawing.Point(80, 19);
            this.txtShowName.Name = "txtShowName";
            this.txtShowName.Size = new System.Drawing.Size(274, 20);
            this.txtShowName.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "SetDelay";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Duration";
            // 
            // gbShowDetails
            // 
            this.gbShowDetails.Controls.Add(this.txtShowName);
            this.gbShowDetails.Controls.Add(this.label4);
            this.gbShowDetails.Controls.Add(this.txtShowDuration);
            this.gbShowDetails.Controls.Add(this.label3);
            this.gbShowDetails.Controls.Add(this.txtSetDelay);
            this.gbShowDetails.Controls.Add(this.label2);
            this.gbShowDetails.Controls.Add(this.txtShowDescription);
            this.gbShowDetails.Controls.Add(this.label1);
            this.gbShowDetails.Location = new System.Drawing.Point(12, 155);
            this.gbShowDetails.Name = "gbShowDetails";
            this.gbShowDetails.Size = new System.Drawing.Size(360, 148);
            this.gbShowDetails.TabIndex = 12;
            this.gbShowDetails.TabStop = false;
            this.gbShowDetails.Text = "Show Details";
            // 
            // cbShowLocations
            // 
            this.cbShowLocations.FormattingEnabled = true;
            this.cbShowLocations.Items.AddRange(new object[] {
            "D:\\VNC\\git\\chrhodes\\Phidgets\\Fx\\Fx-Old\\Shows",
            "C:\\TFS\\VNC\\Embedded Systems\\Fx\\Shows",
            "U:\\TFS\\VNC\\Embedded Systems\\Fx\\Shows",
            "V:\\USR\\VNC\\Fx\\Shows"});
            this.cbShowLocations.Location = new System.Drawing.Point(12, 101);
            this.cbShowLocations.Name = "cbShowLocations";
            this.cbShowLocations.Size = new System.Drawing.Size(288, 21);
            this.cbShowLocations.TabIndex = 13;
            // 
            // btnFindShow
            // 
            this.btnFindShow.Location = new System.Drawing.Point(306, 99);
            this.btnFindShow.Name = "btnFindShow";
            this.btnFindShow.Size = new System.Drawing.Size(89, 23);
            this.btnFindShow.TabIndex = 14;
            this.btnFindShow.Text = "Find Show";
            this.btnFindShow.UseVisualStyleBackColor = true;
            this.btnFindShow.Click += new System.EventHandler(this.btnFindShow_Click);
            // 
            // btnPresentShow
            // 
            this.btnPresentShow.Location = new System.Drawing.Point(258, 417);
            this.btnPresentShow.Name = "btnPresentShow";
            this.btnPresentShow.Size = new System.Drawing.Size(89, 23);
            this.btnPresentShow.TabIndex = 15;
            this.btnPresentShow.Text = "Present Show";
            this.btnPresentShow.UseVisualStyleBackColor = true;
            this.btnPresentShow.Click += new System.EventHandler(this.btnPresentShow_Click);
            // 
            // btnLightsOff
            // 
            this.btnLightsOff.Location = new System.Drawing.Point(117, 344);
            this.btnLightsOff.Name = "btnLightsOff";
            this.btnLightsOff.Size = new System.Drawing.Size(89, 23);
            this.btnLightsOff.TabIndex = 16;
            this.btnLightsOff.Text = "Lights Off";
            this.btnLightsOff.UseVisualStyleBackColor = true;
            this.btnLightsOff.Click += new System.EventHandler(this.btnLightsOff_Click);
            // 
            // btnLightsOn
            // 
            this.btnLightsOn.Location = new System.Drawing.Point(12, 344);
            this.btnLightsOn.Name = "btnLightsOn";
            this.btnLightsOn.Size = new System.Drawing.Size(89, 23);
            this.btnLightsOn.TabIndex = 17;
            this.btnLightsOn.Text = "Lights On";
            this.btnLightsOn.UseVisualStyleBackColor = true;
            this.btnLightsOn.Click += new System.EventHandler(this.btnLightsOn_Click);
            // 
            // txtOnDelay
            // 
            this.txtOnDelay.Location = new System.Drawing.Point(12, 318);
            this.txtOnDelay.Name = "txtOnDelay";
            this.txtOnDelay.Size = new System.Drawing.Size(89, 20);
            this.txtOnDelay.TabIndex = 12;
            this.txtOnDelay.Text = "100";
            // 
            // txtOffDelay
            // 
            this.txtOffDelay.Location = new System.Drawing.Point(117, 318);
            this.txtOffDelay.Name = "txtOffDelay";
            this.txtOffDelay.Size = new System.Drawing.Size(89, 20);
            this.txtOffDelay.TabIndex = 18;
            this.txtOffDelay.Text = "50";
            // 
            // txtLoops
            // 
            this.txtLoops.Location = new System.Drawing.Point(258, 391);
            this.txtLoops.Name = "txtLoops";
            this.txtLoops.Size = new System.Drawing.Size(89, 20);
            this.txtLoops.TabIndex = 19;
            this.txtLoops.Text = "25";
            // 
            // btnShow2
            // 
            this.btnShow2.Location = new System.Drawing.Point(163, 417);
            this.btnShow2.Name = "btnShow2";
            this.btnShow2.Size = new System.Drawing.Size(89, 23);
            this.btnShow2.TabIndex = 20;
            this.btnShow2.Text = "Present Show2";
            this.btnShow2.UseVisualStyleBackColor = true;
            this.btnShow2.Click += new System.EventHandler(this.Show2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbUseLightGrid);
            this.groupBox1.Controls.Add(this.rbUsePhidgets);
            this.groupBox1.Location = new System.Drawing.Point(386, 160);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(138, 68);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Display Device";
            // 
            // rbUseLightGrid
            // 
            this.rbUseLightGrid.AutoSize = true;
            this.rbUseLightGrid.Checked = true;
            this.rbUseLightGrid.Location = new System.Drawing.Point(7, 44);
            this.rbUseLightGrid.Name = "rbUseLightGrid";
            this.rbUseLightGrid.Size = new System.Drawing.Size(89, 17);
            this.rbUseLightGrid.TabIndex = 1;
            this.rbUseLightGrid.TabStop = true;
            this.rbUseLightGrid.Text = "Use LightGrid";
            this.rbUseLightGrid.UseVisualStyleBackColor = true;
            // 
            // rbUsePhidgets
            // 
            this.rbUsePhidgets.AutoSize = true;
            this.rbUsePhidgets.Location = new System.Drawing.Point(7, 20);
            this.rbUsePhidgets.Name = "rbUsePhidgets";
            this.rbUsePhidgets.Size = new System.Drawing.Size(88, 17);
            this.rbUsePhidgets.TabIndex = 0;
            this.rbUsePhidgets.Text = "Use Phidgets";
            this.rbUsePhidgets.UseVisualStyleBackColor = true;
            // 
            // btnPresentFxShow
            // 
            this.btnPresentFxShow.Location = new System.Drawing.Point(454, 112);
            this.btnPresentFxShow.Name = "btnPresentFxShow";
            this.btnPresentFxShow.Size = new System.Drawing.Size(126, 23);
            this.btnPresentFxShow.TabIndex = 23;
            this.btnPresentFxShow.Text = " Present FxShow";
            this.btnPresentFxShow.UseVisualStyleBackColor = true;
            this.btnPresentFxShow.Click += new System.EventHandler(this.btnPresentFxShow_Click);
            // 
            // LightShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 485);
            this.Controls.Add(this.btnPresentFxShow);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnShow2);
            this.Controls.Add(this.txtLoops);
            this.Controls.Add(this.txtOffDelay);
            this.Controls.Add(this.txtOnDelay);
            this.Controls.Add(this.btnLightsOn);
            this.Controls.Add(this.btnLightsOff);
            this.Controls.Add(this.btnPresentShow);
            this.Controls.Add(this.btnFindShow);
            this.Controls.Add(this.cbShowLocations);
            this.Controls.Add(this.gbShowDetails);
            this.Controls.Add(this.lstShows);
            this.Controls.Add(this.btnLoadShows);
            this.Controls.Add(this.txtShow);
            this.Controls.Add(this.btnLoadShow);
            this.Name = "LightShow";
            this.Text = "LightShow";
            this.gbShowDetails.ResumeLayout(false);
            this.gbShowDetails.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadShow;
        private System.Windows.Forms.TextBox txtShow;
        private System.Windows.Forms.Button btnLoadShows;
        private System.Windows.Forms.ListBox lstShows;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtShowDuration;
        private System.Windows.Forms.TextBox txtSetDelay;
        private System.Windows.Forms.TextBox txtShowDescription;
        private System.Windows.Forms.TextBox txtShowName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbShowDetails;
        private System.Windows.Forms.ComboBox cbShowLocations;
        private System.Windows.Forms.Button btnFindShow;
        private System.Windows.Forms.Button btnPresentShow;
        private System.Windows.Forms.Button btnLightsOff;
        private System.Windows.Forms.Button btnLightsOn;
        private System.Windows.Forms.TextBox txtOnDelay;
        private System.Windows.Forms.TextBox txtOffDelay;
        private System.Windows.Forms.TextBox txtLoops;
        private System.Windows.Forms.Button btnShow2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbUseLightGrid;
        private System.Windows.Forms.RadioButton rbUsePhidgets;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnPresentFxShow;
    }
}