
namespace Phidget22.ExampleUtils
{
    partial class PhidgetInfoBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhidgetInfoBox));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.locationButton = new System.Windows.Forms.Button();
			this.remoteLbl = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.attachedDeviceNameTxt = new System.Windows.Forms.TextBox();
			this.hubPortTxt = new System.Windows.Forms.TextBox();
			this.serialNumberTxt = new System.Windows.Forms.TextBox();
			this.channelTxt = new System.Windows.Forms.TextBox();
			this.versionTxt = new System.Windows.Forms.TextBox();
			this.hubPortLbl = new System.Windows.Forms.Label();
			this.serialNumberLbl = new System.Windows.Forms.Label();
			this.line1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.locationButton);
			this.groupBox1.Controls.Add(this.remoteLbl);
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.attachedDeviceNameTxt);
			this.groupBox1.Controls.Add(this.hubPortTxt);
			this.groupBox1.Controls.Add(this.serialNumberTxt);
			this.groupBox1.Controls.Add(this.channelTxt);
			this.groupBox1.Controls.Add(this.versionTxt);
			this.groupBox1.Controls.Add(this.hubPortLbl);
			this.groupBox1.Controls.Add(this.serialNumberLbl);
			this.groupBox1.Controls.Add(this.line1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(396, 84);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Phidget Info";
			// 
			// locationButton
			// 
			this.locationButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.locationButton.Image = ((System.Drawing.Image)(resources.GetObject("locationButton.Image")));
			this.locationButton.Location = new System.Drawing.Point(279, 42);
			this.locationButton.Name = "locationButton";
			this.locationButton.Size = new System.Drawing.Size(22, 23);
			this.locationButton.TabIndex = 30;
			this.locationButton.UseVisualStyleBackColor = true;
			this.locationButton.Click += new System.EventHandler(this.locationButton_Click);
			// 
			// remoteLbl
			// 
			this.remoteLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.remoteLbl.AutoSize = true;
			this.remoteLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.remoteLbl.Location = new System.Drawing.Point(256, 68);
			this.remoteLbl.Name = "remoteLbl";
			this.remoteLbl.Size = new System.Drawing.Size(45, 9);
			this.remoteLbl.TabIndex = 29;
			this.remoteLbl.Text = "REMOTE";
			this.remoteLbl.Visible = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox1.Location = new System.Drawing.Point(307, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(83, 74);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 28;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Visible = false;
			// 
			// attachedDeviceNameTxt
			// 
			this.attachedDeviceNameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.attachedDeviceNameTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.attachedDeviceNameTxt.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.attachedDeviceNameTxt.Location = new System.Drawing.Point(65, 17);
			this.attachedDeviceNameTxt.Name = "attachedDeviceNameTxt";
			this.attachedDeviceNameTxt.ReadOnly = true;
			this.attachedDeviceNameTxt.Size = new System.Drawing.Size(236, 17);
			this.attachedDeviceNameTxt.TabIndex = 27;
			this.attachedDeviceNameTxt.TabStop = false;
			this.attachedDeviceNameTxt.Text = "Nothing";
			// 
			// hubPortTxt
			// 
			this.hubPortTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.hubPortTxt.Location = new System.Drawing.Point(229, 65);
			this.hubPortTxt.MinimumSize = new System.Drawing.Size(10, 13);
			this.hubPortTxt.Name = "hubPortTxt";
			this.hubPortTxt.ReadOnly = true;
			this.hubPortTxt.Size = new System.Drawing.Size(30, 13);
			this.hubPortTxt.TabIndex = 26;
			this.hubPortTxt.TabStop = false;
			this.hubPortTxt.Text = "???";
			// 
			// serialNumberTxt
			// 
			this.serialNumberTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.serialNumberTxt.Location = new System.Drawing.Point(229, 47);
			this.serialNumberTxt.MinimumSize = new System.Drawing.Size(72, 13);
			this.serialNumberTxt.Name = "serialNumberTxt";
			this.serialNumberTxt.ReadOnly = true;
			this.serialNumberTxt.Size = new System.Drawing.Size(72, 13);
			this.serialNumberTxt.TabIndex = 25;
			this.serialNumberTxt.TabStop = false;
			this.serialNumberTxt.Text = "???";
			// 
			// channelTxt
			// 
			this.channelTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.channelTxt.Location = new System.Drawing.Point(65, 65);
			this.channelTxt.Name = "channelTxt";
			this.channelTxt.ReadOnly = true;
			this.channelTxt.Size = new System.Drawing.Size(42, 13);
			this.channelTxt.TabIndex = 24;
			this.channelTxt.TabStop = false;
			this.channelTxt.Text = "???";
			// 
			// versionTxt
			// 
			this.versionTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.versionTxt.Location = new System.Drawing.Point(65, 47);
			this.versionTxt.Name = "versionTxt";
			this.versionTxt.ReadOnly = true;
			this.versionTxt.Size = new System.Drawing.Size(42, 13);
			this.versionTxt.TabIndex = 23;
			this.versionTxt.TabStop = false;
			this.versionTxt.Text = "???";
			// 
			// hubPortLbl
			// 
			this.hubPortLbl.Location = new System.Drawing.Point(156, 65);
			this.hubPortLbl.Name = "hubPortLbl";
			this.hubPortLbl.Size = new System.Drawing.Size(64, 13);
			this.hubPortLbl.TabIndex = 21;
			this.hubPortLbl.Text = "Hub Port:";
			this.hubPortLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// serialNumberLbl
			// 
			this.serialNumberLbl.Location = new System.Drawing.Point(113, 47);
			this.serialNumberLbl.Name = "serialNumberLbl";
			this.serialNumberLbl.Size = new System.Drawing.Size(107, 13);
			this.serialNumberLbl.TabIndex = 19;
			this.serialNumberLbl.Text = "Serial Number:";
			this.serialNumberLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// line1
			// 
			this.line1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.line1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.line1.Location = new System.Drawing.Point(6, 40);
			this.line1.Name = "line1";
			this.line1.Size = new System.Drawing.Size(384, 1);
			this.line1.TabIndex = 15;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(45, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "Version:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Attached:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(10, 65);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(49, 13);
			this.label7.TabIndex = 4;
			this.label7.Text = "Channel:";
			// 
			// PhidgetInfoBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.Controls.Add(this.groupBox1);
			this.MinimumSize = new System.Drawing.Size(396, 84);
			this.Name = "PhidgetInfoBox";
			this.Size = new System.Drawing.Size(396, 84);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label line1;
        private System.Windows.Forms.Label hubPortLbl;
        private System.Windows.Forms.Label serialNumberLbl;
        private System.Windows.Forms.TextBox hubPortTxt;
        private System.Windows.Forms.TextBox serialNumberTxt;
        private System.Windows.Forms.TextBox channelTxt;
        private System.Windows.Forms.TextBox versionTxt;
        private System.Windows.Forms.TextBox attachedDeviceNameTxt;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label remoteLbl;
        private System.Windows.Forms.Button locationButton;
    }
}
