using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UniCade.Backend
{
    internal class GlobalKeyboardHook
    {
        #region Properties

        /// <summary>
        /// A list of the currently hooked keys
        /// </summary>
        public List<Keys> HookedKeys = new List<Keys>();

        /// <summary>
        /// A pointer to an integer for the current hook instance
        /// </summary>
        IntPtr _hhook = IntPtr.Zero;

        /// <summary>
        /// The KeyHandler event when a key is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// The KeyHandler event when a key is released
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// Function delegate for the keyboardHookProc function
        /// </summary>
        public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        /// <summary>
        /// A static function delegate for the keyboardHookProc function
        /// </summary>
        private static KeyboardHookProc _callbackDelegate;

        /// <summary>
        /// Keyboard code constants
        /// </summary>
        const int WhKeyboardLl = 13;
        const int WmKeydown = 0x100;
        const int WmKeyup = 0x101;
        const int WmSyskeydown = 0x104;
        const int WmSyskeyup = 0x105;

        /// <summary>
        /// An instance of the keybaord hook
        /// </summary>
        public struct KeyboardHookStruct
        {
            public int VkCode;
            public int ScanCode;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }

        #endregion

        #region Constructor & Descructor

        /// <summary>
        /// Public constructor for the GlobalKeyboardHook class
        /// </summary>
        public GlobalKeyboardHook()
        {
            HookKeys();
            Hook();
        }

        /// <summary>
        /// Hook the list of specified keys
        /// </summary>
        public void HookKeys()
        {
            HookedKeys.Clear();
            HookedKeys.Add(Keys.N );
            HookedKeys.Add(Keys.NumPad2);
            HookedKeys.Add(Keys.NumPad3);
            HookedKeys.Add(Keys.NumPad4);
            HookedKeys.Add(Keys.A);
            HookedKeys.Add(Keys.B);
            HookedKeys.Add(Keys.Left);
            HookedKeys.Add(Keys.Right);
            HookedKeys.Add(Keys.Enter);
            HookedKeys.Add(Keys.I);
            HookedKeys.Add(Keys.Back);
            HookedKeys.Add(Keys.Space);
            HookedKeys.Add(Keys.Tab);
            HookedKeys.Add(Keys.Escape);
            HookedKeys.Add(Keys.Delete);
            HookedKeys.Add(Keys.F);
            HookedKeys.Add(Keys.G);
            HookedKeys.Add(Keys.C);
            HookedKeys.Add(Keys.P);
            HookedKeys.Add(Keys.B);
            HookedKeys.Add(Keys.S);
            HookedKeys.Add(Keys.E);
            HookedKeys.Add(Keys.Q);
            HookedKeys.Add(Keys.F10);
            HookedKeys.Add(Keys.F1);
        }

        /// <summary>
        /// destructor for the GlobalKeyboardHook class
        /// </summary>
        ~GlobalKeyboardHook()
        {
            Unhook();
        }

        #endregion 

        #region Public Methods

        /// <summary>
        /// Hook the specified keys
        /// </summary>
        public void Hook()
        {
            if (_callbackDelegate != null)
            {
                throw new InvalidOperationException("Can't hook more than once");
            }

            IntPtr hInstance = LoadLibrary("User32");
            _callbackDelegate = HookProc;
            _hhook = SetWindowsHookEx(WhKeyboardLl, _callbackDelegate, hInstance, 0);
            if (_hhook == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
        }

        /// <summary>
        /// Unhook the specified keys and set the callbackDelegate to null
        /// </summary>
        public void Unhook()
        {
            _callbackDelegate = null;
        }



        /// <summary>
        /// Hook the specified keys
        /// </summary>
        public int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.VkCode;
                if (HookedKeys.Contains(key))
                {
                    KeyEventArgs keyEventArgs = new KeyEventArgs(key);
                    if ((wParam == WmKeydown || wParam == WmSyskeydown) && (KeyDown != null))
                    {
                        KeyDown(this, keyEventArgs);
                    }
                    else if ((wParam == WmKeyup || wParam == WmSyskeyup))
                    {
                        KeyUp?.Invoke(this, keyEventArgs);
                    }
                    if (keyEventArgs.Handled)
                    {
                        return 1;
                    }
                }
            }
            return CallNextHookEx(_hhook, code, wParam, ref lParam);
        }

        #endregion

        #region DLL Imports

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        #endregion
    }
}
