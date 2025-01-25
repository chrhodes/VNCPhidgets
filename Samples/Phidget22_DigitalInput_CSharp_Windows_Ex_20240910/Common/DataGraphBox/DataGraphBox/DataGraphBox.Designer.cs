namespace Phidget22.ExampleUtils
{
	partial class DataGraphBox
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataGraphBox));
			this.graph = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.ChartTimeTrackbar = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			this.maxTimeLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ChartTimeLabel = new System.Windows.Forms.Label();
			this.ClearButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.PauseButton = new System.Windows.Forms.Button();
			this.FileSizeLabel = new System.Windows.Forms.Label();
			this.FileNameLabel = new System.Windows.Forms.Label();
			this.chartTypeBox = new System.Windows.Forms.ComboBox();
			this.graphTypeLabel = new System.Windows.Forms.Label();
			this.filterBar = new System.Windows.Forms.TrackBar();
			this.filterSectionLabel = new System.Windows.Forms.Label();
			this.filterLabel = new System.Windows.Forms.Label();
			this.filterTypeBox = new System.Windows.Forms.ComboBox();
			this.minSamplesLabel = new System.Windows.Forms.Label();
			this.maxSamplesLabel = new System.Windows.Forms.Label();
			this.filterTypeLabel = new System.Windows.Forms.Label();
			this.filterWarningLabel = new System.Windows.Forms.Label();
			this.updateGuiTimer = new System.Windows.Forms.Timer(this.components);
			this.filterPanel = new System.Windows.Forms.Panel();
			this.pointsBox = new System.Windows.Forms.CheckBox();
			this.dataRateLbl = new System.Windows.Forms.Label();
			this.badIntervalText = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.graph)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ChartTimeTrackbar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.filterBar)).BeginInit();
			this.filterPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// graph
			// 
			this.graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			chartArea1.AxisX.Interval = 5D;
			chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Seconds;
			chartArea1.AxisX.LabelStyle.Format = "hh:mm:ss tt";
			chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.DarkGray;
			chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea1.AxisY.IsStartedFromZero = false;
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.DarkGray;
			chartArea1.AxisY.MinorGrid.LineColor = System.Drawing.Color.Silver;
			chartArea1.BackColor = System.Drawing.Color.White;
			chartArea1.InnerPlotPosition.Auto = false;
			chartArea1.InnerPlotPosition.Height = 85.21063F;
			chartArea1.InnerPlotPosition.Width = 93F;
			chartArea1.InnerPlotPosition.X = 5F;
			chartArea1.InnerPlotPosition.Y = 3.63602F;
			chartArea1.Name = "ChartArea1";
			this.graph.ChartAreas.Add(chartArea1);
			this.graph.Location = new System.Drawing.Point(12, 12);
			this.graph.Name = "graph";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
			series1.Color = System.Drawing.Color.Blue;
			series1.Name = "Series1";
			series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
			this.graph.Series.Add(series1);
			this.graph.Size = new System.Drawing.Size(709, 269);
			this.graph.SuppressExceptions = true;
			this.graph.TabIndex = 0;
			this.graph.Text = "chart1";
			title1.Name = "Title1";
			title1.Text = "Title";
			this.graph.Titles.Add(title1);
			this.graph.FormatNumber += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.FormatNumberEventArgs>(this.graph_FormatNumber);
			// 
			// ChartTimeTrackbar
			// 
			this.ChartTimeTrackbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ChartTimeTrackbar.LargeChange = 10;
			this.ChartTimeTrackbar.Location = new System.Drawing.Point(72, 334);
			this.ChartTimeTrackbar.Maximum = 60;
			this.ChartTimeTrackbar.Minimum = 1;
			this.ChartTimeTrackbar.Name = "ChartTimeTrackbar";
			this.ChartTimeTrackbar.Size = new System.Drawing.Size(221, 45);
			this.ChartTimeTrackbar.TabIndex = 1;
			this.ChartTimeTrackbar.TickFrequency = 5;
			this.ChartTimeTrackbar.Value = 1;
			this.ChartTimeTrackbar.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 334);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 26);
			this.label1.TabIndex = 2;
			this.label1.Text = "Chart\r\nLength";
			// 
			// maxTimeLabel
			// 
			this.maxTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.maxTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.maxTimeLabel.Location = new System.Drawing.Point(206, 362);
			this.maxTimeLabel.Name = "maxTimeLabel";
			this.maxTimeLabel.Size = new System.Drawing.Size(87, 13);
			this.maxTimeLabel.TabIndex = 3;
			this.maxTimeLabel.Text = "60s";
			this.maxTimeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(79, 362);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(49, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "00:00:01";
			// 
			// ChartTimeLabel
			// 
			this.ChartTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ChartTimeLabel.AutoSize = true;
			this.ChartTimeLabel.Location = new System.Drawing.Point(289, 339);
			this.ChartTimeLabel.Name = "ChartTimeLabel";
			this.ChartTimeLabel.Size = new System.Drawing.Size(24, 13);
			this.ChartTimeLabel.TabIndex = 5;
			this.ChartTimeLabel.Text = "??s";
			// 
			// ClearButton
			// 
			this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ClearButton.Location = new System.Drawing.Point(12, 386);
			this.ClearButton.Name = "ClearButton";
			this.ClearButton.Size = new System.Drawing.Size(75, 23);
			this.ClearButton.TabIndex = 6;
			this.ClearButton.Text = "Clear Graph";
			this.ClearButton.UseVisualStyleBackColor = true;
			this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
			// 
			// SaveButton
			// 
			this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.SaveButton.Location = new System.Drawing.Point(188, 386);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(89, 23);
			this.SaveButton.TabIndex = 7;
			this.SaveButton.Text = "Save to CSV";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// PauseButton
			// 
			this.PauseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.PauseButton.Location = new System.Drawing.Point(93, 386);
			this.PauseButton.Name = "PauseButton";
			this.PauseButton.Size = new System.Drawing.Size(89, 23);
			this.PauseButton.TabIndex = 8;
			this.PauseButton.Text = "Pause";
			this.PauseButton.UseVisualStyleBackColor = true;
			this.PauseButton.Click += new System.EventHandler(this.button1_Click);
			// 
			// FileSizeLabel
			// 
			this.FileSizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.FileSizeLabel.AutoSize = true;
			this.FileSizeLabel.Location = new System.Drawing.Point(617, 391);
			this.FileSizeLabel.Name = "FileSizeLabel";
			this.FileSizeLabel.Size = new System.Drawing.Size(49, 13);
			this.FileSizeLabel.TabIndex = 9;
			this.FileSizeLabel.Text = "File Size:";
			// 
			// FileNameLabel
			// 
			this.FileNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FileNameLabel.Location = new System.Drawing.Point(283, 391);
			this.FileNameLabel.Name = "FileNameLabel";
			this.FileNameLabel.Size = new System.Drawing.Size(328, 13);
			this.FileNameLabel.TabIndex = 10;
			this.FileNameLabel.Text = "File:";
			// 
			// chartTypeBox
			// 
			this.chartTypeBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chartTypeBox.DisplayMember = "Value";
			this.chartTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.chartTypeBox.FormattingEnabled = true;
			this.chartTypeBox.Location = new System.Drawing.Point(77, 302);
			this.chartTypeBox.Name = "chartTypeBox";
			this.chartTypeBox.Size = new System.Drawing.Size(137, 21);
			this.chartTypeBox.TabIndex = 12;
			this.chartTypeBox.ValueMember = "Key";
			this.chartTypeBox.Visible = false;
			this.chartTypeBox.SelectedIndexChanged += new System.EventHandler(this.chartTypeBox_SelectedIndexChanged);
			// 
			// graphTypeLabel
			// 
			this.graphTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.graphTypeLabel.AutoSize = true;
			this.graphTypeLabel.Location = new System.Drawing.Point(12, 305);
			this.graphTypeLabel.Name = "graphTypeLabel";
			this.graphTypeLabel.Size = new System.Drawing.Size(59, 13);
			this.graphTypeLabel.TabIndex = 13;
			this.graphTypeLabel.Text = "Chart Type";
			this.graphTypeLabel.Visible = false;
			// 
			// filterBar
			// 
			this.filterBar.LargeChange = 1;
			this.filterBar.Location = new System.Drawing.Point(73, 32);
			this.filterBar.Maximum = 1000;
			this.filterBar.Minimum = 2;
			this.filterBar.Name = "filterBar";
			this.filterBar.Size = new System.Drawing.Size(224, 45);
			this.filterBar.TabIndex = 14;
			this.filterBar.Value = 2;
			this.filterBar.Scroll += new System.EventHandler(this.filterBar_Scroll);
			// 
			// filterSectionLabel
			// 
			this.filterSectionLabel.AutoSize = true;
			this.filterSectionLabel.Location = new System.Drawing.Point(0, 32);
			this.filterSectionLabel.Name = "filterSectionLabel";
			this.filterSectionLabel.Size = new System.Drawing.Size(72, 26);
			this.filterSectionLabel.TabIndex = 15;
			this.filterSectionLabel.Text = "Moving Avg.\r\nFilter Samples";
			// 
			// filterLabel
			// 
			this.filterLabel.AutoSize = true;
			this.filterLabel.Location = new System.Drawing.Point(294, 39);
			this.filterLabel.Name = "filterLabel";
			this.filterLabel.Size = new System.Drawing.Size(19, 13);
			this.filterLabel.TabIndex = 16;
			this.filterLabel.Text = "??";
			// 
			// filterTypeBox
			// 
			this.filterTypeBox.DisplayMember = "Value";
			this.filterTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.filterTypeBox.FormattingEnabled = true;
			this.filterTypeBox.Location = new System.Drawing.Point(78, 0);
			this.filterTypeBox.Name = "filterTypeBox";
			this.filterTypeBox.Size = new System.Drawing.Size(87, 21);
			this.filterTypeBox.TabIndex = 17;
			this.filterTypeBox.ValueMember = "Key";
			this.filterTypeBox.SelectedIndexChanged += new System.EventHandler(this.filterTypeBox_SelectedIndexChanged);
			// 
			// minSamplesLabel
			// 
			this.minSamplesLabel.AutoSize = true;
			this.minSamplesLabel.Location = new System.Drawing.Point(80, 64);
			this.minSamplesLabel.Name = "minSamplesLabel";
			this.minSamplesLabel.Size = new System.Drawing.Size(13, 13);
			this.minSamplesLabel.TabIndex = 19;
			this.minSamplesLabel.Text = "1";
			// 
			// maxSamplesLabel
			// 
			this.maxSamplesLabel.AutoSize = true;
			this.maxSamplesLabel.Location = new System.Drawing.Point(266, 64);
			this.maxSamplesLabel.Name = "maxSamplesLabel";
			this.maxSamplesLabel.Size = new System.Drawing.Size(31, 13);
			this.maxSamplesLabel.TabIndex = 18;
			this.maxSamplesLabel.Text = "1000";
			// 
			// filterTypeLabel
			// 
			this.filterTypeLabel.AutoSize = true;
			this.filterTypeLabel.Location = new System.Drawing.Point(0, 3);
			this.filterTypeLabel.Name = "filterTypeLabel";
			this.filterTypeLabel.Size = new System.Drawing.Size(56, 13);
			this.filterTypeLabel.TabIndex = 20;
			this.filterTypeLabel.Text = "Filter Type";
			// 
			// filterWarningLabel
			// 
			this.filterWarningLabel.AutoSize = true;
			this.filterWarningLabel.Location = new System.Drawing.Point(171, 3);
			this.filterWarningLabel.Name = "filterWarningLabel";
			this.filterWarningLabel.Size = new System.Drawing.Size(69, 13);
			this.filterWarningLabel.TabIndex = 21;
			this.filterWarningLabel.Text = "FilterWarning";
			// 
			// updateGuiTimer
			// 
			this.updateGuiTimer.Interval = 50;
			this.updateGuiTimer.Tick += new System.EventHandler(this.updateGuiTimer_Tick);
			// 
			// filterPanel
			// 
			this.filterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.filterPanel.Controls.Add(this.pointsBox);
			this.filterPanel.Controls.Add(this.filterLabel);
			this.filterPanel.Controls.Add(this.minSamplesLabel);
			this.filterPanel.Controls.Add(this.maxSamplesLabel);
			this.filterPanel.Controls.Add(this.filterTypeLabel);
			this.filterPanel.Controls.Add(this.filterBar);
			this.filterPanel.Controls.Add(this.filterWarningLabel);
			this.filterPanel.Controls.Add(this.filterSectionLabel);
			this.filterPanel.Controls.Add(this.filterTypeBox);
			this.filterPanel.Location = new System.Drawing.Point(353, 302);
			this.filterPanel.Name = "filterPanel";
			this.filterPanel.Size = new System.Drawing.Size(368, 89);
			this.filterPanel.TabIndex = 22;
			this.filterPanel.Visible = false;
			// 
			// pointsBox
			// 
			this.pointsBox.AutoSize = true;
			this.pointsBox.Location = new System.Drawing.Point(246, 4);
			this.pointsBox.Name = "pointsBox";
			this.pointsBox.Size = new System.Drawing.Size(85, 17);
			this.pointsBox.TabIndex = 22;
			this.pointsBox.Text = "Show Points";
			this.pointsBox.UseVisualStyleBackColor = true;
			this.pointsBox.CheckedChanged += new System.EventHandler(this.pointsBox_CheckedChanged);
			// 
			// dataRateLbl
			// 
			this.dataRateLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataRateLbl.BackColor = System.Drawing.Color.White;
			this.dataRateLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dataRateLbl.Location = new System.Drawing.Point(12, 281);
			this.dataRateLbl.Name = "dataRateLbl";
			this.dataRateLbl.Size = new System.Drawing.Size(709, 15);
			this.dataRateLbl.TabIndex = 23;
			this.dataRateLbl.Text = "Data Rate";
			this.dataRateLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// badIntervalText
			// 
			this.badIntervalText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.badIntervalText.AutoSize = true;
			this.badIntervalText.BackColor = System.Drawing.Color.White;
			this.badIntervalText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
			this.badIntervalText.ForeColor = System.Drawing.Color.Red;
			this.badIntervalText.Location = new System.Drawing.Point(12, 280);
			this.badIntervalText.Name = "badIntervalText";
			this.badIntervalText.Size = new System.Drawing.Size(284, 13);
			this.badIntervalText.TabIndex = 24;
			this.badIntervalText.Text = "Inconsistent Interval Between Data. Clear Graph to refresh.";
			this.badIntervalText.Visible = false;
			// 
			// DataGraphBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(733, 421);
			this.Controls.Add(this.badIntervalText);
			this.Controls.Add(this.dataRateLbl);
			this.Controls.Add(this.ChartTimeLabel);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.maxTimeLabel);
			this.Controls.Add(this.filterPanel);
			this.Controls.Add(this.FileSizeLabel);
			this.Controls.Add(this.graphTypeLabel);
			this.Controls.Add(this.chartTypeBox);
			this.Controls.Add(this.FileNameLabel);
			this.Controls.Add(this.PauseButton);
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(this.ClearButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ChartTimeTrackbar);
			this.Controls.Add(this.graph);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(706, 460);
			this.Name = "DataGraphBox";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataGraphBox_FormClosing);
			this.Load += new System.EventHandler(this.DataGraphBox_Load);
			((System.ComponentModel.ISupportInitialize)(this.graph)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ChartTimeTrackbar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.filterBar)).EndInit();
			this.filterPanel.ResumeLayout(false);
			this.filterPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataVisualization.Charting.Chart graph;
		private System.Windows.Forms.TrackBar ChartTimeTrackbar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label maxTimeLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label ChartTimeLabel;
		private System.Windows.Forms.Button ClearButton;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button PauseButton;
		private System.Windows.Forms.Label FileSizeLabel;
		private System.Windows.Forms.Label FileNameLabel;
		private System.Windows.Forms.ComboBox chartTypeBox;
		private System.Windows.Forms.Label graphTypeLabel;
		private System.Windows.Forms.TrackBar filterBar;
		private System.Windows.Forms.Label filterSectionLabel;
		private System.Windows.Forms.Label filterLabel;
		private System.Windows.Forms.ComboBox filterTypeBox;
		private System.Windows.Forms.Label minSamplesLabel;
		private System.Windows.Forms.Label maxSamplesLabel;
		private System.Windows.Forms.Label filterTypeLabel;
		private System.Windows.Forms.Label filterWarningLabel;
		private System.Windows.Forms.Timer updateGuiTimer;
		private System.Windows.Forms.Panel filterPanel;
		private System.Windows.Forms.Label dataRateLbl;
		private System.Windows.Forms.CheckBox pointsBox;
		private System.Windows.Forms.Label badIntervalText;
	}
}

