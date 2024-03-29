﻿using System;
using System.Collections.Generic;

namespace kan4
{
    public static class KanpouUtil
    {
        public static string MainUrl = "https://kanpou.npb.go.jp/";

        private static readonly string ContentsUrl = "index.html";
        private static readonly string RegPat = "<a href=\"\\./20\\d{6}/(20\\d{6}[a-z]\\d{5})/20\\d{6}[a-z]\\d{5}0000f\\.html\" class=.*>(.+)<br>(.+)</a>";


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
        /// インターネット官報サイトから直近30日分の官報一覧を取得
        /// </summary>
        /// <returns></returns>
        public static List<Kanpou> getKanpouList()
        {
            var list = new List<Kanpou>();
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var wc = new System.Net.WebClient();
            wc.Encoding = System.Text.Encoding.UTF8;
            try
            {
                var lines = wc.DownloadString(MainUrl + ContentsUrl).Split('\n');
                foreach (var l in lines)
                {
                    var m = System.Text.RegularExpressions.Regex.Match(l, RegPat);
                    if (m.Success)
                    {
                        list.Add(new Kanpou(string.Format("{0} {1}",m.Groups[2].Value, m.Groups[3].Value), m.Groups[1].Value));
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
            var pdflist = new List<string>();
            foreach (var u in urls)
            {
                var name = System.IO.Path.GetFileName(u);
                try
                {
                    var ofile = String.Format("{0}\\{1}", dir, name);
                    pdflist.Add(ofile);
                    if (System.IO.File.Exists(ofile)) {
                        System.IO.File.Delete(ofile);
                    }
                    wc.DownloadFile(u, ofile);
                    System.Threading.Thread.Sleep(50);//連続ダウンロードを控えるため
                }
                catch (System.Net.WebException)
                {
                    ret = false;
                    deleteFiles(pdflist);//正常にダウンロードできていない可能性
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
            foreach (var k in klist)
            {
                downloadKanpou(db,k);
                break;//for test
            }
        }

        /// <summary>
        /// 官報のダウンロード・連結・DB登録
        /// </summary>
        /// <param name="db"></param>
        /// <param name="k"></param>
        public static void downloadKanpou(Kan4DB db, Kanpou k)
        {
            db.open();
            if (!db.isRegisted(k))
            {
                var plist = k.getPdfUrls();
                k.getHeadLines();
                var tmpdir = getTempDir();
                
                if (downloadFiles(plist, tmpdir))
                {
                    var filelist = new List<string>();
                    foreach (var u in plist)
                    {
                        filelist.Add(string.Format("{0}\\{1}", tmpdir, System.IO.Path.GetFileName(u)));
                    }

                    try
                    {
                        PdfUtil.joinPdf(filelist, getKanpouPath(k));
                        db.regist(k);
                        deleteFiles(filelist);
                    }
                    catch (Exception)
                    {
                        // 連結に失敗した場合、日付ディレクトリを作って、ファイルをすべてそこに移動する
                        movePdf(filelist, getKanpouIdDirPath(k));
                        db.regist(k);
                        deleteFiles(filelist);
                    }
                }
            }
            db.close();
        }

        /// <summary>
        /// pdfファイルを移動する
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="outdir"></param>
        public static void movePdf(List<string> paths, string outdir)
        {
            System.IO.Directory.CreateDirectory(outdir);
            //System.IO.File.Move();
            foreach (var item in paths)
            {
                var destfile = string.Format("{0}\\{1}", outdir, System.IO.Path.GetFileName(item));
                System.IO.File.Copy(item, destfile, true);
            }
        }

        /// <summary>
        /// pdfファイルを開く
        /// </summary>
        /// <param name="id"></param>
        public static void openKanpouPdf(string id)
        {
            var path = getKanpouPath(id);
            var fi = new System.IO.FileInfo(path);
            if (path.Length > 0)
            {
                if (System.IO.File.Exists(path) && fi.Length > 0)
                {
                    System.Diagnostics.Process.Start(path);
                }
                else
                {
                    if (System.IO.File.Exists(path) && fi.Length == 0)
                    {
                        System.IO.File.Delete(path);
                    }
                    var dirpath = getKanpouIdDirPath(id);
                    if (System.IO.Directory.Exists(dirpath))
                    {
                        System.Diagnostics.Process.Start(dirpath);
                    }
                }
            }
        }

        /// <summary>
        /// Acrobat Readerでファイルを開く
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="readerPath"></param>
        public static void openKanpouPdf(string id, int page, string readerPath)
        {
            if (System.IO.File.Exists(readerPath))
            {
                var path = getKanpouPath(id);
                var fi = new System.IO.FileInfo(path);
                if (path.Length > 0 && System.IO.File.Exists(path) && fi.Length > 0)
                {
                    System.Diagnostics.Process.Start(readerPath, string.Format("/a page={0} \"{1}\"", page, path));
                }
                else
                {
                    openKanpouPdf(id);
                }
            }
            else
            {
                openKanpouPdf(id);
            }
        }

        /// <summary>
        /// 官報pdfのファイルパス取得
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string getKanpouPath(Kanpou k)
        {
            return getKanpouPath(k.id);
        }
        /// <summary>
        /// 官報pdfのIDディレクトリパス取得
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string getKanpouIdDirPath(Kanpou k)
        {
            return getKanpouIdDirPath(k.id);
        }
        /// <summary>
        /// 官報pdfのファイルパス取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getKanpouPath(string id)
        {
            var m = System.Text.RegularExpressions.Regex.Match(id, "^(20\\d\\d)\\d{4}[a-z]\\d{5}$");
            if (!m.Success)
            {
                return string.Empty;
            }
            string kdir = string.Format("{0}\\{1}", getPdfDir(), m.Groups[1].Value);
            if (!System.IO.Directory.Exists(kdir))
            {
                System.IO.Directory.CreateDirectory(kdir);
            }
            return string.Format("{0}\\{1}.pdf", kdir, id);
        }
        /// <summary>
        /// 官報pdfのIDディレクトリパス取得
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getKanpouIdDirPath(string id)
        {
            var m = System.Text.RegularExpressions.Regex.Match(id, "^(20\\d\\d)\\d{4}[a-z]\\d{5}$");
            if (!m.Success)
            {
                return string.Empty;
            }
            string kdir = string.Format("{0}\\{1}", getPdfDir(), m.Groups[1].Value);
            if (!System.IO.Directory.Exists(kdir))
            {
                System.IO.Directory.CreateDirectory(kdir);
            }
            return string.Format("{0}\\{1}", kdir, id);
        }

        private static string getPdfDir()
        {
            string mydir = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            string pdfdir = mydir + "\\pdf";
            if (!System.IO.Directory.Exists(pdfdir))
            {
                System.IO.Directory.CreateDirectory(pdfdir);
            }

            return pdfdir;
        }

        private static string getTempDir()
        {
            string mydir = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            string tmpDir = mydir + "\\tmp";
            if (!System.IO.Directory.Exists(tmpDir))
            {
                System.IO.Directory.CreateDirectory(tmpDir);
            }

            return tmpDir;
        }
    }




}
