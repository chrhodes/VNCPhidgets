namespace VoltageInput_Example
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
            this.dataBox = new System.Windows.Forms.GroupBox();
            this.voltageGraphButton = new System.Windows.Forms.Button();
            this.sensor_value_panel = new System.Windows.Forms.Panel();
            this.sensorValueGraphButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.sensorValueTxt = new System.Windows.Forms.TextBox();
            this.sensorTypeCmb = new System.Windows.Forms.ComboBox();
            this.sensorTypeLbl = new System.Windows.Forms.Label();
            this.voltageTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.interval_panel = new System.Windows.Forms.Panel();
            this.dataIntervalTrk = new PhidgetsControlLibrary.CustomScroll();
            this.voltage_range_panel = new System.Windows.Forms.Panel();
            this.voltageRangeCmb = new System.Windows.Forms.ComboBox();
            this.voltageRangeLbl = new System.Windows.Forms.Label();
            this.trigger_panel = new System.Windows.Forms.Panel();
            this.changeTriggerTrk = new PhidgetsControlLibrary.CustomScroll();
            this.PowerSupplyPanel = new System.Windows.Forms.GroupBox();
            this.powerSupplyCmb = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.guiUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.phidgetInfoBox = new Phidget22.ExampleUtils.PhidgetInfoBox();
            this.dataBox.SuspendLayout();
            this.sensor_value_panel.SuspendLayout();
            this.settingsBox.SuspendLayout();
            this.interval_panel.SuspendLayout();
            this.voltage_range_panel.SuspendLayout();
            this.trigger_panel.SuspendLayout();
            this.PowerSupplyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataBox
            // 
            this.dataBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataBox.AutoSize = true;
            this.dataBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dataBox.Controls.Add(this.voltageGraphButton);
            this.dataBox.Controls.Add(this.sensor_value_panel);
            this.dataBox.Controls.Add(this.sensorTypeCmb);
            this.dataBox.Controls.Add(this.sensorTypeLbl);
            this.dataBox.Controls.Add(this.voltageTxt);
            this.dataBox.Controls.Add(this.label5);
            this.dataBox.Location = new System.Drawing.Point(303, 104);
            this.dataBox.MinimumSize = new System.Drawing.Size(245, 0);
            this.dataBox.Name = "dataBox";
            this.dataBox.Size = new System.Drawing.Size(269, 119);
            this.dataBox.TabIndex = 11;
            this.dataBox.TabStop = false;
            this.dataBox.Text = "Data";
            this.dataBox.Visible = false;
            // 
            // voltageGraphButton
            // 
            this.voltageGraphButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.voltageGraphButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.voltageGraphButton.Image = ((System.Drawing.Image)(resources.GetObject("voltageGraphButton.Image")));
            this.voltageGraphButton.Location = new System.Drawing.Point(188, 16);
            this.voltageGraphButton.Name = "voltageGraphButton";
            this.voltageGraphButton.Size = new System.Drawing.Size(24, 23);
            this.voltageGraphButton.TabIndex = 103;
            this.voltageGraphButton.UseVisualStyleBackColor = false;
            this.voltageGraphButton.Click += new System.EventHandler(this.VoltageGraphButton_Click);
            // 
            // sensor_value_panel
            // 
            this.sensor_value_panel.Controls.Add(this.sensorValueGraphButton);
            this.sensor_value_panel.Controls.Add(this.label11);
            this.sensor_value_panel.Controls.Add(this.sensorValueTxt);
            this.sensor_value_panel.Enabled = false;
            this.sensor_value_panel.Location = new System.Drawing.Point(6, 76);
            this.sensor_value_panel.Name = "sensor_value_panel";
            this.sensor_value_panel.Size = new System.Drawing.Size(206, 24);
            this.sensor_value_panel.TabIndex = 106;
            this.sensor_value_panel.Visible = false;
            // 
            // sensorValueGraphButton
            // 
            this.sensorValueGraphButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorValueGraphButton.Image = ((System.Drawing.Image)(resources.GetObject("sensorValueGraphButton.Image")));
            this.sensorValueGraphButton.Location = new System.Drawing.Point(182, 1);
            this.sensorValueGraphButton.Name = "sensorValueGraphButton";
            this.sensorValueGraphButton.Size = new System.Drawing.Size(24, 23);
            this.sensorValueGraphButton.TabIndex = 107;
            this.sensorValueGraphButton.UseVisualStyleBackColor = true;
            this.sensorValueGraphButton.Click += new System.EventHandler(this.sensorValueGraphButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 6);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Sensor Value:";
            // 
            // sensorValueTxt
            // 
            this.sensorValueTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorValueTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sensorValueTxt.Location = new System.Drawing.Point(82, 6);
            this.sensorValueTxt.Name = "sensorValueTxt";
            this.sensorValueTxt.ReadOnly = true;
            this.sensorValueTxt.Size = new System.Drawing.Size(94, 13);
            this.sensorValueTxt.TabIndex = 17;
            this.sensorValueTxt.TabStop = false;
            // 
            // sensorTypeCmb
            // 
            this.sensorTypeCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorTypeCmb.DisplayMember = "Value";
            this.sensorTypeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sensorTypeCmb.DropDownWidth = 350;
            this.sensorTypeCmb.FormattingEnabled = true;
            this.sensorTypeCmb.Location = new System.Drawing.Point(85, 44);
            this.sensorTypeCmb.Name = "sensorTypeCmb";
            this.sensorTypeCmb.Size = new System.Drawing.Size(178, 21);
            this.sensorTypeCmb.TabIndex = 7;
            this.sensorTypeCmb.ValueMember = "Key";
            this.sensorTypeCmb.Visible = false;
            this.sensorTypeCmb.SelectedIndexChanged += new System.EventHandler(this.sensorTypeCmb_SelectedIndexChanged);
            // 
            // sensorTypeLbl
            // 
            this.sensorTypeLbl.AutoSize = true;
            this.sensorTypeLbl.Location = new System.Drawing.Point(9, 47);
            this.sensorTypeLbl.Name = "sensorTypeLbl";
            this.sensorTypeLbl.Size = new System.Drawing.Size(70, 13);
            this.sensorTypeLbl.TabIndex = 22;
            this.sensorTypeLbl.Text = "Sensor Type:";
            this.sensorTypeLbl.Visible = false;
            // 
            // voltageTxt
            // 
            this.voltageTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voltageTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.voltageTxt.Location = new System.Drawing.Point(85, 21);
            this.voltageTxt.Name = "voltageTxt";
            this.voltageTxt.ReadOnly = true;
            this.voltageTxt.Size = new System.Drawing.Size(97, 13);
            this.voltageTxt.TabIndex = 20;
            this.voltageTxt.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Voltage:";
            // 
            // settingsBox
            // 
            this.settingsBox.AutoSize = true;
            this.settingsBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.settingsBox.Controls.Add(this.interval_panel);
            this.settingsBox.Controls.Add(this.voltage_range_panel);
            this.settingsBox.Controls.Add(this.trigger_panel);
            this.settingsBox.Location = new System.Drawing.Point(12, 104);
            this.settingsBox.MinimumSize = new System.Drawing.Size(285, 0);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(285, 171);
            this.settingsBox.TabIndex = 12;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            this.settingsBox.Visible = false;
            // 
            // interval_panel
            // 
            this.interval_panel.Controls.Add(this.dataIntervalTrk);
            this.interval_panel.Location = new System.Drawing.Point(9, 59);
            this.interval_panel.Name = "interval_panel";
            this.interval_panel.Size = new System.Drawing.Size(270, 60);
            this.interval_panel.TabIndex = 20;
            // 
            // dataIntervalTrk
            // 
            this.dataIntervalTrk.AutoSize = true;
            this.dataIntervalTrk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dataIntervalTrk.BackColor = System.Drawing.SystemColors.Control;
            this.dataIntervalTrk.isFixedRange = true;
            this.dataIntervalTrk.isIntValue = true;
            this.dataIntervalTrk.labelTxt = "Data Interval:";
            this.dataIntervalTrk.Location = new System.Drawing.Point(2, 0);
            this.dataIntervalTrk.LogBase = 5E-08D;
            this.dataIntervalTrk.Maximum = 2D;
            this.dataIntervalTrk.maxTxt = "2";
            this.dataIntervalTrk.Minimum = -1D;
            this.dataIntervalTrk.minTxt = "-1";
            this.dataIntervalTrk.multiMode = true;
            this.dataIntervalTrk.Name = "dataIntervalTrk";
            this.dataIntervalTrk.Size = new System.Drawing.Size(268, 47);
            this.dataIntervalTrk.TabIndex = 109;
            this.dataIntervalTrk.TimingMode = PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL;
            this.dataIntervalTrk.trkText = "-1";
            this.dataIntervalTrk.TrueValue = -1D;
            this.dataIntervalTrk.Unit = "";
            this.dataIntervalTrk.Value = -1D;
            this.dataIntervalTrk.ModeSwitch += new System.EventHandler(this.dataIntervalTrk_ModeSwitch);
            this.dataIntervalTrk.KeyOverride += new System.Windows.Forms.KeyEventHandler(this.dataIntervalTrk_KeyOverride);
            this.dataIntervalTrk.ValueChange += new System.EventHandler(this.slider_ValueChangeEvent);
            // 
            // voltage_range_panel
            // 
            this.voltage_range_panel.Controls.Add(this.voltageRangeCmb);
            this.voltage_range_panel.Controls.Add(this.voltageRangeLbl);
            this.voltage_range_panel.Location = new System.Drawing.Point(9, 128);
            this.voltage_range_panel.Name = "voltage_range_panel";
            this.voltage_range_panel.Size = new System.Drawing.Size(256, 24);
            this.voltage_range_panel.TabIndex = 104;
            this.voltage_range_panel.Visible = false;
            // 
            // voltageRangeCmb
            // 
            this.voltageRangeCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voltageRangeCmb.DisplayMember = "Value";
            this.voltageRangeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.voltageRangeCmb.FormattingEnabled = true;
            this.voltageRangeCmb.Location = new System.Drawing.Point(90, 2);
            this.voltageRangeCmb.Name = "voltageRangeCmb";
            this.voltageRangeCmb.Size = new System.Drawing.Size(163, 21);
            this.voltageRangeCmb.TabIndex = 0;
            this.voltageRangeCmb.ValueMember = "Key";
            this.voltageRangeCmb.SelectedIndexChanged += new System.EventHandler(this.voltageRangeCmb_SelectedIndexChanged);
            // 
            // voltageRangeLbl
            // 
            this.voltageRangeLbl.AutoSize = true;
            this.voltageRangeLbl.Location = new System.Drawing.Point(3, 5);
            this.voltageRangeLbl.Name = "voltageRangeLbl";
            this.voltageRangeLbl.Size = new System.Drawing.Size(81, 13);
            this.voltageRangeLbl.TabIndex = 19;
            this.voltageRangeLbl.Text = "Voltage Range:";
            // 
            // trigger_panel
            // 
            this.trigger_panel.Controls.Add(this.changeTriggerTrk);
            this.trigger_panel.Location = new System.Drawing.Point(9, 19);
            this.trigger_panel.Name = "trigger_panel";
            this.trigger_panel.Size = new System.Drawing.Size(270, 34);
            this.trigger_panel.TabIndex = 19;
            // 
            // changeTriggerTrk
            // 
            this.changeTriggerTrk.AutoSize = true;
            this.changeTriggerTrk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.changeTriggerTrk.BackColor = System.Drawing.SystemColors.Control;
            this.changeTriggerTrk.isFixedRange = false;
            this.changeTriggerTrk.isIntValue = false;
            this.changeTriggerTrk.labelTxt = "Change Trigger:";
            this.changeTriggerTrk.Location = new System.Drawing.Point(2, 0);
            this.changeTriggerTrk.LogBase = 5E-08D;
            this.changeTriggerTrk.Maximum = 2D;
            this.changeTriggerTrk.maxTxt = "2";
            this.changeTriggerTrk.Minimum = -1D;
            this.changeTriggerTrk.minTxt = "-1";
            this.changeTriggerTrk.multiMode = false;
            this.changeTriggerTrk.Name = "changeTriggerTrk";
            this.changeTriggerTrk.Size = new System.Drawing.Size(268, 47);
            this.changeTriggerTrk.TabIndex = 108;
            this.changeTriggerTrk.TimingMode = PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL;
            this.changeTriggerTrk.trkText = "-1.000";
            this.changeTriggerTrk.TrueValue = -1D;
            this.changeTriggerTrk.Unit = "";
            this.changeTriggerTrk.Value = -1D;
            this.changeTriggerTrk.ValueChange += new System.EventHandler(this.slider_ValueChangeEvent);
            // 
            // PowerSupplyPanel
            // 
            this.PowerSupplyPanel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PowerSupplyPanel.AutoSize = true;
            this.PowerSupplyPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PowerSupplyPanel.Controls.Add(this.powerSupplyCmb);
            this.PowerSupplyPanel.Controls.Add(this.label3);
            this.PowerSupplyPanel.Controls.Add(this.textBox1);
            this.PowerSupplyPanel.Location = new System.Drawing.Point(12, 288);
            this.PowerSupplyPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.PowerSupplyPanel.Name = "PowerSupplyPanel";
            this.PowerSupplyPanel.Size = new System.Drawing.Size(285, 65);
            this.PowerSupplyPanel.TabIndex = 102;
            this.PowerSupplyPanel.TabStop = false;
            this.PowerSupplyPanel.Text = "Power Supply";
            this.PowerSupplyPanel.Visible = false;
            // 
            // powerSupplyCmb
            // 
            this.powerSupplyCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.powerSupplyCmb.DisplayMember = "Value";
            this.powerSupplyCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.powerSupplyCmb.DropDownWidth = 350;
            this.powerSupplyCmb.FormattingEnabled = true;
            this.powerSupplyCmb.Location = new System.Drawing.Point(108, 25);
            this.powerSupplyCmb.Name = "powerSupplyCmb";
            this.powerSupplyCmb.Size = new System.Drawing.Size(144, 21);
            this.powerSupplyCmb.TabIndex = 99;
            this.powerSupplyCmb.ValueMember = "Key";
            this.powerSupplyCmb.SelectedIndexChanged += new System.EventHandler(this.powerSupplyCmb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 100;
            this.label3.Text = "Power Supply:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(224, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(55, 13);
            this.textBox1.TabIndex = 43;
            this.textBox1.TabStop = false;
            // 
            // guiUpdateTimer
            // 
            this.guiUpdateTimer.Interval = 50;
            this.guiUpdateTimer.Tick += new System.EventHandler(this.guiUpdateTimer_Tick);
            // 
            // phidgetInfoBox
            // 
            this.phidgetInfoBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.phidgetInfoBox.Location = new System.Drawing.Point(12, 12);
            this.phidgetInfoBox.MinimumSize = new System.Drawing.Size(396, 84);
            this.phidgetInfoBox.Name = "phidgetInfoBox";
            this.phidgetInfoBox.Size = new System.Drawing.Size(550, 86);
            this.phidgetInfoBox.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(570, 372);
            this.Controls.Add(this.PowerSupplyPanel);
            this.Controls.Add(this.phidgetInfoBox);
            this.Controls.Add(this.settingsBox);
            this.Controls.Add(this.dataBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = " Voltage Input";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.dataBox.ResumeLayout(false);
            this.dataBox.PerformLayout();
            this.sensor_value_panel.ResumeLayout(false);
            this.sensor_value_panel.PerformLayout();
            this.settingsBox.ResumeLayout(false);
            this.interval_panel.ResumeLayout(false);
            this.interval_panel.PerformLayout();
            this.voltage_range_panel.ResumeLayout(false);
            this.voltage_range_panel.PerformLayout();
            this.trigger_panel.ResumeLayout(false);
            this.trigger_panel.PerformLayout();
            this.PowerSupplyPanel.ResumeLayout(false);
            this.PowerSupplyPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox dataBox;
        private System.Windows.Forms.GroupBox settingsBox;
        private System.Windows.Forms.ComboBox voltageRangeCmb;
        private System.Windows.Forms.TextBox voltageTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox sensorTypeCmb;
        private Phidget22.ExampleUtils.PhidgetInfoBox phidgetInfoBox;
        private System.Windows.Forms.Label sensorTypeLbl;
        private System.Windows.Forms.Label voltageRangeLbl;
        private System.Windows.Forms.Panel interval_panel;
        private System.Windows.Forms.Panel trigger_panel;
        private System.Windows.Forms.Panel voltage_range_panel;
        private System.Windows.Forms.GroupBox PowerSupplyPanel;
        private System.Windows.Forms.ComboBox powerSupplyCmb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private PhidgetsControlLibrary.CustomScroll changeTriggerTrk;
        private PhidgetsControlLibrary.CustomScroll dataIntervalTrk;
        private System.Windows.Forms.Panel sensor_value_panel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox sensorValueTxt;
		private System.Windows.Forms.Button voltageGraphButton;
		private System.Windows.Forms.Button sensorValueGraphButton;
		private System.Windows.Forms.Timer guiUpdateTimer;
	}
}

