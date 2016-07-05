using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniCadeCmd
{
    public class BackgroundThread
    {
        public void BackgroundProcess() {
            while (true)
            {
                System.Console.WriteLine("Thread Loop");

                Thread.Sleep(5000);
                Program.gui.activate();
            }
        }

    }
}
