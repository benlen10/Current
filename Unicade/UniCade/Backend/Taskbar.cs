using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace UniCade
{
    public class Taskbar
    {
        #region Properties

        /// <summary>
        /// Constant for the hidden state of the Windows taskbar
        /// </summary>
        private const int SW_HIDE = 0;

        /// <summary>
        /// Constant for the visible state of the Windows taskbar
        /// </summary>
        private const int SW_SHOW = 1;

        #endregion

        #region DLL Imports

        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int parentHandle, int childAfter, string className, int windowTitle);

        [DllImport("user32.dll")]
        private static extern int GetDesktopWindow();

        #endregion

        #region Public Methods

        /// <summary>
        /// Show the Windows taskbar
        /// </summary>
        public static void ShowTaskbar()
        {
            ShowWindow(Handle, SW_SHOW);
            ShowWindow(HandleOfStartButton, SW_SHOW);
        }

        /// <summary>
        /// Hide the Windows taskbar
        /// </summary>
        public static void HideTaskbar()
        {
            ShowWindow(Handle, SW_HIDE);
            ShowWindow(HandleOfStartButton, SW_HIDE);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Return the current handle for Shell_TrayWnd
        /// </summary>
        protected static int Handle
        {
            get
            {
                return FindWindow("Shell_TrayWnd", "");
            }
        }

        /// <summary>
        /// Return the current handle for the start button
        /// </summary>
        protected static int HandleOfStartButton
        {
            get
            {
                int handleOfDesktop = GetDesktopWindow();
                int handleOfStartButton = FindWindowEx(handleOfDesktop, 0, "button", 0);
                return handleOfStartButton;
            }
        }

        /// <summary>
        /// Private constructor for the Taskbar class
        /// </summary>
        private Taskbar()
        {
        }

        #endregion
    }
}