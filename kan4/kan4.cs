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

        private void button1_Click(object sender, EventArgs e)
        {
            KanpouUtil.downloadKanpou(db);
            /*
            listBox1.Items.Clear();
            db.open();
            var l = db.searchHeadline("");
            foreach (var item in l)
            {
                listBox1.Items.Add(string.Format("【{0,-10}】　{1}",item["pdf_title"], item["headline"]));
            }
            db.close();
            */

        }
    }
}
