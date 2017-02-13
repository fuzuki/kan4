using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kan4
{
    public class Kan4DB
    {
        private System.Data.SQLite.SQLiteConnection db;

        public Kan4DB()
        {
            string mydir = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            string mydbpath = string.Format("{0}\\kan4.db", mydir);
            db = new System.Data.SQLite.SQLiteConnection(string.Format("Data Source={0}", mydbpath));
            if (!System.IO.File.Exists(mydbpath))
            {
                createDB();
            }

        }

        public void open()
        {
            db.Open();
        }

        public void close()
        {
            db.Close();
        }

        private void createDB()
        {
            db.Open();
            var com = db.CreateCommand();
            com.CommandText = "CREATE TABLE pdf (id text primary key, title text)";
            com.ExecuteNonQuery();
            com.CommandText = "CREATE TABLE contents (id integer primary key AUTOINCREMENT, headline text,page integer,pdf_id text not null,FOREIGN KEY(pdf_id) REFERENCES pdf(id))";
            com.ExecuteNonQuery();
            db.Close();
        }

        /// <summary>
        /// 対象がDB登録済みかどうか確認
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public bool isRegisted(Kanpou k)
        {
            var com = db.CreateCommand();
            com.CommandText = "select id from pdf where id=@1";
            com.Parameters.AddWithValue("@1",k.id);
//            com.ExecuteReader();
            var r = com.ExecuteReader();
            
            return (r.FieldCount > 0);
        }

        /// <summary>
        /// データベース登録
        /// </summary>
        /// <param name="k"></param>
        public void regist(Kanpou k)
        {
            var com = db.CreateCommand();

            com.CommandText = "insert into pdf(id,title) values(@1,@2);";
            com.Parameters.AddWithValue("@1", k.id);
            com.Parameters.AddWithValue("@2", k.title);
            com.ExecuteNonQuery();

            var headlines = k.getHeadLines();
            com.CommandText = "insert into contents(headline,page,pdf_id) values(@1,@2,@3)";
            foreach (var h in headlines)
            {
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@1",h.text);
                com.Parameters.AddWithValue("@2", h.page);
                com.Parameters.AddWithValue("@3", k.id);
                com.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 目次検索
        /// </summary>
        /// <param name="txt">検索文字列</param>
        /// <param name="from">検索対象期間指定:yyyyMM</param>
        /// <param name="to">検索対象期間指定:yyyyMM</param>
        /// <returns></returns>
        public List<Dictionary<string, string>> searchHeadline(string txt,string from="200101",string to="299912")
        {
            var list = new List<Dictionary<string, string>>();
            string sql = "select contents.headline,contents.page,pdf.id,pdf.title from contents inner join pdf contents.pdf_id = pdf.id where contents.headline like @1 and pdf.id > @2 and pdf.id < @3;";
            // select * from contents inner join pdf contents.pdf_id = pdf.id;
            var com = db.CreateCommand();
            com.CommandText = sql;
            com.Parameters.AddWithValue("@1", string.Format("%{0}%",txt));
            com.Parameters.AddWithValue("@2", string.Format("{0}00", from));
            com.Parameters.AddWithValue("@3", string.Format("{0}99", to));
            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var r = new Dictionary<string, string>();
                r.Add("headline", reader.GetString(0));
                r.Add("page",string.Format("{0}", reader.GetInt32(1)));
                r.Add("pdf_id", reader.GetString(2));
                r.Add("pdf_title", reader.GetString(3));
                list.Add(r);
            }
            return list;
        }
    }
}
