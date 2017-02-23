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
        private class ListItem
        {
            public readonly string txt;
            public readonly string id;
            public ListItem(string t,string i)
            {
                txt = t;
                id = i;
            }

            public override string ToString()
            {
                return txt;
            }
        }

        private Kan4DB db;

        private bool downloading;
        private bool cancel;
        private string confPath;

        /// <summary>
        /// kan4コンストラクタ
        /// </summary>
        public kan4()
        {
            InitializeComponent();
            db = new Kan4DB();
            downloading = false;
            cancel = false;
//            dateTimePickerFrom.Value = DateTime.Parse(string.Format("{0}/{1}/1",DateTime.Today.Year,DateTime.Today.Month)) ;
            dateTimePickerFrom.Value = DateTime.Parse("2017/1/1");

            string mydir = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            confPath = string.Format("{0}\\kan4.conf", mydir);
            loadConf();

        }

        /// <summary>
        /// ダウンロード・キャンセルボタンのクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (!downloading)
            {
                downloadStatusStrip.Enabled = false;
                cancelToolStripMenuItem.Enabled = true;
                downloadButton.Text = "Cancel";
                downloading = true;
                toolStripDownloadStatusLabel.Text = "downloading...";
                backgroundDownloadWorker.RunWorkerAsync();
            }else
            {
                toolStripDownloadStatusLabel.Text = "canceling...";
                downloadButton.Enabled = false;
                cancelToolStripMenuItem.Enabled = false;
                cancel = true;
            }
        }

        /// <summary>
        /// バックグラウンドでpdfダウンロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundDownloadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var klist = KanpouUtil.getKanpouList();
            if(klist.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Network Error !");
            }
            int i = 0;
            klist.Reverse();//古いものから保存
            foreach (var k in klist)
            {
                KanpouUtil.downloadKanpou(db, k);
                i++;
                backgroundDownloadWorker.ReportProgress((i * 100)/klist.Count);
                if (cancel)
                {
                    break;
                }
            }
        }

        private void backgroundDownloadWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripDownloadProgressBar.Value = e.ProgressPercentage;
        }

        private void backgroundDownloadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripDownloadProgressBar.Value = 0;
            toolStripDownloadStatusLabel.Text = "ok";

            downloading = false;
            cancel = false;
            downloadButton.Text = "Download";
            downloadButton.Enabled = true;
            downloadStatusStrip.Enabled = true;
            cancelToolStripMenuItem.Enabled = false;
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            toolStripDownloadStatusLabel.Text = "search";
            listBox1.Items.Clear();
            db.open();
            var from = dateTimePickerFrom.Value.ToString("yyyyMMdd");
            var to = dateTimePickerTo.Value.ToString("yyyyMMdd");
            var l = db.searchHeadline(searchTextBox.Text.Trim(),from,to);
            db.close();
            foreach (var item in l)
            {
                var date = string.Format("{0}/{1}/{2}", item[Kan4DB.KanpouInfo.id].Substring(0, 4), item[Kan4DB.KanpouInfo.id].Substring(4, 2), item[Kan4DB.KanpouInfo.id].Substring(6, 2));
                listBox1.Items.Add(new ListItem(string.Format("【{0} ({1})】　{2} ... {3}", item[Kan4DB.KanpouInfo.title], date, item[Kan4DB.KanpouInfo.headline], item[Kan4DB.KanpouInfo.page]), item[Kan4DB.KanpouInfo.id]));
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                return;
            }
            var item = (ListItem)listBox1.SelectedItem;
            KanpouUtil.openKanpouPdf(item.id);
            
        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                searchButton_Click(sender, e);
            }
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            downloadButton_Click(sender, e);
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = string.Format("簡単官報管理官。\nver: {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            MessageBox.Show(msg,"バージョン情報");
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fd = new FontDialog();
            fd.Font = listBox1.Font;

            fd.FontMustExist = true;
            fd.AllowVerticalFonts = false;
            fd.ShowColor = false;
            fd.ShowEffects = false;
            fd.MinSize = 9;
            if (fd.ShowDialog() != DialogResult.Cancel)
            {
                listBox1.Font = fd.Font;
                saveConf();
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            downloadButton_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kan4_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!confirmExit())
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 修了確認
        /// </summary>
        /// <returns>true:yes / false:no</returns>
        private bool confirmExit()
        {
            if (downloading)
            {
                var result = MessageBox.Show("官報をダウンロード中です。\n終了しますか？", "確認ダイアログ", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 設定ファイルの読み込み
        /// </summary>
        private void loadConf()
        {
            if (System.IO.File.Exists(confPath))
            {

            }
        }

        /// <summary>
        /// 設定ファイルの保存
        /// </summary>
        private void saveConf()
        {

        }

    }
}
