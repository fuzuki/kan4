using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kan4
{
    public static class Test
    {
        public static void showList()
        {
            int i = 0;
            var l = KanpouUtil.getKanpouList();
            Console.WriteLine(l.Count);
            foreach(var t in l)
            {
                Console.WriteLine(string.Format("{0},{1}", t.title,t.id));
                var ul = t.getPdfUrls();
                foreach(var u in ul)
                {
                    Console.WriteLine(u);
                }
                var hl = t.getHeadLines();
                foreach(var h in hl)
                {
                    Console.WriteLine(h.text);
                }
                if(i > 1)
                {
                    break;
                }
                i++;
            }
        }
    }
}
