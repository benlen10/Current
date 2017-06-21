using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UniCade.ConsoleInterface
{
    class UniCadeCmd
    {

        public static void Run()
        {
            ShowConsoleWindow();
            while (true)
            {
                System.Console.WriteLine("Enter Cmd (Type 'exit' to close)");
                string line = System.Console.ReadLine();
                if (line.Contains("exit"))
                {
                    HideConsoleWindow();
                    return;
                }
            }
        }

        public static void PrepAndRun()
        {
            //Prep
            MainWindow.UnhookKeys();
            Program.App.MainWindow.Hide();

            //Run
            Run();

            //Cleanup
            MainWindow.ReHookKeys();
            Program.App.MainWindow.Show();
        }

        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
        }

        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();

            ShowWindow(handle, SW_HIDE);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

    }
}
