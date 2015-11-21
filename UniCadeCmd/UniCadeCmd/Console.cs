using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniCadeCmd
{
    class Console
    {
        private string name;
        private string emuPath;
        private string romPath;
        private string prefPath;
        private string romExt;
        private int gameCount;
        private string consoleInfo;
        private string launchParam;

        // Methods

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

        }

        public string getName()
        {
            return name;
        }
    }

}
