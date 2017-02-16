﻿using System;
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

        public kan4()
        {
            InitializeComponent();
            db = new Kan4DB();
            downloading = false;
            cancel = false;
            dateTimePickerFrom.Value = DateTime.Parse(string.Format("{0}/{1}/1",DateTime.Today.Year,DateTime.Today.Month)) ;
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            if (!downloading)
            {
                downloadButton.Text = "Cancel";
                downloading = true;
                toolStripDownloadStatusLabel.Text = "downloading...";
                backgroundDownloadWorker.RunWorkerAsync();
            }else
            {
                toolStripDownloadStatusLabel.Text = "canceling...";
                downloadButton.Enabled = false;
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
    }
}
