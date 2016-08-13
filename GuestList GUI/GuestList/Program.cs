using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuestList
{
    
    static class Program
    {
        public static Form1 f1;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f1 = new Form1();
            Application.Run(f1);
        }
        /*static void OnProcessExit(object sender, EventArgs e)
        {
            f1.saveDatabase();
        }*/
    }
}
