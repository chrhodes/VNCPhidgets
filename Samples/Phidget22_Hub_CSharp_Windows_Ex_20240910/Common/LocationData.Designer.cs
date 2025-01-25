namespace Phidget22.ExampleUtils
{
    partial class LocationData
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LocationData));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.serialTxt = new System.Windows.Forms.Label();
			this.portTxt = new System.Windows.Forms.Label();
			this.channelTxt = new System.Windows.Forms.Label();
			this.hubportTxt = new System.Windows.Forms.Label();
			this.serverTxt = new System.Windows.Forms.Label();
			this.ipTxt = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.classTxt = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.PropertyInfo = new System.Windows.Forms.ToolTip(this.components);
			this.label13 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.labelTxt = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label1.Location = new System.Drawing.Point(34, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Device/Hub Serial Number";
			this.PropertyInfo.SetToolTip(this.label1, "Serial number of your device or VINT hub the device is connected to.");
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label2.Location = new System.Drawing.Point(34, 77);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Hub Port";
			this.PropertyInfo.SetToolTip(this.label2, "If your device is connected to a VINT hub, this shows the port number that it is " +
        "plugged into.  It will be 0 otherwise.");
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label3.Location = new System.Drawing.Point(34, 102);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Device Channel";
			this.PropertyInfo.SetToolTip(this.label3, "This is the channel on the device that you have opened.");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label4.Location = new System.Drawing.Point(266, 51);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(103, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Is Hub Port Device?";
			this.PropertyInfo.SetToolTip(this.label4, "If you are opening a VINT port on your hub as a input/output channel rather than " +
        "connecting to an external VINT device then this will be true.");
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label5.Location = new System.Drawing.Point(34, 139);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(81, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Network Server";
			this.PropertyInfo.SetToolTip(this.label5, "This is the SSID of the network server you are connecting to if you have opened a" +
        " remote device.");
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label6.Location = new System.Drawing.Point(289, 139);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "IP Address:Port";
			this.PropertyInfo.SetToolTip(this.label6, "This is the IP address and the port of the network server you have connected to i" +
        "f you are connecting to a remote device.");
			// 
			// label7
			// 
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label7.Location = new System.Drawing.Point(-4, 125);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(549, 2);
			this.label7.TabIndex = 6;
			// 
			// serialTxt
			// 
			this.serialTxt.AutoSize = true;
			this.serialTxt.Location = new System.Drawing.Point(179, 51);
			this.serialTxt.Name = "serialTxt";
			this.serialTxt.Size = new System.Drawing.Size(53, 13);
			this.serialTxt.TabIndex = 7;
			this.serialTxt.Text = "Unknown";
			// 
			// portTxt
			// 
			this.portTxt.AutoSize = true;
			this.portTxt.Location = new System.Drawing.Point(179, 77);
			this.portTxt.Name = "portTxt";
			this.portTxt.Size = new System.Drawing.Size(53, 13);
			this.portTxt.TabIndex = 8;
			this.portTxt.Text = "Unknown";
			// 
			// channelTxt
			// 
			this.channelTxt.AutoSize = true;
			this.channelTxt.Location = new System.Drawing.Point(179, 102);
			this.channelTxt.Name = "channelTxt";
			this.channelTxt.Size = new System.Drawing.Size(53, 13);
			this.channelTxt.TabIndex = 9;
			this.channelTxt.Text = "Unknown";
			// 
			// hubportTxt
			// 
			this.hubportTxt.AutoSize = true;
			this.hubportTxt.Location = new System.Drawing.Point(383, 51);
			this.hubportTxt.Name = "hubportTxt";
			this.hubportTxt.Size = new System.Drawing.Size(53, 13);
			this.hubportTxt.TabIndex = 10;
			this.hubportTxt.Text = "Unknown";
			// 
			// serverTxt
			// 
			this.serverTxt.AutoSize = true;
			this.serverTxt.Location = new System.Drawing.Point(179, 139);
			this.serverTxt.Name = "serverTxt";
			this.serverTxt.Size = new System.Drawing.Size(53, 13);
			this.serverTxt.TabIndex = 11;
			this.serverTxt.Text = "Unknown";
			// 
			// ipTxt
			// 
			this.ipTxt.AutoSize = true;
			this.ipTxt.Location = new System.Drawing.Point(383, 140);
			this.ipTxt.Name = "ipTxt";
			this.ipTxt.Size = new System.Drawing.Size(53, 13);
			this.ipTxt.TabIndex = 12;
			this.ipTxt.Text = "Unknown";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label8.Location = new System.Drawing.Point(295, 77);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(74, 13);
			this.label8.TabIndex = 13;
			this.label8.Text = "Channel Class";
			this.PropertyInfo.SetToolTip(this.label8, "This is the software class being used by the object you have opened.  You can use" +
        " this to find the functions for the device in the Phidget\'s API.");
			// 
			// classTxt
			// 
			this.classTxt.AutoSize = true;
			this.classTxt.Location = new System.Drawing.Point(383, 77);
			this.classTxt.Name = "classTxt";
			this.classTxt.Size = new System.Drawing.Size(53, 13);
			this.classTxt.TabIndex = 14;
			this.classTxt.Text = "Unknown";
			// 
			// label9
			// 
			this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label9.Location = new System.Drawing.Point(171, 53);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(2, 70);
			this.label9.TabIndex = 15;
			// 
			// label10
			// 
			this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label10.Location = new System.Drawing.Point(375, 53);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(2, 70);
			this.label10.TabIndex = 16;
			// 
			// label11
			// 
			this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label11.Location = new System.Drawing.Point(171, 139);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(2, 20);
			this.label11.TabIndex = 17;
			// 
			// label12
			// 
			this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label12.Location = new System.Drawing.Point(375, 138);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(2, 20);
			this.label12.TabIndex = 18;
			// 
			// PropertyInfo
			// 
			this.PropertyInfo.AutoPopDelay = 20000;
			this.PropertyInfo.InitialDelay = 50;
			this.PropertyInfo.ReshowDelay = 100;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.ForeColor = System.Drawing.SystemColors.Highlight;
			this.label13.Location = new System.Drawing.Point(299, 102);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(70, 13);
			this.label13.TabIndex = 20;
			this.label13.Text = "Device Label";
			this.PropertyInfo.SetToolTip(this.label13, "This is the channel on the device that you have opened.");
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
			this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(70, 7);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(438, 41);
			this.textBox1.TabIndex = 19;
			this.textBox1.TabStop = false;
			this.textBox1.Text = resources.GetString("textBox1.Text");
			// 
			// labelTxt
			// 
			this.labelTxt.AutoSize = true;
			this.labelTxt.Location = new System.Drawing.Point(383, 102);
			this.labelTxt.Name = "labelTxt";
			this.labelTxt.Size = new System.Drawing.Size(53, 13);
			this.labelTxt.TabIndex = 21;
			this.labelTxt.Text = "Unknown";
			// 
			// LocationData
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(533, 167);
			this.Controls.Add(this.labelTxt);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.classTxt);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.ipTxt);
			this.Controls.Add(this.serverTxt);
			this.Controls.Add(this.hubportTxt);
			this.Controls.Add(this.channelTxt);
			this.Controls.Add(this.portTxt);
			this.Controls.Add(this.serialTxt);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LocationData";
			this.Text = "Addressing Information";
			this.Load += new System.EventHandler(this.LocationData_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label serialTxt;
        private System.Windows.Forms.Label portTxt;
        private System.Windows.Forms.Label channelTxt;
        private System.Windows.Forms.Label hubportTxt;
        private System.Windows.Forms.Label serverTxt;
        private System.Windows.Forms.Label ipTxt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label classTxt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolTip PropertyInfo;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelTxt;
    }
}