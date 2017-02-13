using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kan4
{
    public class Kanpou
    {
        public readonly string title;
        public readonly string date;
        public readonly string id;

        private static string pat = "<P>(.+)　………　<A HREF=\"\\./20\\d{6}[a-z]\\d{9}f\\.html\" TARGET=\"_top\">(.+)</A></P>";
//        private static string secPat = "<P><FONT SIZE=\"+1\"><B>(〔.+〕)</B></FONT></P>";

        public class HeadLine
        {
            public readonly string text;
            public readonly int page;
            public HeadLine(string t,int p)
            {
                text = t;
                page = p;
            }
        }

        public Kanpou(string t,string d,string i)
        {
            title = t;
            date = d;
            id = i;
        }


        /// <summary>
        /// 官報pdfのURL一覧を返す
        /// </summary>
        /// <returns></returns>
        public List<string> getPdfUrls()
        {
            var list = new List<string>();
            var wc = new System.Net.WebClient();
            try
            {
                var lines = wc.DownloadString(KanpouUtil.MainUrl + String.Format("{0}/{1}/button/{1}0000b.html", date, id)).Split('\n');
                System.Text.RegularExpressions.Match m;
                foreach (var item in lines)
                {
                    m = System.Text.RegularExpressions.Regex.Match(item, "<A HREF=.+/" + id + "(\\d{4})f.html.+><B>前ページ</B></FONT></A>");
                    if (m.Success)
                    {
                        int p = int.Parse(m.Groups[1].Value);
                        for (int i = 0; i < p; i++)
                        {
                            string u = String.Format("{0}{1}/{2}/pdf/{2}{3}.pdf", KanpouUtil.MainUrl, date, id, (i + 1).ToString("D4"));
                            list.Add(u);
                        }
                        break;
                    }
                }
            }
            catch (System.Net.WebException)
            {
                list.Clear();
            }

            return list;
        }


        /// <summary>
        /// 目次を返す
        /// </summary>
        /// <returns></returns>
        public List<HeadLine> getHeadLines()
        {
            var list = new List<HeadLine>();
            var wc = new System.Net.WebClient();
            try
            {
                var lines = wc.DownloadString(KanpouUtil.MainUrl + String.Format("{0}/{1}/{1}0000.html", date, id)).Split('\n');
                var sec = string.Empty;
                foreach (var l in lines)
                {
                    var m = System.Text.RegularExpressions.Regex.Match(l,pat);
                    if (m.Success)
                    {
                        //目次にマッチ
                        int p = KanpouUtil.toInt(m.Groups[2].Value);
                        string h = System.Text.RegularExpressions.Regex.Replace(m.Groups[1].Value,"<.+?>", string.Empty);
                        if (h.StartsWith("〔"))
                        {
                            sec = string.Empty;
                        }
                        list.Add(new HeadLine(sec + h, p));
                    }
                    else
                    {
                        m = System.Text.RegularExpressions.Regex.Match(l, ">(〔.+〕)</B></FONT></P>");
                        if (m.Success)
                        {
                            //小題にマッチ
                            sec = m.Groups[1].Value;
                        }
                    }
                }
            }
            catch (System.Net.WebException)
            {
                list.Clear();
            }
            return list;
        }
    }
}
