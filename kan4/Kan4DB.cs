using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kan4
{
    class Kan4DB
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
            com.CommandText = "CREATE TABLE pdf (id text, title text)";
            com.ExecuteNonQuery();
            com.CommandText = "CREATE TABLE contents (id integer primary key AUTOINCREMENT, headline text,page integer,pdf_id text not null,FOREIGN KEY(pdf_id) REFERENCES pdf(id))";
            com.ExecuteNonQuery();
            db.Close();
        }

    }
}
