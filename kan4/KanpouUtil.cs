using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kan4
{
    public static class KanpouUtil
    {
        public static string MainUrl = "http://kanpou.npb.go.jp/";

        private static string ContentsUrl = "html/contents.html";
        private static string RegPat = "<a href=\"\\.\\./(20\\d{6})/(20\\d{6}[a-z]\\d{5})/20\\d{6}[a-z]\\d{5}0000f\\.html\".+>(.+)</a>";

        /**
         * 直近30日分の官報一覧を取得
         */
        public static List<Kanpou> getKanpouList()
        {
            var list = new List<Kanpou>();
            var wc = new System.Net.WebClient();
            try
            {
                var lines = wc.DownloadString(MainUrl + ContentsUrl).Split('\n');
                foreach (var l in lines)
                {
                    var m = System.Text.RegularExpressions.Regex.Match(l, RegPat);
                    if (m.Success)
                    {
                        list.Add(new Kanpou(m.Groups[3].Value, m.Groups[1].Value, m.Groups[2].Value));
                    }
                }
            }
            catch (System.Net.WebException)
            {
                list.Clear();
//                throw;
            }
            return list;
        }

        /**
         * ファイルをダウンロードして、指定のディレクトリに保存する。
         */
        public static bool downloadFiles(List<string> urls,string dir)
        {
            bool ret = true;
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            var wc = new System.Net.WebClient();
            foreach (var u in urls)
            {
                var name = System.IO.Path.GetFileName(u);
                try
                {
                    wc.DownloadFile(u, String.Format("{0}\\{1}", dir, name));
                }
                catch (System.Net.WebException)
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }

        /**
         * 指定のファイルを削除する
         * ダウンロードした官報ファイルの削除
         */
        public static void deleteFiles(List<string> list)
        {
            foreach (var f in list)
            {
                if (System.IO.File.Exists(f))
                {
                    System.IO.File.Delete(f);
                }
            }
        }
    }
}
