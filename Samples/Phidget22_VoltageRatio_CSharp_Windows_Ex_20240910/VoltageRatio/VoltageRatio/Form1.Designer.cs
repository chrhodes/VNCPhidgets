namespace VoltageRatio_Example
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
            this.settingsBox = new System.Windows.Forms.GroupBox();
            this.bridgeSettings = new System.Windows.Forms.GroupBox();
            this.bridge_gain_panel = new System.Windows.Forms.Panel();
            this.gainLbl = new System.Windows.Forms.Label();
            this.gainCombo = new System.Windows.Forms.ComboBox();
            this.enabledChk = new System.Windows.Forms.CheckBox();
            this.interval_panel = new System.Windows.Forms.Panel();
            this.dataIntervalTrk = new PhidgetsControlLibrary.CustomScroll();
            this.trigger_panel = new System.Windows.Forms.Panel();
            this.changeTriggerTrk = new PhidgetsControlLibrary.CustomScroll();
            this.tareBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.calibrateBtn = new System.Windows.Forms.Button();
            this.offsetTxt = new System.Windows.Forms.TextBox();
            this.gainTxt = new System.Windows.Forms.TextBox();
            this.sensorTypeLbl = new System.Windows.Forms.Label();
            this.sensorTypeCmb = new System.Windows.Forms.ComboBox();
            this.outputBox = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.voltageGraphButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.voltageTxt = new System.Windows.Forms.TextBox();
            this.sensor_type_panel = new System.Windows.Forms.Panel();
            this.sensor_value_panel = new System.Windows.Forms.Panel();
            this.sensorGraphButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.sensorTxt = new System.Windows.Forms.TextBox();
            this.calibrationPanel = new System.Windows.Forms.Panel();
            this.weightGraphBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.weightTxt = new System.Windows.Forms.TextBox();
            this.guiUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.calibrationBox = new System.Windows.Forms.GroupBox();
            this.cpyBtn = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.gainSet = new System.Windows.Forms.Button();
            this.urlTip = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.phidgetInfoBox1 = new Phidget22.ExampleUtils.PhidgetInfoBox();
            this.settingsBox.SuspendLayout();
            this.bridgeSettings.SuspendLayout();
            this.bridge_gain_panel.SuspendLayout();
            this.interval_panel.SuspendLayout();
            this.trigger_panel.SuspendLayout();
            this.outputBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.sensor_type_panel.SuspendLayout();
            this.sensor_value_panel.SuspendLayout();
            this.calibrationPanel.SuspendLayout();
            this.calibrationBox.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsBox
            // 
            this.settingsBox.AutoSize = true;
            this.settingsBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.settingsBox.Controls.Add(this.bridgeSettings);
            this.settingsBox.Controls.Add(this.interval_panel);
            this.settingsBox.Controls.Add(this.trigger_panel);
            this.settingsBox.Location = new System.Drawing.Point(9, 102);
            this.settingsBox.Name = "settingsBox";
            this.settingsBox.Size = new System.Drawing.Size(282, 209);
            this.settingsBox.TabIndex = 0;
            this.settingsBox.TabStop = false;
            this.settingsBox.Text = "Settings";
            this.settingsBox.Visible = false;
            // 
            // bridgeSettings
            // 
            this.bridgeSettings.Controls.Add(this.bridge_gain_panel);
            this.bridgeSettings.Controls.Add(this.enabledChk);
            this.bridgeSettings.Location = new System.Drawing.Point(9, 134);
            this.bridgeSettings.Name = "bridgeSettings";
            this.bridgeSettings.Size = new System.Drawing.Size(267, 56);
            this.bridgeSettings.TabIndex = 17;
            this.bridgeSettings.TabStop = false;
            this.bridgeSettings.Text = "Bridge Settings";
            this.bridgeSettings.Visible = false;
            // 
            // bridge_gain_panel
            // 
            this.bridge_gain_panel.Controls.Add(this.gainLbl);
            this.bridge_gain_panel.Controls.Add(this.gainCombo);
            this.bridge_gain_panel.Location = new System.Drawing.Point(6, 19);
            this.bridge_gain_panel.Name = "bridge_gain_panel";
            this.bridge_gain_panel.Size = new System.Drawing.Size(168, 24);
            this.bridge_gain_panel.TabIndex = 104;
            // 
            // gainLbl
            // 
            this.gainLbl.AutoSize = true;
            this.gainLbl.Location = new System.Drawing.Point(3, 4);
            this.gainLbl.Name = "gainLbl";
            this.gainLbl.Size = new System.Drawing.Size(65, 13);
            this.gainLbl.TabIndex = 2;
            this.gainLbl.Text = "Bridge Gain:";
            // 
            // gainCombo
            // 
            this.gainCombo.DisplayMember = "Value";
            this.gainCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gainCombo.FormattingEnabled = true;
            this.gainCombo.Location = new System.Drawing.Point(74, 3);
            this.gainCombo.Name = "gainCombo";
            this.gainCombo.Size = new System.Drawing.Size(88, 21);
            this.gainCombo.TabIndex = 0;
            this.gainCombo.TabStop = false;
            this.gainCombo.ValueMember = "Key";
            this.gainCombo.SelectedIndexChanged += new System.EventHandler(this.gainCombo_SelectedIndexChanged);
            // 
            // enabledChk
            // 
            this.enabledChk.AutoSize = true;
            this.enabledChk.Location = new System.Drawing.Point(194, 22);
            this.enabledChk.Name = "enabledChk";
            this.enabledChk.Size = new System.Drawing.Size(59, 17);
            this.enabledChk.TabIndex = 1;
            this.enabledChk.TabStop = false;
            this.enabledChk.Text = "Enable";
            this.enabledChk.UseVisualStyleBackColor = true;
            this.enabledChk.Visible = false;
            this.enabledChk.CheckedChanged += new System.EventHandler(this.enabledChk_CheckedChanged);
            // 
            // interval_panel
            // 
            this.interval_panel.Controls.Add(this.dataIntervalTrk);
            this.interval_panel.Location = new System.Drawing.Point(6, 71);
            this.interval_panel.Name = "interval_panel";
            this.interval_panel.Size = new System.Drawing.Size(270, 57);
            this.interval_panel.TabIndex = 108;
            // 
            // dataIntervalTrk
            // 
            this.dataIntervalTrk.AutoSize = true;
            this.dataIntervalTrk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dataIntervalTrk.BackColor = System.Drawing.SystemColors.Control;
            this.dataIntervalTrk.isFixedRange = true;
            this.dataIntervalTrk.isIntValue = true;
            this.dataIntervalTrk.labelTxt = "Data Interval:";
            this.dataIntervalTrk.Location = new System.Drawing.Point(3, 3);
            this.dataIntervalTrk.LogBase = 0D;
            this.dataIntervalTrk.Maximum = 2D;
            this.dataIntervalTrk.maxTxt = "2";
            this.dataIntervalTrk.Minimum = -1D;
            this.dataIntervalTrk.minTxt = "-1";
            this.dataIntervalTrk.multiMode = true;
            this.dataIntervalTrk.Name = "dataIntervalTrk";
            this.dataIntervalTrk.Size = new System.Drawing.Size(267, 47);
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
            // trigger_panel
            // 
            this.trigger_panel.Controls.Add(this.changeTriggerTrk);
            this.trigger_panel.Location = new System.Drawing.Point(6, 19);
            this.trigger_panel.Name = "trigger_panel";
            this.trigger_panel.Size = new System.Drawing.Size(270, 48);
            this.trigger_panel.TabIndex = 107;
            // 
            // changeTriggerTrk
            // 
            this.changeTriggerTrk.AutoSize = true;
            this.changeTriggerTrk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.changeTriggerTrk.BackColor = System.Drawing.SystemColors.Control;
            this.changeTriggerTrk.isFixedRange = false;
            this.changeTriggerTrk.isIntValue = false;
            this.changeTriggerTrk.labelTxt = "Change Trigger:";
            this.changeTriggerTrk.Location = new System.Drawing.Point(3, -1);
            this.changeTriggerTrk.LogBase = 5E-08D;
            this.changeTriggerTrk.Maximum = 2D;
            this.changeTriggerTrk.maxTxt = "2";
            this.changeTriggerTrk.Minimum = -1D;
            this.changeTriggerTrk.minTxt = "-1";
            this.changeTriggerTrk.multiMode = false;
            this.changeTriggerTrk.Name = "changeTriggerTrk";
            this.changeTriggerTrk.Size = new System.Drawing.Size(267, 47);
            this.changeTriggerTrk.TabIndex = 108;
            this.changeTriggerTrk.TimingMode = PhidgetsControlLibrary.CustomScroll.Modes.INTERVAL;
            this.changeTriggerTrk.trkText = "-1.000";
            this.changeTriggerTrk.TrueValue = -1D;
            this.changeTriggerTrk.Unit = "";
            this.changeTriggerTrk.Value = -1D;
            this.changeTriggerTrk.ValueChange += new System.EventHandler(this.slider_ValueChangeEvent);
            // 
            // tareBtn
            // 
            this.tareBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.tareBtn.Location = new System.Drawing.Point(204, 83);
            this.tareBtn.Name = "tareBtn";
            this.tareBtn.Size = new System.Drawing.Size(32, 16);
            this.tareBtn.TabIndex = 107;
            this.tareBtn.Text = "Tare";
            this.tareBtn.UseVisualStyleBackColor = true;
            this.tareBtn.Click += new System.EventHandler(this.tareBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(124, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 106;
            this.label3.Text = "Offset:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 105;
            this.label2.Text = "Gain:";
            // 
            // calibrateBtn
            // 
            this.calibrateBtn.Location = new System.Drawing.Point(6, 19);
            this.calibrateBtn.Name = "calibrateBtn";
            this.calibrateBtn.Size = new System.Drawing.Size(94, 23);
            this.calibrateBtn.TabIndex = 17;
            this.calibrateBtn.Text = "Begin Calibration";
            this.calibrateBtn.UseVisualStyleBackColor = true;
            this.calibrateBtn.Click += new System.EventHandler(this.calibrateBtn_Click);
            // 
            // offsetTxt
            // 
            this.offsetTxt.Location = new System.Drawing.Point(127, 68);
            this.offsetTxt.Name = "offsetTxt";
            this.offsetTxt.Size = new System.Drawing.Size(110, 20);
            this.offsetTxt.TabIndex = 18;
            // 
            // gainTxt
            // 
            this.gainTxt.Location = new System.Drawing.Point(7, 68);
            this.gainTxt.Name = "gainTxt";
            this.gainTxt.Size = new System.Drawing.Size(112, 20);
            this.gainTxt.TabIndex = 19;
            this.gainTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gainTxt_KeyDown);
            // 
            // sensorTypeLbl
            // 
            this.sensorTypeLbl.AutoSize = true;
            this.sensorTypeLbl.Location = new System.Drawing.Point(3, 5);
            this.sensorTypeLbl.Name = "sensorTypeLbl";
            this.sensorTypeLbl.Size = new System.Drawing.Size(70, 13);
            this.sensorTypeLbl.TabIndex = 9;
            this.sensorTypeLbl.Text = "Sensor Type:";
            // 
            // sensorTypeCmb
            // 
            this.sensorTypeCmb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorTypeCmb.DisplayMember = "Value";
            this.sensorTypeCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sensorTypeCmb.DropDownWidth = 450;
            this.sensorTypeCmb.FormattingEnabled = true;
            this.sensorTypeCmb.Location = new System.Drawing.Point(79, 2);
            this.sensorTypeCmb.Name = "sensorTypeCmb";
            this.sensorTypeCmb.Size = new System.Drawing.Size(164, 21);
            this.sensorTypeCmb.TabIndex = 0;
            this.sensorTypeCmb.ValueMember = "Key";
            this.sensorTypeCmb.SelectedIndexChanged += new System.EventHandler(this.sensorTypeCmb_SelectedIndexChanged);
            // 
            // outputBox
            // 
            this.outputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputBox.AutoSize = true;
            this.outputBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.outputBox.Controls.Add(this.flowLayoutPanel1);
            this.outputBox.Location = new System.Drawing.Point(3, 3);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(262, 150);
            this.outputBox.TabIndex = 1;
            this.outputBox.TabStop = false;
            this.outputBox.Text = "Data";
            this.outputBox.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.sensor_type_panel);
            this.flowLayoutPanel1.Controls.Add(this.sensor_value_panel);
            this.flowLayoutPanel1.Controls.Add(this.calibrationPanel);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 11);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(252, 120);
            this.flowLayoutPanel1.TabIndex = 107;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.voltageGraphButton);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.voltageTxt);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 24);
            this.panel1.TabIndex = 106;
            // 
            // voltageGraphButton
            // 
            this.voltageGraphButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.voltageGraphButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.voltageGraphButton.Image = ((System.Drawing.Image)(resources.GetObject("voltageGraphButton.Image")));
            this.voltageGraphButton.Location = new System.Drawing.Point(212, 1);
            this.voltageGraphButton.Name = "voltageGraphButton";
            this.voltageGraphButton.Size = new System.Drawing.Size(24, 23);
            this.voltageGraphButton.TabIndex = 104;
            this.voltageGraphButton.UseVisualStyleBackColor = false;
            this.voltageGraphButton.Click += new System.EventHandler(this.voltageGraphButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Voltage Ratio:";
            // 
            // voltageTxt
            // 
            this.voltageTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.voltageTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.voltageTxt.Location = new System.Drawing.Point(82, 6);
            this.voltageTxt.Name = "voltageTxt";
            this.voltageTxt.ReadOnly = true;
            this.voltageTxt.Size = new System.Drawing.Size(124, 13);
            this.voltageTxt.TabIndex = 16;
            this.voltageTxt.TabStop = false;
            // 
            // sensor_type_panel
            // 
            this.sensor_type_panel.Controls.Add(this.sensorTypeLbl);
            this.sensor_type_panel.Controls.Add(this.sensorTypeCmb);
            this.sensor_type_panel.Location = new System.Drawing.Point(3, 33);
            this.sensor_type_panel.Name = "sensor_type_panel";
            this.sensor_type_panel.Size = new System.Drawing.Size(246, 24);
            this.sensor_type_panel.TabIndex = 105;
            this.sensor_type_panel.Visible = false;
            // 
            // sensor_value_panel
            // 
            this.sensor_value_panel.Controls.Add(this.sensorGraphButton);
            this.sensor_value_panel.Controls.Add(this.label11);
            this.sensor_value_panel.Controls.Add(this.sensorTxt);
            this.sensor_value_panel.Enabled = false;
            this.sensor_value_panel.Location = new System.Drawing.Point(3, 63);
            this.sensor_value_panel.Name = "sensor_value_panel";
            this.sensor_value_panel.Size = new System.Drawing.Size(246, 24);
            this.sensor_value_panel.TabIndex = 105;
            this.sensor_value_panel.Visible = false;
            // 
            // sensorGraphButton
            // 
            this.sensorGraphButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorGraphButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.sensorGraphButton.Image = ((System.Drawing.Image)(resources.GetObject("sensorGraphButton.Image")));
            this.sensorGraphButton.Location = new System.Drawing.Point(212, 1);
            this.sensorGraphButton.Name = "sensorGraphButton";
            this.sensorGraphButton.Size = new System.Drawing.Size(24, 23);
            this.sensorGraphButton.TabIndex = 105;
            this.sensorGraphButton.UseVisualStyleBackColor = false;
            this.sensorGraphButton.Click += new System.EventHandler(this.sensorGraphButton_Click);
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
            // sensorTxt
            // 
            this.sensorTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sensorTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sensorTxt.Location = new System.Drawing.Point(82, 6);
            this.sensorTxt.Name = "sensorTxt";
            this.sensorTxt.ReadOnly = true;
            this.sensorTxt.Size = new System.Drawing.Size(124, 13);
            this.sensorTxt.TabIndex = 17;
            this.sensorTxt.TabStop = false;
            // 
            // calibrationPanel
            // 
            this.calibrationPanel.Controls.Add(this.weightGraphBtn);
            this.calibrationPanel.Controls.Add(this.label1);
            this.calibrationPanel.Controls.Add(this.weightTxt);
            this.calibrationPanel.Location = new System.Drawing.Point(3, 93);
            this.calibrationPanel.Name = "calibrationPanel";
            this.calibrationPanel.Size = new System.Drawing.Size(246, 24);
            this.calibrationPanel.TabIndex = 107;
            this.calibrationPanel.Visible = false;
            // 
            // weightGraphBtn
            // 
            this.weightGraphBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.weightGraphBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.weightGraphBtn.Image = ((System.Drawing.Image)(resources.GetObject("weightGraphBtn.Image")));
            this.weightGraphBtn.Location = new System.Drawing.Point(212, 1);
            this.weightGraphBtn.Name = "weightGraphBtn";
            this.weightGraphBtn.Size = new System.Drawing.Size(24, 23);
            this.weightGraphBtn.TabIndex = 104;
            this.weightGraphBtn.UseVisualStyleBackColor = false;
            this.weightGraphBtn.Click += new System.EventHandler(this.weightGraphBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Weight:";
            // 
            // weightTxt
            // 
            this.weightTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.weightTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.weightTxt.Location = new System.Drawing.Point(53, 6);
            this.weightTxt.Name = "weightTxt";
            this.weightTxt.ReadOnly = true;
            this.weightTxt.Size = new System.Drawing.Size(153, 13);
            this.weightTxt.TabIndex = 16;
            this.weightTxt.TabStop = false;
            // 
            // guiUpdateTimer
            // 
            this.guiUpdateTimer.Interval = 50;
            this.guiUpdateTimer.Tick += new System.EventHandler(this.guiUpdateTimer_Tick);
            // 
            // calibrationBox
            // 
            this.calibrationBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.calibrationBox.Controls.Add(this.gainSet);
            this.calibrationBox.Controls.Add(this.tareBtn);
            this.calibrationBox.Controls.Add(this.calibrateBtn);
            this.calibrationBox.Controls.Add(this.label3);
            this.calibrationBox.Controls.Add(this.cpyBtn);
            this.calibrationBox.Controls.Add(this.label2);
            this.calibrationBox.Controls.Add(this.linkLabel1);
            this.calibrationBox.Controls.Add(this.gainTxt);
            this.calibrationBox.Controls.Add(this.offsetTxt);
            this.calibrationBox.Location = new System.Drawing.Point(3, 159);
            this.calibrationBox.Name = "calibrationBox";
            this.calibrationBox.Size = new System.Drawing.Size(262, 129);
            this.calibrationBox.TabIndex = 17;
            this.calibrationBox.TabStop = false;
            this.calibrationBox.Text = "Calibration Settings";
            this.calibrationBox.Visible = false;
            // 
            // cpyBtn
            // 
            this.cpyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpyBtn.Location = new System.Drawing.Point(55, 83);
            this.cpyBtn.Name = "cpyBtn";
            this.cpyBtn.Size = new System.Drawing.Size(33, 16);
            this.cpyBtn.TabIndex = 111;
            this.cpyBtn.Text = "Copy";
            this.cpyBtn.UseVisualStyleBackColor = true;
            this.cpyBtn.Click += new System.EventHandler(this.cpyBtn_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(120, 105);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(124, 13);
            this.linkLabel1.TabIndex = 109;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Learn about calibration >";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // gainSet
            // 
            this.gainSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gainSet.Location = new System.Drawing.Point(89, 83);
            this.gainSet.Name = "gainSet";
            this.gainSet.Size = new System.Drawing.Size(29, 16);
            this.gainSet.TabIndex = 110;
            this.gainSet.Text = "Set";
            this.gainSet.UseVisualStyleBackColor = true;
            this.gainSet.Click += new System.EventHandler(this.gainSet_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.outputBox);
            this.flowLayoutPanel2.Controls.Add(this.calibrationBox);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(291, 102);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(268, 291);
            this.flowLayoutPanel2.TabIndex = 18;
            // 
            // phidgetInfoBox1
            // 
            this.phidgetInfoBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.phidgetInfoBox1.Location = new System.Drawing.Point(9, 12);
            this.phidgetInfoBox1.MinimumSize = new System.Drawing.Size(396, 84);
            this.phidgetInfoBox1.Name = "phidgetInfoBox1";
            this.phidgetInfoBox1.Size = new System.Drawing.Size(545, 84);
            this.phidgetInfoBox1.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(566, 398);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.phidgetInfoBox1);
            this.Controls.Add(this.settingsBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = " Voltage Ratio";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.settingsBox.ResumeLayout(false);
            this.bridgeSettings.ResumeLayout(false);
            this.bridgeSettings.PerformLayout();
            this.bridge_gain_panel.ResumeLayout(false);
            this.bridge_gain_panel.PerformLayout();
            this.interval_panel.ResumeLayout(false);
            this.interval_panel.PerformLayout();
            this.trigger_panel.ResumeLayout(false);
            this.trigger_panel.PerformLayout();
            this.outputBox.ResumeLayout(false);
            this.outputBox.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.sensor_type_panel.ResumeLayout(false);
            this.sensor_type_panel.PerformLayout();
            this.sensor_value_panel.ResumeLayout(false);
            this.sensor_value_panel.PerformLayout();
            this.calibrationPanel.ResumeLayout(false);
            this.calibrationPanel.PerformLayout();
            this.calibrationBox.ResumeLayout(false);
            this.calibrationBox.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox settingsBox;
        private System.Windows.Forms.GroupBox outputBox;
        private System.Windows.Forms.ComboBox gainCombo;
        private System.Windows.Forms.CheckBox enabledChk;
        private System.Windows.Forms.Label gainLbl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox sensorTypeCmb;
        private System.Windows.Forms.TextBox sensorTxt;
        private System.Windows.Forms.TextBox voltageTxt;
        private Phidget22.ExampleUtils.PhidgetInfoBox phidgetInfoBox1;
        private System.Windows.Forms.Label sensorTypeLbl;
        private System.Windows.Forms.Panel interval_panel;
        private System.Windows.Forms.Panel trigger_panel;
        private System.Windows.Forms.Panel bridge_gain_panel;
        private System.Windows.Forms.Panel sensor_value_panel;
        private System.Windows.Forms.Panel sensor_type_panel;
        private System.Windows.Forms.Panel panel1;
        private PhidgetsControlLibrary.CustomScroll changeTriggerTrk;
        private PhidgetsControlLibrary.CustomScroll dataIntervalTrk;
        private System.Windows.Forms.Button voltageGraphButton;
        private System.Windows.Forms.Button sensorGraphButton;
		private System.Windows.Forms.Timer guiUpdateTimer;
        private System.Windows.Forms.Panel calibrationPanel;
        private System.Windows.Forms.Button weightGraphBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox weightTxt;
        private System.Windows.Forms.Button calibrateBtn;
        private System.Windows.Forms.TextBox offsetTxt;
        private System.Windows.Forms.TextBox gainTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox bridgeSettings;
        private System.Windows.Forms.Button tareBtn;
        private System.Windows.Forms.GroupBox calibrationBox;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolTip urlTip;
        private System.Windows.Forms.Button gainSet;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button cpyBtn;
    }
}

