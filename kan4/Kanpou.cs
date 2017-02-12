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

        public Kanpou(string t,string d,string i)
        {
            title = t;
            date = d;
            id = i;
        }

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
    }
}
