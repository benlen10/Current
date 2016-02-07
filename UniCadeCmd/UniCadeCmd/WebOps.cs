using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace UniCadeCmd
{
    class WebOps
    {
        //Scraper Settings
        public static int metac = 1;
        public static int mobyg = 1;
        public static int year = 1;
        public static int publisher = 1;
        public static int critic = 1;
        public static int developer = 1;
        public static int description = 1;
        public static int esrb = 1;
        public static int esrbDescriptor = 1;
        public static int players = 1;
        public static int releaseDate = 1;
        public static int boxFront = 1;
        public static int boxBack = 1;
        public static int screenshot = 1;
        public static string gameName;

        public static int maxDescriptionLength = 5000;


        public static void scrapeInfo(Game g)
        {
            gameName = g.getTitle().Replace(" - ", " ");
            gameName = gameName.Replace(" ", "-");
            gameName = gameName.Replace("'", "");
            if (mobyg > 0)
            {
                scrapeMobyGames(g);
            }
            if (metac > 0)
            {
                scrapeMetacritic(g);
            }

        }



        public static void scrapeMobyGames(Game g)
        {
            if (g == null)
            {
                return;
            }
            string url = ("http://www.mobygames.com/game/" + g.getConsole() + "/" + gameName);
            url = url.ToLower();

            WebClient site = new WebClient();

            string html = site.DownloadString(url);

            //Parse ESRB
            if (esrb > 0)
            {
                int indexA = html.IndexOf("ESRB");
                if (indexA < 0)
                {
                    indexA = 0;
                }
                string s = html; //html.Substring(indexA, indexA + 50);
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
            }

            //Parse Release Date
            if (releaseDate > 0)
            {
                int tmp = html.IndexOf("release-info");

                if (tmp > 0)
                {
                    int indexB = html.IndexOf("release-info", (tmp + 20));

                    string releaseDate = html.Substring((indexB + 14), 4);
                    g.setReleaseDate(releaseDate);
                    System.Console.WriteLine(g.getReleaseDate());
                }

                //Parse Critic Scores
                tmp = 0;
                tmp = html.IndexOf("scoreHi");

                if (tmp > 0)
                {
                    string criticScore = html.Substring((tmp + 9), 2);
                    string s2 = html.Substring((tmp + 9));
                    g.setCriticScore(criticScore);

                }
            }


            //Parse Publisher
            if (publisher > 0)
            {
                int tmp = 0;
                tmp = html.IndexOf("/company/");
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("-", tmp + 10);
                    //System.Console.WriteLine(tmp);



                    string publisher = html.Substring((tmp + 9), tmp2 - (tmp + 9));
                    g.setPublisher(publisher);
                    System.Console.WriteLine(publisher);

                }
            }

            if (description > 0)
            {
                //Parse Description
                int tmp = 0;
                tmp = html.IndexOf("Description<");
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("<div class", tmp + 15);
                    System.Console.WriteLine(tmp);

                    System.Console.WriteLine(tmp2);
                    if (tmp2 > 0)
                    {
                        string description = html.Substring((tmp + 16), tmp2 - (tmp + 16));
                        description = Regex.Replace(description, @"\t|\n|\r", " ");
                        description = description.Replace("\"", "");
                        if (description.Length > maxDescriptionLength)
                        {
                            description = description.Substring(0, maxDescriptionLength);
                        }
                        g.setDescription(description);
                    }
                }
            }
        }


        public static void scrapeMetacritic(Game g)
        {
            //Metacritic Scraper

            string metaCon = "";
            if (g.getConsole().Equals("PS1"))
            {
                metaCon = "playstation";
            }
            else if (g.getConsole().Equals("N64"))
            {
                metaCon = "nintendo-64";
            }
            else if (g.getConsole().Equals("GBA"))
            {
                metaCon = "game-boy-advance";
            }
            else if (g.getConsole().Equals("PSP"))
            {
                metaCon = "psp";
            }
            else if (g.getConsole().Equals("Gamecube"))
            {
                metaCon = "gamecube";
            }
            else if (g.getConsole().Equals("Wii"))
            {
                metaCon = "wii";
            }
            else if (g.getConsole().Equals("NDS"))
            {
                metaCon = "ds";
            }
            else if (g.getConsole().Equals("Dreamcast"))
            {
                metaCon = "dreamcast";
            }


            string html;

            if (metaCon.Length < 1)
            {
                return;
            }

            string url = ("http://www.metacritic.com/game/" + metaCon + "/" + gameName + "/details");
            url = url.ToLower();

            var http = (HttpWebRequest)WebRequest.Create(url);
            http.UserAgent = "Chrome";
            try
            {
                var response = http.GetResponse();
                var stream = response.GetResponseStream();
                var sr = new StreamReader(stream);
                html = sr.ReadToEnd();
                response.Close();
            }
            catch (WebException ex)
            {

                return;
            }


            //Parse Esrb descriptors
            if (esrbDescriptor > 0)
            {
                int tmp = 0;
                tmp = html.IndexOf("ESRB Descriptors:");
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("</td>", tmp + 26);

                    if (tmp2 > 0)
                    {
                        string descriptors = html.Substring((tmp + 26), tmp2 - (tmp + 26));
                        g.setEsrbDescriptors(descriptors);
                    }
                }
            }

            if (players > 0)
            {
                //Parse Players (Metacritic)
                int tmp = 0;
                tmp = html.IndexOf("Players");
                if (tmp > 0)
                {
                    int tmp2 = html.IndexOf("<", tmp + 17);

                    if (tmp2 > 0)
                    {
                        string players = html.Substring((tmp + 17), tmp2 - (tmp + 17));
                        g.setPlayers(players);
                    }
                }
            }

           

                //Parse Developer (Metacritic)
                /*if (developer > 0)
                { 
                    int tmp = 0;
                    tmp = html.IndexOf("/company/",6850 );
                    int tmp2 = html.IndexOf("/company/", (tmp+30));
                    System.Console.WriteLine("Length" + tmp2);
                    if (tmp > 0)
                    {
                        //System.Console.WriteLine("YEs1");
                        int tmp3 = html.Substring(tmp).IndexOf(">");
                        if (true) //tmp2 > 0)
                        {
                            string dev = html.Substring((tmp2+9), 5);
                            System.Console.WriteLine(dev);
                            g.setDeveloper(dev);
                        }
                    }
                }
            }*/

            }
    }
}
