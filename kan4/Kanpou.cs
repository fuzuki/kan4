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
        private List<HeadLine> headlines;
        private string[] lines;

//        private static string titpat = "<(h\\d) class=\"title\">";
        private static string titpat = "<(h\\d)";
        private static string spanpat = "<span class=\"(.+)\">(.+)</span>";

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

        public Kanpou(string t,string i)
        {
            title = t;
            date = i.Substring(0,8);
            id = i;
            headlines = new List<HeadLine>();
        }


        /// <summary>
        /// 官報pdfのURL一覧を返す
        /// </summary>
        /// <returns></returns>
        public List<string> getPdfUrls()
        {
            var list = new List<string>();
            var wc = new System.Net.WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            try
            {
                if(lines == null || lines.Length == 0)
                {
                    lines = wc.DownloadString(KanpouUtil.MainUrl + String.Format("{0}/{1}/{1}0000f.html", date, id)).Split('\n');
                }
                System.Text.RegularExpressions.Match m;
                foreach (var item in lines)
                {
                    m = System.Text.RegularExpressions.Regex.Match(item, "<li class=\"back\"><a href=\"\\./" + id + "(\\d{4})f.html\">前ページ</a></li>");
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
            if (headlines.Count > 0)
            {
                return headlines;
            }
            var wc = new System.Net.WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            try
            {
                if (lines == null || lines.Length == 0)
                {
                    lines = wc.DownloadString(KanpouUtil.MainUrl + String.Format("{0}/{1}/{1}0000f.html", date, id)).Split('\n');
                }

                string h2 = string.Empty;
                string h3 = string.Empty;
                string h4 = string.Empty;
                string headline = string.Empty;
                bool h2f = false;
                bool h3f = false;
                bool h4f = false;

                foreach (var l in lines)
                {
                    var m = System.Text.RegularExpressions.Regex.Match(l, titpat);
                    if (m.Success)
                    {
                        if(m.Groups[1].Value == "h2")
                        {
                            h2 = string.Empty;
                            h2f = true;
                        }
                        else if (m.Groups[1].Value == "h3")
                        {
                            h3 = string.Empty;
                            h3f = true;
                        }
                        else if (m.Groups[1].Value == "h4")
                        {
                            h4 = string.Empty;
                            h4f = true;
                        }
                    }
                    else
                    {
                        m = System.Text.RegularExpressions.Regex.Match(l, spanpat);
                        if (m.Success)
                        {
                            //小題にマッチ
                            if(m.Groups[1].Value == "text")
                            {
                                if (h2f)
                                {
                                    h2 = string.Format("〔{0}〕", m.Groups[2].Value);
                                    h2f = false;
                                }
                                else if (h3f)
                                {
                                    h3 = m.Groups[2].Value;
                                    h3f = false;
                                }
                                else if (h4f)
                                {
                                    h4 = m.Groups[2].Value;
                                    h4f = false;
                                }
                                else
                                {
                                    headline = m.Groups[2].Value;
                                }
                            }
                            else if (m.Groups[1].Value == "date")
                            {
                                int p = KanpouUtil.toInt(m.Groups[2].Value);
                                string h = string.Format("{0}{1} {2}:{3}", h2, h3, h4, headline);
                                headlines.Add(new HeadLine(h, p));
                                headline = string.Empty;
                            }
                        }
                    }
                }
            }
            catch (System.Net.WebException)
            {
                headlines.Clear();
            }
            return headlines;
        }
    }
}
