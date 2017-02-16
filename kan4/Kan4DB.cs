using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kan4
{
    public class Kan4DB
    {
        /// <summary>
        /// title:紙名、headline:目次、id、page 
        /// </summary>
        public enum KanpouInfo { id, title, headline, page};
        private System.Data.SQLite.SQLiteConnection db;
        private bool closed;
        private int limit = 10000;

        public Kan4DB()
        {
            string mydir = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            string mydbpath = string.Format("{0}\\kan4.db", mydir);
            db = new System.Data.SQLite.SQLiteConnection(string.Format("Data Source={0}", mydbpath));
            if (!System.IO.File.Exists(mydbpath))
            {
                createDB();
            }
            closed = true;
        }

        public void open()
        {
            if (closed)
            {
                db.Open();
                closed = false;
            }
        }

        public void close()
        {
            if (!closed)
            {
                db.Close();
                closed = true;
            }
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
            return isRegisted(k.id);
        }
        /// <summary>
        /// 対象がDB登録済みかどうか確認
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool isRegisted(string id)
        {
            var com = db.CreateCommand();
            com.CommandText = "select id from pdf where id = @1";
            com.Parameters.AddWithValue("@1", id);
            var r = com.ExecuteReader();
            return r.Read();

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
        /// 今月の官報一覧を取得する
        /// </summary>
        /// <returns></returns>
        public List<Dictionary<KanpouInfo, string>> searchKanpou()
        {
            var from = DateTime.Now.ToString("yyyyMM00");
            var to = DateTime.Now.ToString("yyyyMM99");
            return searchKanpou(from,to);
        }
        /// <summary>
        /// 指定した期間の官報一覧を取得する
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>官報情報のリスト</returns>
        public List<Dictionary<KanpouInfo, string>> searchKanpou(string from,string to)
        {
            var list = new List<Dictionary<KanpouInfo, string>>();
            string sql = string.Format("select id,title from pdf where id > @1 and id < @2 order by id limit {0}",limit);
            var com = db.CreateCommand();
            com.CommandText = sql;
            com.Parameters.AddWithValue("@1", string.Format("{0}00", from));
            com.Parameters.AddWithValue("@2", string.Format("{0}zz", to));
            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var r = new Dictionary<KanpouInfo, string>();
                r.Add(KanpouInfo.id, reader.GetString(0));
                r.Add(KanpouInfo.title, reader.GetString(1));
                list.Add(r);
            }

            return list;
        }

        /// <summary>
        /// 目次検索
        /// </summary>
        /// <param name="txt">検索文字列</param>
        /// <param name="from">検索対象期間指定:yyyyMM</param>
        /// <param name="to">検索対象期間指定:yyyyMM</param>
        /// <returns></returns>
        public List<Dictionary<KanpouInfo, string>> searchHeadline(string txt,string from="20010101",string to="29991231")
        {
            var list = new List<Dictionary<KanpouInfo, string>>();
            string sql = string.Format("select contents.headline,contents.page,pdf.id,pdf.title from contents inner join pdf on contents.pdf_id = pdf.id where (contents.headline like @1 or pdf.title like @1) and pdf.id > @2 and pdf.id < @3 order by pdf.id limit {0}",limit);
            var com = db.CreateCommand();
            com.CommandText = sql;
            com.Parameters.AddWithValue("@1", string.Format("%{0}%",txt));
            com.Parameters.AddWithValue("@2", string.Format("{0}00", from));
            com.Parameters.AddWithValue("@3", string.Format("{0}zz", to));
            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var r = new Dictionary<KanpouInfo, string>();
                r.Add(KanpouInfo.headline, reader.GetString(0));
                r.Add(KanpouInfo.page,string.Format("{0}", reader.GetInt32(1)));
                r.Add(KanpouInfo.id, reader.GetString(2));
                r.Add(KanpouInfo.title, reader.GetString(3));
                list.Add(r);
            }
            return list;
        }
    }
}
