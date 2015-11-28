using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace UniCadeCmd
{
    class Console
    {
        private string name;
        private string emuPath;
        private string romPath;
        private string prefPath;
        private string romExt;
        private string consoleInfo;
        private string launchParam;
        private ArrayList gameList;
        public int gameCount;

        // Methods

        public Console()
        {
            name = "null";
        }

        public Console(string name, string emuPath, string romPath, string prefPath, string romExt, int gameCount, string consoleInfo, string launchParam)
        {
            this.name = name;
            this.emuPath = emuPath;
            this.romPath = romPath;
            this.prefPath = prefPath;
            this.romExt = romExt;
            this.gameCount = gameCount;
            this.consoleInfo = consoleInfo;
            this.launchParam = launchParam;
            gameList = new ArrayList();

        }

        public string getName()
        {
            return name;
        }

        public ArrayList getGameList()
        {
            return gameList;
        }

        public string getEmuPath()
        {
            return emuPath;
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

        public void setRomPath(string s)
        {
            romPath = s;
        }

        public void setRomEet(string s)
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
