namespace DigitalInput_Example
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.inputState = new System.Windows.Forms.TextBox();
            this.stateLbl = new System.Windows.Forms.Label();
            this.dataBox = new System.Windows.Forms.GroupBox();
            this.stateGraphButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.PowerSupplyPanel = new System.Windows.Forms.GroupBox();
            this.powerSupplyCmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.inputModeBox = new System.Windows.Forms.GroupBox();
            this.inputModeCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.guiUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.phidgetInfoBox1 = new Phidget22.ExampleUtils.PhidgetInfoBox();
            this.dataBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.PowerSupplyPanel.SuspendLayout();
            this.inputModeBox.SuspendLayout();
            this.settingsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputState
            // 
            this.inputState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputState.Location = new System.Drawing.Point(66, 20);
            this.inputState.Multiline = true;
            this.inputState.Name = "inputState";
            this.inputState.ReadOnly = true;
            this.inputState.Size = new System.Drawing.Size(89, 46);
            this.inputState.TabIndex = 4;
            this.inputState.TabStop = false;
            this.inputState.Text = "\r\nUnknown";
            this.inputState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // stateLbl
            // 
            this.stateLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stateLbl.AutoSize = true;
            this.stateLbl.Location = new System.Drawing.Point(16, 36);
            this.stateLbl.Name = "stateLbl";
            this.stateLbl.Size = new System.Drawing.Size(35, 13);
            this.stateLbl.TabIndex = 5;
            this.stateLbl.Text = "State:";
            // 
            // dataBox
            // 
            this.dataBox.Controls.Add(this.stateGraphButton);
            this.dataBox.Controls.Add(this.stateLbl);
            this.dataBox.Controls.Add(this.inputState);
            this.dataBox.Location = new System.Drawing.Point(3, 3);
            this.dataBox.Name = "dataBox";
            this.dataBox.Size = new System.Drawing.Size(161, 81);
            this.dataBox.TabIndex = 8;
            this.dataBox.TabStop = false;
            this.dataBox.Text = "Data";
            this.dataBox.Visible = false;
            // 
            // stateGraphButton
            // 
            this.stateGraphButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stateGraphButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.stateGraphButton.Image = ((System.Drawing.Image)(resources.GetObject("stateGraphButton.Image")));
            this.stateGraphButton.Location = new System.Drawing.Point(27, 52);
            this.stateGraphButton.Name = "stateGraphButton";
            this.stateGraphButton.Size = new System.Drawing.Size(24, 23);
            this.stateGraphButton.TabIndex = 104;
            this.stateGraphButton.UseVisualStyleBackColor = false;
            this.stateGraphButton.Click += new System.EventHandler(this.stateGraphButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.dataBox);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 117);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(491, 88);
            this.flowLayoutPanel1.TabIndex = 9;
            this.flowLayoutPanel1.Visible = false;
            // 
            // PowerSupplyPanel
            // 
            this.PowerSupplyPanel.AutoSize = true;
            this.PowerSupplyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PowerSupplyPanel.Controls.Add(this.powerSupplyCmb);
            this.PowerSupplyPanel.Controls.Add(this.label3);
            this.PowerSupplyPanel.Controls.Add(this.textBox1);
            this.PowerSupplyPanel.Location = new System.Drawing.Point(6, 19);
            this.PowerSupplyPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.PowerSupplyPanel.Name = "PowerSupplyPanel";
            this.PowerSupplyPanel.Size = new System.Drawing.Size(239, 65);
            this.PowerSupplyPanel.TabIndex = 103;
            this.PowerSupplyPanel.TabStop = false;
            this.PowerSupplyPanel.Text = "Power Supply";
            // 
            // powerSupplyCmb
            // 
            this.powerSupplyCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.powerSupplyCmb.DisplayMember = "Value";
            this.powerSupplyCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.powerSupplyCmb.DropDownWidth = 350;
            this.powerSupplyCmb.FormattingEnabled = true;
            this.powerSupplyCmb.Location = new System.Drawing.Point(79, 25);
            this.powerSupplyCmb.Name = "powerSupplyCmb";
            this.powerSupplyCmb.Size = new System.Drawing.Size(154, 21);
            this.powerSupplyCmb.TabIndex = 99;
            this.powerSupplyCmb.ValueMember = "Key";
            this.powerSupplyCmb.SelectedIndexChanged += new System.EventHandler(this.powerSupplyCmb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 100;
            this.label3.Text = "Power Supply:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(178, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(55, 13);
            this.textBox1.TabIndex = 43;
            this.textBox1.TabStop = false;
            // 
            // inputModeBox
            // 
            this.inputModeBox.AutoSize = true;
            this.inputModeBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.inputModeBox.Controls.Add(this.inputModeCombo);
            this.inputModeBox.Controls.Add(this.label1);
            this.inputModeBox.Controls.Add(this.textBox2);
            this.inputModeBox.Location = new System.Drawing.Point(251, 19);
            this.inputModeBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.inputModeBox.Name = "inputModeBox";
            this.inputModeBox.Size = new System.Drawing.Size(231, 65);
            this.inputModeBox.TabIndex = 104;
            this.inputModeBox.TabStop = false;
            this.inputModeBox.Text = "Input Mode";
            // 
            // inputModeCombo
            // 
            this.inputModeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputModeCombo.DisplayMember = "Value";
            this.inputModeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputModeCombo.DropDownWidth = 350;
            this.inputModeCombo.FormattingEnabled = true;
            this.inputModeCombo.Location = new System.Drawing.Point(79, 25);
            this.inputModeCombo.Name = "inputModeCombo";
            this.inputModeCombo.Size = new System.Drawing.Size(146, 21);
            this.inputModeCombo.TabIndex = 99;
            this.inputModeCombo.ValueMember = "Key";
            this.inputModeCombo.SelectedIndexChanged += new System.EventHandler(this.inputModeCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 100;
            this.label1.Text = "Input Mode:";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Location = new System.Drawing.Point(170, 19);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(55, 13);
            this.textBox2.TabIndex = 43;
            this.textBox2.TabStop = false;
            // 
            // guiUpdateTimer
            // 
            this.guiUpdateTimer.Interval = 50;
            this.guiUpdateTimer.Tick += new System.EventHandler(this.guiUpdateTimer_Tick);
            // 
            // settingsBox
            // 
            this.settingsBox.Controls.Add(this.PowerSupplyPanel);
            this.settingsBox.Controls.Add(this.inputModeBox);
            this.settingsBox.Location = new System.Drawing.Point(12, 211);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(488, 98);
            this.settingsBox.TabIndex = 105;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            // 
            // phidgetInfoBox1
            // 
            this.phidgetInfoBox1.Location = new System.Drawing.Point(12, 12);
            this.phidgetInfoBox1.MinimumSize = new System.Drawing.Size(396, 84);
            this.phidgetInfoBox1.Name = "phidgetInfoBox1";
            this.phidgetInfoBox1.Size = new System.Drawing.Size(492, 99);
            this.phidgetInfoBox1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(515, 315);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.phidgetInfoBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = " Digital Input";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.dataBox.ResumeLayout(false);
            this.dataBox.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.PowerSupplyPanel.ResumeLayout(false);
            this.PowerSupplyPanel.PerformLayout();
            this.inputModeBox.ResumeLayout(false);
            this.inputModeBox.PerformLayout();
            this.settingsBox.ResumeLayout(false);
            this.settingsBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox inputState;
        private System.Windows.Forms.Label stateLbl;
        private Phidget22.ExampleUtils.PhidgetInfoBox phidgetInfoBox1;
        private System.Windows.Forms.GroupBox dataBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox PowerSupplyPanel;
        private System.Windows.Forms.ComboBox powerSupplyCmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox inputModeBox;
        private System.Windows.Forms.ComboBox inputModeCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button stateGraphButton;
		private System.Windows.Forms.Timer guiUpdateTimer;
        private System.Windows.Forms.GroupBox settingsBox;
    }
}

