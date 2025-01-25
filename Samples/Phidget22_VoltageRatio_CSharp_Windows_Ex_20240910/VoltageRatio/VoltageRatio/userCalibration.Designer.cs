namespace VoltageRatio_Example
{
    partial class userCalibration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(userCalibration));
            this.instructionTxt = new System.Windows.Forms.RichTextBox();
            this.nextBtn = new System.Windows.Forms.Button();
            this.dataTxt = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.Label();
            this.dataLbl = new System.Windows.Forms.Label();
            this.capacityLbl = new System.Windows.Forms.Label();
            this.capacityTxt = new System.Windows.Forms.TextBox();
            this.unitLbl = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // instructionTxt
            // 
            this.instructionTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionTxt.BackColor = System.Drawing.SystemColors.Control;
            this.instructionTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.instructionTxt.Location = new System.Drawing.Point(15, 36);
            this.instructionTxt.Name = "instructionTxt";
            this.instructionTxt.Size = new System.Drawing.Size(360, 81);
            this.instructionTxt.TabIndex = 0;
            this.instructionTxt.Text = "";
            // 
            // nextBtn
            // 
            this.nextBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextBtn.Location = new System.Drawing.Point(300, 141);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(75, 23);
            this.nextBtn.TabIndex = 1;
            this.nextBtn.Text = "Continue";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // dataTxt
            // 
            this.dataTxt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dataTxt.Location = new System.Drawing.Point(134, 123);
            this.dataTxt.Name = "dataTxt";
            this.dataTxt.Size = new System.Drawing.Size(100, 20);
            this.dataTxt.TabIndex = 3;
            this.dataTxt.Visible = false;
            this.dataTxt.TextChanged += new System.EventHandler(this.dataTxt_TextChanged);
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Location = new System.Drawing.Point(12, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(35, 13);
            this.title.TabIndex = 4;
            this.title.Text = "label1";
            // 
            // dataLbl
            // 
            this.dataLbl.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dataLbl.AutoSize = true;
            this.dataLbl.Location = new System.Drawing.Point(57, 126);
            this.dataLbl.Name = "dataLbl";
            this.dataLbl.Size = new System.Drawing.Size(71, 13);
            this.dataLbl.TabIndex = 5;
            this.dataLbl.Text = "Input Weight:";
            this.dataLbl.Visible = false;
            // 
            // capacityLbl
            // 
            this.capacityLbl.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.capacityLbl.AutoSize = true;
            this.capacityLbl.Location = new System.Drawing.Point(57, 152);
            this.capacityLbl.Name = "capacityLbl";
            this.capacityLbl.Size = new System.Drawing.Size(51, 13);
            this.capacityLbl.TabIndex = 7;
            this.capacityLbl.Text = "Capacity:";
            this.capacityLbl.Visible = false;
            // 
            // capacityTxt
            // 
            this.capacityTxt.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.capacityTxt.Location = new System.Drawing.Point(134, 149);
            this.capacityTxt.Name = "capacityTxt";
            this.capacityTxt.Size = new System.Drawing.Size(100, 20);
            this.capacityTxt.TabIndex = 6;
            this.capacityTxt.Visible = false;
            this.capacityTxt.TextChanged += new System.EventHandler(this.capacityTxt_TextChanged);
            // 
            // unitLbl
            // 
            this.unitLbl.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.unitLbl.AutoSize = true;
            this.unitLbl.Location = new System.Drawing.Point(234, 126);
            this.unitLbl.Name = "unitLbl";
            this.unitLbl.Size = new System.Drawing.Size(34, 13);
            this.unitLbl.TabIndex = 8;
            this.unitLbl.Text = "mV/V";
            this.unitLbl.Visible = false;
            // 
            // linkLabel
            // 
            this.linkLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(154, 9);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(68, 13);
            this.linkLabel.TabIndex = 9;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "What is this?";
            this.linkLabel.Visible = false;
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel_LinkClicked);
            // 
            // userCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(384, 176);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(this.unitLbl);
            this.Controls.Add(this.capacityLbl);
            this.Controls.Add(this.capacityTxt);
            this.Controls.Add(this.dataLbl);
            this.Controls.Add(this.title);
            this.Controls.Add(this.dataTxt);
            this.Controls.Add(this.nextBtn);
            this.Controls.Add(this.instructionTxt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 215);
            this.Name = "userCalibration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calibration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.userCalibration_FormClosing);
            this.Load += new System.EventHandler(this.userCalibration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox instructionTxt;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.TextBox dataTxt;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label dataLbl;
        private System.Windows.Forms.Label capacityLbl;
        private System.Windows.Forms.TextBox capacityTxt;
        private System.Windows.Forms.Label unitLbl;
        private System.Windows.Forms.LinkLabel linkLabel;
    }
}