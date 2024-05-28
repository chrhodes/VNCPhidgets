namespace MotorControl_full
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numInputsTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numMotorsTxt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.versiontxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.serialTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nameTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.attachedTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.input3Chk = new System.Windows.Forms.CheckBox();
            this.input2Chk = new System.Windows.Forms.CheckBox();
            this.input1Chk = new System.Windows.Forms.CheckBox();
            this.input0Chk = new System.Windows.Forms.CheckBox();
            this.motor1Grp = new System.Windows.Forms.GroupBox();
            this.accelTrk = new System.Windows.Forms.TrackBar();
            this.label13 = new System.Windows.Forms.Label();
            this.cChangeTxt = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.motorCmb = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.velocityTrk = new System.Windows.Forms.TrackBar();
            this.AccelTxt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.vMaxTxt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.vCurrentTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.motor1Grp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accelTrk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.velocityTrk)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numInputsTxt);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numMotorsTxt);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.versiontxt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.serialTxt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nameTxt);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.attachedTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 247);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MotorControl Details";
            // 
            // numInputsTxt
            // 
            this.numInputsTxt.Location = new System.Drawing.Point(118, 209);
            this.numInputsTxt.Name = "numInputsTxt";
            this.numInputsTxt.ReadOnly = true;
            this.numInputsTxt.Size = new System.Drawing.Size(163, 20);
            this.numInputsTxt.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "# Inputs:";
            // 
            // numMotorsTxt
            // 
            this.numMotorsTxt.Location = new System.Drawing.Point(118, 180);
            this.numMotorsTxt.Name = "numMotorsTxt";
            this.numMotorsTxt.ReadOnly = true;
            this.numMotorsTxt.Size = new System.Drawing.Size(163, 20);
            this.numMotorsTxt.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(56, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "# Motors:";
            // 
            // versiontxt
            // 
            this.versiontxt.Location = new System.Drawing.Point(118, 151);
            this.versiontxt.Name = "versiontxt";
            this.versiontxt.ReadOnly = true;
            this.versiontxt.Size = new System.Drawing.Size(163, 20);
            this.versiontxt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Version:";
            // 
            // serialTxt
            // 
            this.serialTxt.Location = new System.Drawing.Point(118, 120);
            this.serialTxt.Name = "serialTxt";
            this.serialTxt.ReadOnly = true;
            this.serialTxt.Size = new System.Drawing.Size(163, 20);
            this.serialTxt.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Serial No.:";
            // 
            // nameTxt
            // 
            this.nameTxt.Location = new System.Drawing.Point(118, 59);
            this.nameTxt.Multiline = true;
            this.nameTxt.Name = "nameTxt";
            this.nameTxt.ReadOnly = true;
            this.nameTxt.Size = new System.Drawing.Size(163, 48);
            this.nameTxt.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(56, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // attachedTxt
            // 
            this.attachedTxt.Location = new System.Drawing.Point(118, 28);
            this.attachedTxt.Name = "attachedTxt";
            this.attachedTxt.ReadOnly = true;
            this.attachedTxt.Size = new System.Drawing.Size(163, 20);
            this.attachedTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Attached:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.input3Chk);
            this.groupBox2.Controls.Add(this.input2Chk);
            this.groupBox2.Controls.Add(this.input1Chk);
            this.groupBox2.Controls.Add(this.input0Chk);
            this.groupBox2.Location = new System.Drawing.Point(12, 523);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(287, 69);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input Data";
            // 
            // input3Chk
            // 
            this.input3Chk.AutoSize = true;
            this.input3Chk.Enabled = false;
            this.input3Chk.Location = new System.Drawing.Point(212, 27);
            this.input3Chk.Name = "input3Chk";
            this.input3Chk.Size = new System.Drawing.Size(59, 17);
            this.input3Chk.TabIndex = 3;
            this.input3Chk.Text = "Input 3";
            this.input3Chk.UseVisualStyleBackColor = true;
            // 
            // input2Chk
            // 
            this.input2Chk.AutoSize = true;
            this.input2Chk.Enabled = false;
            this.input2Chk.Location = new System.Drawing.Point(147, 27);
            this.input2Chk.Name = "input2Chk";
            this.input2Chk.Size = new System.Drawing.Size(59, 17);
            this.input2Chk.TabIndex = 2;
            this.input2Chk.Text = "Input 2";
            this.input2Chk.UseVisualStyleBackColor = true;
            // 
            // input1Chk
            // 
            this.input1Chk.AutoSize = true;
            this.input1Chk.Enabled = false;
            this.input1Chk.Location = new System.Drawing.Point(82, 27);
            this.input1Chk.Name = "input1Chk";
            this.input1Chk.Size = new System.Drawing.Size(59, 17);
            this.input1Chk.TabIndex = 1;
            this.input1Chk.Text = "Input 1";
            this.input1Chk.UseVisualStyleBackColor = true;
            // 
            // input0Chk
            // 
            this.input0Chk.AutoSize = true;
            this.input0Chk.Enabled = false;
            this.input0Chk.Location = new System.Drawing.Point(17, 27);
            this.input0Chk.Name = "input0Chk";
            this.input0Chk.Size = new System.Drawing.Size(59, 17);
            this.input0Chk.TabIndex = 0;
            this.input0Chk.Text = "Input 0";
            this.input0Chk.UseVisualStyleBackColor = true;
            // 
            // motor1Grp
            // 
            this.motor1Grp.Controls.Add(this.accelTrk);
            this.motor1Grp.Controls.Add(this.label13);
            this.motor1Grp.Controls.Add(this.cChangeTxt);
            this.motor1Grp.Controls.Add(this.label12);
            this.motor1Grp.Controls.Add(this.motorCmb);
            this.motor1Grp.Controls.Add(this.label11);
            this.motor1Grp.Controls.Add(this.label10);
            this.motor1Grp.Controls.Add(this.velocityTrk);
            this.motor1Grp.Controls.Add(this.AccelTxt);
            this.motor1Grp.Controls.Add(this.label9);
            this.motor1Grp.Controls.Add(this.vMaxTxt);
            this.motor1Grp.Controls.Add(this.label8);
            this.motor1Grp.Controls.Add(this.vCurrentTxt);
            this.motor1Grp.Controls.Add(this.label7);
            this.motor1Grp.Location = new System.Drawing.Point(12, 265);
            this.motor1Grp.Name = "motor1Grp";
            this.motor1Grp.Size = new System.Drawing.Size(287, 252);
            this.motor1Grp.TabIndex = 2;
            this.motor1Grp.TabStop = false;
            this.motor1Grp.Text = "Motor Data";
            // 
            // accelTrk
            // 
            this.accelTrk.Location = new System.Drawing.Point(150, 197);
            this.accelTrk.Name = "accelTrk";
            this.accelTrk.Size = new System.Drawing.Size(131, 45);
            this.accelTrk.TabIndex = 13;
            this.accelTrk.Scroll += new System.EventHandler(this.accelTrk_Scroll);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(147, 181);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Set Acceleration:";
            // 
            // cChangeTxt
            // 
            this.cChangeTxt.Location = new System.Drawing.Point(138, 146);
            this.cChangeTxt.Name = "cChangeTxt";
            this.cChangeTxt.ReadOnly = true;
            this.cChangeTxt.Size = new System.Drawing.Size(143, 20);
            this.cChangeTxt.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(44, 149);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Current:";
            // 
            // motorCmb
            // 
            this.motorCmb.FormattingEnabled = true;
            this.motorCmb.Location = new System.Drawing.Point(117, 19);
            this.motorCmb.Name = "motorCmb";
            this.motorCmb.Size = new System.Drawing.Size(164, 21);
            this.motorCmb.TabIndex = 9;
            this.motorCmb.SelectedIndexChanged += new System.EventHandler(this.motorCmb_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(35, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "Choose Motor:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 181);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Set Maximum Velocity:";
            // 
            // velocityTrk
            // 
            this.velocityTrk.Location = new System.Drawing.Point(9, 197);
            this.velocityTrk.Name = "velocityTrk";
            this.velocityTrk.Size = new System.Drawing.Size(132, 45);
            this.velocityTrk.TabIndex = 6;
            this.velocityTrk.Scroll += new System.EventHandler(this.velocityTrk_Scroll);
            // 
            // AccelTxt
            // 
            this.AccelTxt.Location = new System.Drawing.Point(137, 117);
            this.AccelTxt.Name = "AccelTxt";
            this.AccelTxt.ReadOnly = true;
            this.AccelTxt.Size = new System.Drawing.Size(144, 20);
            this.AccelTxt.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(44, 120);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Acceleration:";
            // 
            // vMaxTxt
            // 
            this.vMaxTxt.Location = new System.Drawing.Point(137, 88);
            this.vMaxTxt.Name = "vMaxTxt";
            this.vMaxTxt.ReadOnly = true;
            this.vMaxTxt.Size = new System.Drawing.Size(144, 20);
            this.vMaxTxt.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(44, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Maximum Velocity:";
            // 
            // vCurrentTxt
            // 
            this.vCurrentTxt.Location = new System.Drawing.Point(137, 58);
            this.vCurrentTxt.Name = "vCurrentTxt";
            this.vCurrentTxt.ReadOnly = true;
            this.vCurrentTxt.Size = new System.Drawing.Size(144, 20);
            this.vCurrentTxt.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Current Velocity:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 601);
            this.Controls.Add(this.motor1Grp);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(321, 635);
            this.MinimumSize = new System.Drawing.Size(321, 635);
            this.Name = "Form1";
            this.Text = "MotorControl-full";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.motor1Grp.ResumeLayout(false);
            this.motor1Grp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accelTrk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.velocityTrk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox attachedTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox versiontxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox serialTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox nameTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox numInputsTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox numMotorsTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox input3Chk;
        private System.Windows.Forms.CheckBox input2Chk;
        private System.Windows.Forms.CheckBox input1Chk;
        private System.Windows.Forms.CheckBox input0Chk;
        private System.Windows.Forms.GroupBox motor1Grp;
        private System.Windows.Forms.TextBox AccelTxt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox vMaxTxt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox vCurrentTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar velocityTrk;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox motorCmb;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox cChangeTxt;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TrackBar accelTrk;
        private System.Windows.Forms.Label label13;
    }
}

