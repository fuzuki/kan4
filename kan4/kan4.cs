using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kan4
{
    public partial class kan4 : Form
    {
        private Kan4DB db;

        public kan4()
        {
            InitializeComponent();
            db = new Kan4DB();
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            downloadButton.Enabled = false;
//            toolStripDownloadStatusLabel.Text = "download";
            backgroundDownloadWorker.RunWorkerAsync();

            

            /*
            listBox1.Items.Clear();
            db.open();
            var l = db.searchHeadline("");
            foreach (var item in l)
            {
                listBox1.Items.Add(string.Format("【{0,-16}】　{1} ... {2}",item["pdf_title"], item["headline"],item["page"]));
            }
            db.close();
            */
        }

        /// <summary>
        /// バックグラウンドでpdfダウンロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundDownloadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //TODO 要修正
            KanpouUtil.downloadKanpou(db);

            for (int i = 0; i <= 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                backgroundDownloadWorker.ReportProgress(i*10);
            }
        }

        private void backgroundDownloadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripDownloadProgressBar.Value = e.ProgressPercentage;
            toolStripDownloadStatusLabel.Text = e.ProgressPercentage.ToString();
        }

        private void backgroundDownloadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripDownloadStatusLabel.Text = "ok";
            downloadButton.Enabled = true;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            toolStripDownloadStatusLabel.Text = "search:" + searchTextBox.Text;
            listBox1.Items.Clear();
            db.open();
            var l = db.searchHeadline(searchTextBox.Text.Trim());
            db.close();
            foreach (var item in l)
            {
                listBox1.Items.Add(string.Format("【{0,-16}】　{1} ... {2}", item["pdf_title"], item["headline"], item["page"]));
            }
        }
    }
}
