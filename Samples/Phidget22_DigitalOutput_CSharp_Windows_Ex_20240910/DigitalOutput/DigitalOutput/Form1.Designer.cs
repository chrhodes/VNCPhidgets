namespace DigitalOutputExample
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
			this.forwardVoltageLbl = new System.Windows.Forms.Label();
			this.currentTxt = new System.Windows.Forms.TextBox();
			this.currentTrk = new System.Windows.Forms.TrackBar();
			this.currentLimitLbl = new System.Windows.Forms.Label();
			this.voltageCombo = new System.Windows.Forms.ComboBox();
			this.stateBtn = new System.Windows.Forms.Button();
			this.outputSettings = new System.Windows.Forms.GroupBox();
			this.dutyCycleMaxLbl = new System.Windows.Forms.Label();
			this.dutyCycleMinLbl = new System.Windows.Forms.Label();
			this.dutyCycleTxt = new System.Windows.Forms.TextBox();
			this.dutyCycleTrk = new System.Windows.Forms.TrackBar();
			this.dutyCycleLbl = new System.Windows.Forms.Label();
			this.currentMaxLbl = new System.Windows.Forms.Label();
			this.currentMinLbl = new System.Windows.Forms.Label();
			this.freqMaxLbl = new System.Windows.Forms.Label();
			this.freqMinLbl = new System.Windows.Forms.Label();
			this.freqTxt = new System.Windows.Forms.TextBox();
			this.freqTrk = new System.Windows.Forms.TrackBar();
			this.freqLbl = new System.Windows.Forms.Label();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.dutyCyclePanel = new System.Windows.Forms.Panel();
			this.frequencyPanel = new System.Windows.Forms.Panel();
			this.ledPanel = new System.Windows.Forms.Panel();
			this.phidgetInfoBox1 = new Phidget22.ExampleUtils.PhidgetInfoBox();
			((System.ComponentModel.ISupportInitialize)(this.currentTrk)).BeginInit();
			this.outputSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dutyCycleTrk)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqTrk)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.dutyCyclePanel.SuspendLayout();
			this.frequencyPanel.SuspendLayout();
			this.ledPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// forwardVoltageLbl
			// 
			this.forwardVoltageLbl.AutoSize = true;
			this.forwardVoltageLbl.Location = new System.Drawing.Point(3, 24);
			this.forwardVoltageLbl.Name = "forwardVoltageLbl";
			this.forwardVoltageLbl.Size = new System.Drawing.Size(87, 13);
			this.forwardVoltageLbl.TabIndex = 4;
			this.forwardVoltageLbl.Text = "Forward Voltage:";
			// 
			// currentTxt
			// 
			this.currentTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.currentTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.currentTxt.Location = new System.Drawing.Point(386, 21);
			this.currentTxt.Name = "currentTxt";
			this.currentTxt.ReadOnly = true;
			this.currentTxt.Size = new System.Drawing.Size(61, 13);
			this.currentTxt.TabIndex = 3;
			// 
			// currentTrk
			// 
			this.currentTrk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.currentTrk.LargeChange = 50;
			this.currentTrk.Location = new System.Drawing.Point(179, 21);
			this.currentTrk.Maximum = 1000;
			this.currentTrk.Name = "currentTrk";
			this.currentTrk.Size = new System.Drawing.Size(200, 45);
			this.currentTrk.TabIndex = 2;
			this.currentTrk.TickFrequency = 50;
			this.currentTrk.Scroll += new System.EventHandler(this.currentTrk_Scroll);
			// 
			// currentLimitLbl
			// 
			this.currentLimitLbl.AutoSize = true;
			this.currentLimitLbl.Location = new System.Drawing.Point(184, 5);
			this.currentLimitLbl.Name = "currentLimitLbl";
			this.currentLimitLbl.Size = new System.Drawing.Size(65, 13);
			this.currentLimitLbl.TabIndex = 1;
			this.currentLimitLbl.Text = "Current Limit";
			// 
			// voltageCombo
			// 
			this.voltageCombo.DisplayMember = "Value";
			this.voltageCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.voltageCombo.FormattingEnabled = true;
			this.voltageCombo.Location = new System.Drawing.Point(96, 21);
			this.voltageCombo.Name = "voltageCombo";
			this.voltageCombo.Size = new System.Drawing.Size(70, 21);
			this.voltageCombo.TabIndex = 0;
			this.voltageCombo.ValueMember = "Key";
			this.voltageCombo.SelectedIndexChanged += new System.EventHandler(this.voltageCombo_SelectedIndexChanged);
			// 
			// stateBtn
			// 
			this.stateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.stateBtn.Enabled = false;
			this.stateBtn.Location = new System.Drawing.Point(130, 3);
			this.stateBtn.Name = "stateBtn";
			this.stateBtn.Size = new System.Drawing.Size(196, 31);
			this.stateBtn.TabIndex = 85;
			this.stateBtn.Text = "Turn On";
			this.stateBtn.UseVisualStyleBackColor = true;
			this.stateBtn.Click += new System.EventHandler(this.stateBtn_Click);
			// 
			// outputSettings
			// 
			this.outputSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outputSettings.AutoSize = true;
			this.outputSettings.Controls.Add(this.flowLayoutPanel1);
			this.outputSettings.Location = new System.Drawing.Point(12, 117);
			this.outputSettings.Name = "outputSettings";
			this.outputSettings.Size = new System.Drawing.Size(463, 296);
			this.outputSettings.TabIndex = 87;
			this.outputSettings.TabStop = false;
			this.outputSettings.Text = "Settings";
			this.outputSettings.Visible = false;
			// 
			// dutyCycleMaxLbl
			// 
			this.dutyCycleMaxLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dutyCycleMaxLbl.AutoSize = true;
			this.dutyCycleMaxLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
			this.dutyCycleMaxLbl.Location = new System.Drawing.Point(368, 43);
			this.dutyCycleMaxLbl.Name = "dutyCycleMaxLbl";
			this.dutyCycleMaxLbl.Size = new System.Drawing.Size(9, 9);
			this.dutyCycleMaxLbl.TabIndex = 92;
			this.dutyCycleMaxLbl.Text = "1";
			// 
			// dutyCycleMinLbl
			// 
			this.dutyCycleMinLbl.AutoSize = true;
			this.dutyCycleMinLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
			this.dutyCycleMinLbl.Location = new System.Drawing.Point(7, 43);
			this.dutyCycleMinLbl.Name = "dutyCycleMinLbl";
			this.dutyCycleMinLbl.Size = new System.Drawing.Size(9, 9);
			this.dutyCycleMinLbl.TabIndex = 91;
			this.dutyCycleMinLbl.Text = "0";
			// 
			// dutyCycleTxt
			// 
			this.dutyCycleTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.dutyCycleTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dutyCycleTxt.Location = new System.Drawing.Point(386, 16);
			this.dutyCycleTxt.Name = "dutyCycleTxt";
			this.dutyCycleTxt.ReadOnly = true;
			this.dutyCycleTxt.Size = new System.Drawing.Size(61, 13);
			this.dutyCycleTxt.TabIndex = 90;
			// 
			// dutyCycleTrk
			// 
			this.dutyCycleTrk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dutyCycleTrk.LargeChange = 50;
			this.dutyCycleTrk.Location = new System.Drawing.Point(6, 16);
			this.dutyCycleTrk.Maximum = 1000;
			this.dutyCycleTrk.Name = "dutyCycleTrk";
			this.dutyCycleTrk.Size = new System.Drawing.Size(373, 45);
			this.dutyCycleTrk.TabIndex = 89;
			this.dutyCycleTrk.TickFrequency = 50;
			this.dutyCycleTrk.Scroll += new System.EventHandler(this.dutyCycleTrk_Scroll);
			// 
			// dutyCycleLbl
			// 
			this.dutyCycleLbl.AutoSize = true;
			this.dutyCycleLbl.Location = new System.Drawing.Point(3, 0);
			this.dutyCycleLbl.Name = "dutyCycleLbl";
			this.dutyCycleLbl.Size = new System.Drawing.Size(58, 13);
			this.dutyCycleLbl.TabIndex = 88;
			this.dutyCycleLbl.Text = "Duty Cycle";
			// 
			// currentMaxLbl
			// 
			this.currentMaxLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.currentMaxLbl.AutoSize = true;
			this.currentMaxLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
			this.currentMaxLbl.Location = new System.Drawing.Point(368, 48);
			this.currentMaxLbl.Name = "currentMaxLbl";
			this.currentMaxLbl.Size = new System.Drawing.Size(20, 9);
			this.currentMaxLbl.TabIndex = 87;
			this.currentMaxLbl.Text = "max";
			// 
			// currentMinLbl
			// 
			this.currentMinLbl.AutoSize = true;
			this.currentMinLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
			this.currentMinLbl.Location = new System.Drawing.Point(173, 48);
			this.currentMinLbl.Name = "currentMinLbl";
			this.currentMinLbl.Size = new System.Drawing.Size(18, 9);
			this.currentMinLbl.TabIndex = 86;
			this.currentMinLbl.Text = "min";
			// 
			// freqMaxLbl
			// 
			this.freqMaxLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.freqMaxLbl.AutoSize = true;
			this.freqMaxLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
			this.freqMaxLbl.Location = new System.Drawing.Point(368, 43);
			this.freqMaxLbl.Name = "freqMaxLbl";
			this.freqMaxLbl.Size = new System.Drawing.Size(20, 9);
			this.freqMaxLbl.TabIndex = 97;
			this.freqMaxLbl.Text = "max";
			// 
			// freqMinLbl
			// 
			this.freqMinLbl.AutoSize = true;
			this.freqMinLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
			this.freqMinLbl.Location = new System.Drawing.Point(7, 43);
			this.freqMinLbl.Name = "freqMinLbl";
			this.freqMinLbl.Size = new System.Drawing.Size(18, 9);
			this.freqMinLbl.TabIndex = 96;
			this.freqMinLbl.Text = "min";
			// 
			// freqTxt
			// 
			this.freqTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.freqTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.freqTxt.Location = new System.Drawing.Point(386, 16);
			this.freqTxt.Name = "freqTxt";
			this.freqTxt.ReadOnly = true;
			this.freqTxt.Size = new System.Drawing.Size(61, 13);
			this.freqTxt.TabIndex = 95;
			// 
			// freqTrk
			// 
			this.freqTrk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.freqTrk.LargeChange = 500;
			this.freqTrk.Location = new System.Drawing.Point(6, 16);
			this.freqTrk.Maximum = 20000;
			this.freqTrk.Name = "freqTrk";
			this.freqTrk.Size = new System.Drawing.Size(373, 45);
			this.freqTrk.TabIndex = 94;
			this.freqTrk.TickFrequency = 500;
			this.freqTrk.Scroll += new System.EventHandler(this.freqTrk_Scroll);
			// 
			// freqLbl
			// 
			this.freqLbl.AutoSize = true;
			this.freqLbl.Location = new System.Drawing.Point(3, 0);
			this.freqLbl.Name = "freqLbl";
			this.freqLbl.Size = new System.Drawing.Size(57, 13);
			this.freqLbl.TabIndex = 93;
			this.freqLbl.Text = "Frequency";
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.panel1);
			this.flowLayoutPanel1.Controls.Add(this.dutyCyclePanel);
			this.flowLayoutPanel1.Controls.Add(this.frequencyPanel);
			this.flowLayoutPanel1.Controls.Add(this.ledPanel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(457, 277);
			this.flowLayoutPanel1.TabIndex = 88;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.stateBtn);
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.MinimumSize = new System.Drawing.Size(451, 38);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(451, 38);
			this.panel1.TabIndex = 0;
			// 
			// dutyCyclePanel
			// 
			this.dutyCyclePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dutyCyclePanel.Controls.Add(this.dutyCycleMinLbl);
			this.dutyCyclePanel.Controls.Add(this.dutyCycleMaxLbl);
			this.dutyCyclePanel.Controls.Add(this.dutyCycleLbl);
			this.dutyCyclePanel.Controls.Add(this.dutyCycleTrk);
			this.dutyCyclePanel.Controls.Add(this.dutyCycleTxt);
			this.dutyCyclePanel.Location = new System.Drawing.Point(3, 47);
			this.dutyCyclePanel.MinimumSize = new System.Drawing.Size(451, 69);
			this.dutyCyclePanel.Name = "dutyCyclePanel";
			this.dutyCyclePanel.Size = new System.Drawing.Size(451, 69);
			this.dutyCyclePanel.TabIndex = 1;
			this.dutyCyclePanel.Visible = false;
			// 
			// frequencyPanel
			// 
			this.frequencyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.frequencyPanel.Controls.Add(this.freqMaxLbl);
			this.frequencyPanel.Controls.Add(this.freqLbl);
			this.frequencyPanel.Controls.Add(this.freqMinLbl);
			this.frequencyPanel.Controls.Add(this.freqTrk);
			this.frequencyPanel.Controls.Add(this.freqTxt);
			this.frequencyPanel.Location = new System.Drawing.Point(3, 122);
			this.frequencyPanel.MinimumSize = new System.Drawing.Size(451, 69);
			this.frequencyPanel.Name = "frequencyPanel";
			this.frequencyPanel.Size = new System.Drawing.Size(451, 69);
			this.frequencyPanel.TabIndex = 2;
			this.frequencyPanel.Visible = false;
			// 
			// ledPanel
			// 
			this.ledPanel.Controls.Add(this.currentMaxLbl);
			this.ledPanel.Controls.Add(this.forwardVoltageLbl);
			this.ledPanel.Controls.Add(this.currentMinLbl);
			this.ledPanel.Controls.Add(this.voltageCombo);
			this.ledPanel.Controls.Add(this.currentLimitLbl);
			this.ledPanel.Controls.Add(this.currentTxt);
			this.ledPanel.Controls.Add(this.currentTrk);
			this.ledPanel.Location = new System.Drawing.Point(3, 197);
			this.ledPanel.MinimumSize = new System.Drawing.Size(451, 75);
			this.ledPanel.Name = "ledPanel";
			this.ledPanel.Size = new System.Drawing.Size(451, 75);
			this.ledPanel.TabIndex = 3;
			this.ledPanel.Visible = false;
			// 
			// phidgetInfoBox1
			// 
			this.phidgetInfoBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.phidgetInfoBox1.Location = new System.Drawing.Point(12, 12);
			this.phidgetInfoBox1.MinimumSize = new System.Drawing.Size(396, 84);
			this.phidgetInfoBox1.Name = "phidgetInfoBox1";
			this.phidgetInfoBox1.Size = new System.Drawing.Size(458, 99);
			this.phidgetInfoBox1.TabIndex = 86;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(478, 421);
			this.Controls.Add(this.outputSettings);
			this.Controls.Add(this.phidgetInfoBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1000, 1000);
			this.Name = "Form1";
			this.Text = " Digital Output";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.currentTrk)).EndInit();
			this.outputSettings.ResumeLayout(false);
			this.outputSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dutyCycleTrk)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqTrk)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.dutyCyclePanel.ResumeLayout(false);
			this.dutyCyclePanel.PerformLayout();
			this.frequencyPanel.ResumeLayout(false);
			this.frequencyPanel.PerformLayout();
			this.ledPanel.ResumeLayout(false);
			this.ledPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label currentLimitLbl;
        private System.Windows.Forms.ComboBox voltageCombo;
        private System.Windows.Forms.TextBox currentTxt;
        private System.Windows.Forms.TrackBar currentTrk;
        private System.Windows.Forms.Button stateBtn;
        private System.Windows.Forms.Label forwardVoltageLbl;
        private Phidget22.ExampleUtils.PhidgetInfoBox phidgetInfoBox1;
        private System.Windows.Forms.GroupBox outputSettings;
        private System.Windows.Forms.Label currentMaxLbl;
        private System.Windows.Forms.Label currentMinLbl;
		private System.Windows.Forms.Label dutyCycleMaxLbl;
		private System.Windows.Forms.Label dutyCycleMinLbl;
		private System.Windows.Forms.TextBox dutyCycleTxt;
		private System.Windows.Forms.TrackBar dutyCycleTrk;
		private System.Windows.Forms.Label dutyCycleLbl;
		private System.Windows.Forms.Label freqMaxLbl;
		private System.Windows.Forms.Label freqMinLbl;
		private System.Windows.Forms.TextBox freqTxt;
		private System.Windows.Forms.TrackBar freqTrk;
		private System.Windows.Forms.Label freqLbl;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel dutyCyclePanel;
		private System.Windows.Forms.Panel frequencyPanel;
		private System.Windows.Forms.Panel ledPanel;
	}
}

