namespace kan4
{
    partial class kan4
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.downloadButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.downloadStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripDownloadProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripDownloadStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundDownloadWorker = new System.ComponentModel.BackgroundWorker();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.downloadStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(12, 12);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(75, 23);
            this.downloadButton.TabIndex = 0;
            this.downloadButton.Text = "Download";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.HorizontalScrollbar = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(12, 41);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(760, 148);
            this.listBox1.TabIndex = 5;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // downloadStatusStrip
            // 
            this.downloadStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDownloadProgressBar,
            this.toolStripDownloadStatusLabel});
            this.downloadStatusStrip.Location = new System.Drawing.Point(0, 195);
            this.downloadStatusStrip.Name = "downloadStatusStrip";
            this.downloadStatusStrip.Size = new System.Drawing.Size(784, 22);
            this.downloadStatusStrip.TabIndex = 2;
            this.downloadStatusStrip.Text = "statusStrip1";
            // 
            // toolStripDownloadProgressBar
            // 
            this.toolStripDownloadProgressBar.Name = "toolStripDownloadProgressBar";
            this.toolStripDownloadProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripDownloadStatusLabel
            // 
            this.toolStripDownloadStatusLabel.Name = "toolStripDownloadStatusLabel";
            this.toolStripDownloadStatusLabel.Size = new System.Drawing.Size(17, 17);
            this.toolStripDownloadStatusLabel.Text = "--";
            // 
            // backgroundDownloadWorker
            // 
            this.backgroundDownloadWorker.WorkerReportsProgress = true;
            this.backgroundDownloadWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundDownloadWorker_DoWork);
            this.backgroundDownloadWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundDownloadWorker_ProgressChanged);
            this.backgroundDownloadWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundDownloadWorker_RunWorkerCompleted);
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerFrom.CustomFormat = "yyyy年M月d日から";
            this.dateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerFrom.Location = new System.Drawing.Point(379, 14);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(147, 19);
            this.dateTimePickerFrom.TabIndex = 2;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTo.Checked = false;
            this.dateTimePickerTo.CustomFormat = "yyyy年M月d日まで";
            this.dateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTo.Location = new System.Drawing.Point(532, 14);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(142, 19);
            this.dateTimePickerTo.TabIndex = 3;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(93, 14);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(280, 19);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchTextBox_KeyPress);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(685, 11);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(87, 22);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // kan4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 217);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.downloadStatusStrip);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.downloadButton);
            this.MinimumSize = new System.Drawing.Size(800, 150);
            this.Name = "kan4";
            this.Text = "簡単官報管理官。";
            this.downloadStatusStrip.ResumeLayout(false);
            this.downloadStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.StatusStrip downloadStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar toolStripDownloadProgressBar;
        private System.ComponentModel.BackgroundWorker backgroundDownloadWorker;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDownloadStatusLabel;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button searchButton;
    }
}

