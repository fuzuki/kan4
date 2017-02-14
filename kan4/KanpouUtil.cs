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


        /// <summary>
        /// 全角交じりの数値文字列を、数字に変換
        /// </summary>
        /// <param name="s">数値文字列</param>
        /// <returns></returns>
        public static int toInt(string s)
        {
            var tmp = System.Text.RegularExpressions.Regex.Replace(s, "[０-９]", p => ((char)(p.Value[0] - '０' + '0')).ToString());
            return int.Parse(tmp);
        }

        /// <summary>
        /// 直近30日分の官報一覧を取得
        /// </summary>
        /// <returns></returns>
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
            }
            return list;
        }

        /// <summary>
        /// ファイルをダウンロードして、指定のディレクトリに保存する。
        /// </summary>
        /// <param name="urls">ダウンロードするURLのリスト</param>
        /// <param name="dir">保存先</param>
        /// <returns>成功/失敗</returns>
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
                    var ofile = String.Format("{0}\\{1}", dir, name);
                    if (!System.IO.File.Exists(ofile)) {
                        wc.DownloadFile(u, ofile);
                    }
                }
                catch (System.Net.WebException)
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }


        /// <summary>
        /// 指定のファイルを削除する
        /// </summary>
        /// <param name="list"></param>
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

        public static void downloadKanpou(Kan4DB db)
        {
            var klist = KanpouUtil.getKanpouList();
            string mydir = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            string pdfdir = mydir + "\\pdf";
            string tmpdir = mydir + "\\tmp";
            if (!System.IO.Directory.Exists(pdfdir))
            {
                System.IO.Directory.CreateDirectory(pdfdir);
            }

            db.open();
            foreach (var k in klist)
            {

                var joined = string.Format("{0}\\{1}.pdf", pdfdir, k.id);
//                if(!System.IO.File.Exists(joined))
                if (!db.isRegisted(k))
                {
                    var plist = k.getPdfUrls();
                    k.getHeadLines();
                    if (downloadFiles(plist,tmpdir))
                    {
                        var filelist = new List<string>();
                        foreach (var u in plist)
                        {
                            filelist.Add(string.Format("{0}\\{1}", tmpdir, System.IO.Path.GetFileName(u)));
                        }
                        PdfUtil.joinPdf(filelist, joined);
                        db.regist(k);
                        deleteFiles(filelist);
                    }
                }
                break;//for test
            }
            db.close();
        }
    }
}
