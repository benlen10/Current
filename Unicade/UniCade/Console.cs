using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UniCadeCmd
{
    public class Console
    {
        private string name;
        private string emuPath;
        private string romPath;
        private string prefPath;
        private string romExt;
        private string consoleInfo;
        private string releaseDate;
        private string launchParam;
        private ArrayList gameList;
        public int gameCount;

        // Methods

        public Console()
        {
            name = "null";
        }

        public Console(string name, string emuPath, string romPath, string prefPath, string romExt, int gameCount, string consoleInfo, string launchParam, string releaseDate)
        {
            this.name = name;
            this.emuPath = emuPath;
            this.romPath = romPath;
            this.prefPath = prefPath;
            this.romExt = romExt;
            this.gameCount = gameCount;
            this.consoleInfo = consoleInfo;
            this.launchParam = launchParam;
            this.releaseDate = releaseDate;
            gameList = new ArrayList();

        }

        public string getName()
        {
            return name;
        }

        public string getReleaseDate()
        {
            return releaseDate;
        }


        public ArrayList getGameList()
        {
            return gameList;
        }

        public string getEmuPath()
        {
            return emuPath;
        }

        public bool addGame(Game gam)
        {
            if (!gam.getConsole().Equals(name))
            {
                return false;
            }
            foreach(Game g in gameList)
            {
                if (g.getFileName().Equals(gam.getFileName())){
                    return false;
                }
            }
            return true;
        }

        public string getPrefPath()
        {
            return prefPath;
        }

        public string getRomPath()
        {
            return romPath;
        }

        public string getRomExt()
        {
            return romExt;
        }

        public string getConsoleInfo()
        {
            return consoleInfo;
        }

        public string getLaunchParam()
        {
            return launchParam;
        }

        public void setName(string s)
        {
            name = s;
        }

        public void setEmuPath(string s)
        {
            emuPath = s;
        }

        public void setReleaseDate(string s)
        {
            releaseDate = s;
        }

        public void setRomPath(string s)
        {
            romPath = s;
        }

        public void setRomExt(string s)
        {
           romExt  = s;
        }

        public void setConsoleInfo(string s)
        {
            consoleInfo = s;
        }

        public void setLaunchParam(string s)
        {
            launchParam = s;
        }


    }

}
