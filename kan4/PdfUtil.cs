using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kan4
{
    public static class PdfUtil
    {
        public static bool joinPdf(List<string> paths,string outpath)
        {
            bool ret = true;
            var outfile = new System.IO.FileStream(outpath, System.IO.FileMode.OpenOrCreate);
            var copy = new iTextSharp.text.pdf.PdfCopyFields(outfile);
            iTextSharp.text.pdf.PdfReader.unethicalreading = true;//owner passwordを無視
            var readers = new List<iTextSharp.text.pdf.PdfReader>();
            foreach (var item in paths)
            {
                if (!System.IO.File.Exists(item))
                {
                    ret = false;
                    break;
                }
                var tmp = new iTextSharp.text.pdf.PdfReader(item);
                if (tmp.GetPageRotation(1) != 0)
                {
                    tmp.GetPageN(1).Put(iTextSharp.text.pdf.PdfName.ROTATE, new iTextSharp.text.pdf.PdfNumber(0));
                }
                readers.Add(tmp);
                copy.AddDocument(tmp);
            }
            copy.Close();
            outfile.Close();
            foreach (var item in readers)
            {
                item.Close();
            }
            return ret;
        }
    }
}
