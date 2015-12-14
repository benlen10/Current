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
            string url = ("http://www.mobygames.com/game/" + g.getConsole() + "/" + g.getTitle().Replace(" ", "-"));
            url = url.ToLower();
            //System.Console.WriteLine(url);
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
            if (s.Contains("Everyone"))
            {
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
            else if (s.Contains("Teen"))
            {
                g.setEsrb("Teen");
            }
            else if (s.Contains("Mature"))
            {
                g.setEsrb("Mature");
            }
            else if (s.Contains("Mature"))
            {
                g.setEsrb("Mature");
            }
            else if (s.Contains("Adults Only"))
            {
                g.setEsrb("AO (Adults Only)");
            }

            if (g.getEsrb().Length > 2)
            {
                System.Console.WriteLine(g.getEsrb());
            }
            else
            {
                g.setEsrb("Unrated");
            }

            //Parse Release Date

            int tmp = html.IndexOf("release-info");

            if (tmp > 0)
            {
                int indexB = html.IndexOf("release-info", (tmp + 20));

                string releaseDate = html.Substring((indexB + 14), 12);
                g.setReleaseDate(releaseDate);
                System.Console.WriteLine(g.getReleaseDate());
            }

            //Parse User and Critic Scores
            tmp = 0;
            tmp = html.IndexOf("scoreHi");
            //System.Console.WriteLine(tmp);

            if (tmp > 0)
            {
                string criticScore = html.Substring((tmp + 9), 2);
                g.setCriticScore(criticScore);
                System.Console.WriteLine(g.getCriticScore());
                int indexB = html.IndexOf("ScoreH", (tmp + 20));
                if (indexB > 0)
                {
                    string userScore = html.Substring((indexB + 9), 2);   //Fix parsing userscore
                    g.setUserScore(userScore);
                    System.Console.WriteLine(userScore);

                }
            }
            //Parse Publisher
            tmp = 0;
            tmp = html.IndexOf("/company/");
            if (tmp > 0)
            {
                int tmp2 = html.IndexOf("\"<", tmp);
                //System.Console.WriteLine(tmp);



                    string publisher = html.Substring((tmp + 9), 10);
                    g.setPublisher(publisher);
                    System.Console.WriteLine(publisher);
                    int indexB = html.IndexOf("ScoreH", (tmp + 20));



            }


        }
    }
}
