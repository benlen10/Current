using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace UniCadeCmd
{
    class WebOps
    {
        public static void scrapeInfo(Game g)
        {
            string url = ("http://www.mobygames.com/game/" + g.getConsole() +"/" + g.getTitle().Replace(" ", "-"));
            url = url.ToLower();
            System.Console.WriteLine(url);
            Uri uri = new Uri("http://www.mobygames.com/game/resident-evil-2");
            WebClient site = new WebClient();
            //Html from page
            string html = site.DownloadString(url);
            //System.Console.WriteLine(html);
            //System.Console.WriteLine("Scan Game "+ g.getTitle());




            //Parse ESRB
            int indexA = html.IndexOf("ESRB Rating");
            if (indexA < 0)
            {
                indexA = 0;
            }
            string s = html.Substring(indexA, indexA + 100);
            if (s.Contains("Everyone")){
                g.setEsrb("Everyone");
            }
            else if (s.Contains("Kids to Adults"))
            {
                g.setEsrb("Everyone (KA)");
            }
            else if (s.Contains("Everyone 10+"))
            {
                g.setEsrb("Everyone 10+");
            }
            else if (s.Contains("Teen")){
                g.setEsrb("Teen");
            }
            else if (s.Contains("Mature")){
                g.setEsrb("Mature");
            }
            else if (s.Contains("Mature")){
                g.setEsrb("Mature");
            }
            else if (s.Contains("Adults Only")){
                g.setEsrb("AO (Adults Only)");
            }

            if (g.getEsrb().Length > 2)
            {
                System.Console.WriteLine(g.getEsrb());
            }

        }
    }
}
