namespace PhidgetsControlLibrary
{
    partial class CustomScroll
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomScroll));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.slider = new System.Windows.Forms.TrackBar();
            this.maxLbl = new System.Windows.Forms.Label();
            this.trkName = new System.Windows.Forms.Label();
            this.minLbl = new System.Windows.Forms.Label();
            this.setBtn = new System.Windows.Forms.Button();
            this.tooltipPic = new System.Windows.Forms.PictureBox();
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.firmwareValueLbl = new System.Windows.Forms.Label();
            this.trkTxt = new System.Windows.Forms.TextBox();
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.slider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tooltipPic)).BeginInit();
            this.SuspendLayout();
            // 
            // slider
            // 
            this.slider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.slider.LargeChange = 1;
            this.slider.Location = new System.Drawing.Point(83, -1);
            this.slider.Name = "slider";
            this.slider.Size = new System.Drawing.Size(180, 45);
            this.slider.TabIndex = 0;
            this.slider.Scroll += new System.EventHandler(this.slider_Scroll);
            this.slider.KeyUp += new System.Windows.Forms.KeyEventHandler(this.slider_KeyUp);
            // 
            // maxLbl
            // 
            this.maxLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.maxLbl.Location = new System.Drawing.Point(233, 25);
            this.maxLbl.Name = "maxLbl";
            this.maxLbl.Size = new System.Drawing.Size(49, 10);
            this.maxLbl.TabIndex = 34;
            this.maxLbl.Text = "max";
            this.maxLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // trkName
            // 
            this.trkName.Location = new System.Drawing.Point(-2, 9);
            this.trkName.Name = "trkName";
            this.trkName.Size = new System.Drawing.Size(88, 13);
            this.trkName.TabIndex = 31;
            this.trkName.Text = "Name";
            // 
            // minLbl
            // 
            this.minLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.minLbl.Location = new System.Drawing.Point(42, 26);
            this.minLbl.Name = "minLbl";
            this.minLbl.Size = new System.Drawing.Size(98, 9);
            this.minLbl.TabIndex = 33;
            this.minLbl.Text = "min";
            this.minLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // setBtn
            // 
            this.setBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.setBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setBtn.Location = new System.Drawing.Point(297, 20);
            this.setBtn.Name = "setBtn";
            this.setBtn.Size = new System.Drawing.Size(29, 15);
            this.setBtn.TabIndex = 35;
            this.setBtn.Text = "Set";
            this.setBtn.UseVisualStyleBackColor = true;
            this.setBtn.Click += new System.EventHandler(this.setBtn_Click);
            // 
            // tooltipPic
            // 
            this.tooltipPic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tooltipPic.Image = ((System.Drawing.Image)(resources.GetObject("tooltipPic.Image")));
            this.tooltipPic.InitialImage = ((System.Drawing.Image)(resources.GetObject("tooltipPic.InitialImage")));
            this.tooltipPic.Location = new System.Drawing.Point(235, 32);
            this.tooltipPic.Name = "tooltipPic";
            this.tooltipPic.Size = new System.Drawing.Size(17, 18);
            this.tooltipPic.TabIndex = 37;
            this.tooltipPic.TabStop = false;
            this.tooltipPic.Visible = false;
            // 
            // toolTip2
            // 
            this.toolTip2.AutomaticDelay = 6000;
            this.toolTip2.AutoPopDelay = 30000;
            this.toolTip2.InitialDelay = 50;
            this.toolTip2.ReshowDelay = 50;
            // 
            // firmwareValueLbl
            // 
            this.firmwareValueLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.firmwareValueLbl.AutoSize = true;
            this.firmwareValueLbl.ForeColor = System.Drawing.Color.Red;
            this.firmwareValueLbl.Location = new System.Drawing.Point(253, 34);
            this.firmwareValueLbl.Name = "firmwareValueLbl";
            this.firmwareValueLbl.Size = new System.Drawing.Size(48, 13);
            this.firmwareValueLbl.TabIndex = 38;
            this.firmwareValueLbl.Text = "trueData";
            this.firmwareValueLbl.Visible = false;
            // 
            // trkTxt
            // 
            this.trkTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trkTxt.Location = new System.Drawing.Point(269, 6);
            this.trkTxt.Name = "trkTxt";
            this.trkTxt.Size = new System.Drawing.Size(57, 20);
            this.trkTxt.TabIndex = 39;
            this.trkTxt.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trkTxt_MouseClick);
            this.trkTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trkTxt_KeyDown);
            // 
            // toolTip3
            // 
            this.toolTip3.AutoPopDelay = 5000;
            this.toolTip3.InitialDelay = 100;
            this.toolTip3.ReshowDelay = 100;
            // 
            // CustomScroll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.setBtn);
            this.Controls.Add(this.trkTxt);
            this.Controls.Add(this.firmwareValueLbl);
            this.Controls.Add(this.tooltipPic);
            this.Controls.Add(this.trkName);
            this.Controls.Add(this.maxLbl);
            this.Controls.Add(this.minLbl);
            this.Controls.Add(this.slider);
            this.Name = "CustomScroll";
            this.Size = new System.Drawing.Size(332, 53);
            ((System.ComponentModel.ISupportInitialize)(this.slider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tooltipPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TrackBar slider;
        private System.Windows.Forms.Label maxLbl;
        private System.Windows.Forms.Label trkName;
        private System.Windows.Forms.Label minLbl;
        private System.Windows.Forms.Button setBtn;
        private System.Windows.Forms.PictureBox tooltipPic;
        private System.Windows.Forms.ToolTip toolTip2;
        private System.Windows.Forms.Label firmwareValueLbl;
        private System.Windows.Forms.TextBox trkTxt;
        private System.Windows.Forms.ToolTip toolTip3;
    }
}
