namespace LoadCellCalibrator
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
            this.deviceStatus = new System.Windows.Forms.StatusStrip();
            this.warningIcon = new System.Windows.Forms.ToolStripStatusLabel();
            this.serialNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.channelAttached = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.box1 = new System.Windows.Forms.GroupBox();
            this.waitingLbl = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.measurement1 = new System.Windows.Forms.TextBox();
            this.capture1 = new System.Windows.Forms.Button();
            this.load1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.box2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.measurement2 = new System.Windows.Forms.TextBox();
            this.capture2 = new System.Windows.Forms.Button();
            this.load2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.mTxt = new System.Windows.Forms.TextBox();
            this.bTxt = new System.Windows.Forms.TextBox();
            this.outputBox = new System.Windows.Forms.GroupBox();
            this.correctedLoad = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.channelSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.warningTimer = new System.Windows.Forms.Timer(this.components);
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceStatus.SuspendLayout();
            this.box1.SuspendLayout();
            this.box2.SuspendLayout();
            this.outputBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceStatus
            // 
            this.deviceStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serialNumber,
            this.channelAttached,
            this.warningIcon});
            this.deviceStatus.Location = new System.Drawing.Point(0, 634);
            this.deviceStatus.Name = "deviceStatus";
            this.deviceStatus.Size = new System.Drawing.Size(800, 22);
            this.deviceStatus.TabIndex = 0;
            this.deviceStatus.Text = "statusStrip1";
            // 
            // warningIcon
            // 
            this.warningIcon.ForeColor = System.Drawing.Color.Red;
            this.warningIcon.Image = global::LoadCellCalibrator.Properties.Resources.warninglarge;
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(16, 17);
            this.warningIcon.Visible = false;
            // 
            // serialNumber
            // 
            this.serialNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.serialNumber.Name = "serialNumber";
            this.serialNumber.Size = new System.Drawing.Size(0, 17);
            // 
            // channelAttached
            // 
            this.channelAttached.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.channelAttached.Name = "channelAttached";
            this.channelAttached.Size = new System.Drawing.Size(0, 17);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(204, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(387, 65);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // box1
            // 
            this.box1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.box1.Controls.Add(this.waitingLbl);
            this.box1.Controls.Add(this.label4);
            this.box1.Controls.Add(this.measurement1);
            this.box1.Controls.Add(this.capture1);
            this.box1.Controls.Add(this.load1);
            this.box1.Controls.Add(this.label3);
            this.box1.Controls.Add(this.label2);
            this.box1.Enabled = false;
            this.box1.Location = new System.Drawing.Point(23, 111);
            this.box1.Name = "box1";
            this.box1.Size = new System.Drawing.Size(749, 89);
            this.box1.TabIndex = 2;
            this.box1.TabStop = false;
            this.box1.Text = "Point 1";
            // 
            // waitingLbl
            // 
            this.waitingLbl.AutoSize = true;
            this.waitingLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waitingLbl.Location = new System.Drawing.Point(6, 17);
            this.waitingLbl.Name = "waitingLbl";
            this.waitingLbl.Size = new System.Drawing.Size(694, 55);
            this.waitingLbl.TabIndex = 6;
            this.waitingLbl.Text = "Waiting for device attachment...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(465, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Measurement:";
            // 
            // measurement1
            // 
            this.measurement1.Location = new System.Drawing.Point(545, 35);
            this.measurement1.Name = "measurement1";
            this.measurement1.ReadOnly = true;
            this.measurement1.Size = new System.Drawing.Size(100, 20);
            this.measurement1.TabIndex = 4;
            // 
            // capture1
            // 
            this.capture1.Location = new System.Drawing.Point(361, 33);
            this.capture1.Name = "capture1";
            this.capture1.Size = new System.Drawing.Size(75, 23);
            this.capture1.TabIndex = 3;
            this.capture1.Text = "Capture";
            this.capture1.UseVisualStyleBackColor = true;
            this.capture1.Click += new System.EventHandler(this.point1_Click);
            // 
            // load1
            // 
            this.load1.Location = new System.Drawing.Point(255, 35);
            this.load1.Name = "load1";
            this.load1.Size = new System.Drawing.Size(100, 20);
            this.load1.TabIndex = 2;
            this.load1.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Load (any unit):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 52);
            this.label2.TabIndex = 0;
            this.label2.Text = "The first point we can take\r\nat 0 load.  Make sure there \r\nis no external force o" +
    "n the \r\nload cell and click \"Capture\":";
            // 
            // box2
            // 
            this.box2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.box2.Controls.Add(this.label5);
            this.box2.Controls.Add(this.measurement2);
            this.box2.Controls.Add(this.capture2);
            this.box2.Controls.Add(this.load2);
            this.box2.Controls.Add(this.label6);
            this.box2.Controls.Add(this.label7);
            this.box2.Enabled = false;
            this.box2.Location = new System.Drawing.Point(23, 206);
            this.box2.Name = "box2";
            this.box2.Size = new System.Drawing.Size(749, 120);
            this.box2.TabIndex = 6;
            this.box2.TabStop = false;
            this.box2.Text = "Point 2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(465, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Measurement:";
            // 
            // measurement2
            // 
            this.measurement2.Location = new System.Drawing.Point(545, 56);
            this.measurement2.Name = "measurement2";
            this.measurement2.ReadOnly = true;
            this.measurement2.Size = new System.Drawing.Size(100, 20);
            this.measurement2.TabIndex = 4;
            // 
            // capture2
            // 
            this.capture2.Location = new System.Drawing.Point(361, 54);
            this.capture2.Name = "capture2";
            this.capture2.Size = new System.Drawing.Size(75, 23);
            this.capture2.TabIndex = 3;
            this.capture2.Text = "Capture";
            this.capture2.UseVisualStyleBackColor = true;
            this.capture2.Click += new System.EventHandler(this.point2_Click);
            // 
            // load2
            // 
            this.load2.Location = new System.Drawing.Point(255, 56);
            this.load2.Name = "load2";
            this.load2.Size = new System.Drawing.Size(100, 20);
            this.load2.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Load (any unit):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 91);
            this.label7.TabIndex = 0;
            this.label7.Text = "The second point needs\r\nto be taken at some non-\r\nzero load value.  Put a\r\nweight" +
    " on the load cell\r\nand enter the weight value\r\ninto the load box.  Then\r\nclick \"" +
    "Capture\":";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(181, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(387, 52);
            this.label8.TabIndex = 7;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(324, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "y = m * x + b";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(147, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Load = ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(319, 124);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "* V/V load cell output + ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(219, 167);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(317, 65);
            this.label12.TabIndex = 11;
            this.label12.Text = resources.GetString("label12.Text");
            // 
            // mTxt
            // 
            this.mTxt.Location = new System.Drawing.Point(202, 121);
            this.mTxt.Name = "mTxt";
            this.mTxt.ReadOnly = true;
            this.mTxt.Size = new System.Drawing.Size(100, 20);
            this.mTxt.TabIndex = 12;
            // 
            // bTxt
            // 
            this.bTxt.Location = new System.Drawing.Point(458, 121);
            this.bTxt.Name = "bTxt";
            this.bTxt.ReadOnly = true;
            this.bTxt.Size = new System.Drawing.Size(100, 20);
            this.bTxt.TabIndex = 13;
            // 
            // outputBox
            // 
            this.outputBox.Controls.Add(this.correctedLoad);
            this.outputBox.Controls.Add(this.label13);
            this.outputBox.Controls.Add(this.label8);
            this.outputBox.Controls.Add(this.bTxt);
            this.outputBox.Controls.Add(this.label9);
            this.outputBox.Controls.Add(this.mTxt);
            this.outputBox.Controls.Add(this.label10);
            this.outputBox.Controls.Add(this.label12);
            this.outputBox.Controls.Add(this.label11);
            this.outputBox.Enabled = false;
            this.outputBox.Location = new System.Drawing.Point(23, 332);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(749, 285);
            this.outputBox.TabIndex = 14;
            this.outputBox.TabStop = false;
            this.outputBox.Text = "Output";
            // 
            // correctedLoad
            // 
            this.correctedLoad.Location = new System.Drawing.Point(343, 247);
            this.correctedLoad.Name = "correctedLoad";
            this.correctedLoad.ReadOnly = true;
            this.correctedLoad.Size = new System.Drawing.Size(100, 20);
            this.correctedLoad.TabIndex = 15;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(303, 250);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "Load:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.channelSelect,
            this.resetToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // channelSelect
            // 
            this.channelSelect.Name = "channelSelect";
            this.channelSelect.Size = new System.Drawing.Size(63, 20);
            this.channelSelect.Text = "Channel";
            // 
            // warningTimer
            // 
            this.warningTimer.Interval = 5000;
            this.warningTimer.Tick += new System.EventHandler(this.warningTimer_Tick);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(800, 656);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.box2);
            this.Controls.Add(this.box1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.deviceStatus);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Load Cell Calibrator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.deviceStatus.ResumeLayout(false);
            this.deviceStatus.PerformLayout();
            this.box1.ResumeLayout(false);
            this.box1.PerformLayout();
            this.box2.ResumeLayout(false);
            this.box2.PerformLayout();
            this.outputBox.ResumeLayout(false);
            this.outputBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip deviceStatus;
        private System.Windows.Forms.ToolStripStatusLabel warningIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox box1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox measurement1;
        private System.Windows.Forms.Button capture1;
        private System.Windows.Forms.TextBox load1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox box2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox measurement2;
        private System.Windows.Forms.Button capture2;
        private System.Windows.Forms.TextBox load2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox mTxt;
        private System.Windows.Forms.TextBox bTxt;
        private System.Windows.Forms.GroupBox outputBox;
        private System.Windows.Forms.TextBox correctedLoad;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ToolStripStatusLabel serialNumber;
        private System.Windows.Forms.Label waitingLbl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem channelSelect;
        private System.Windows.Forms.ToolStripStatusLabel channelAttached;
        private System.Windows.Forms.Timer warningTimer;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
    }
}