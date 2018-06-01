using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HookDLL;

namespace MySpy
{
    class HookApp
    {
        private const int WH_MOUSE_LL = 14;
        private const int WH_KEYBOARD_LL = 13;

        private IntPtr hookKeyId, hookMouseId;
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        


        public HookApp()
        {
            HookProceduces hookProceduces = new HookProceduces();
            this.hookMouseId=SetHook(WH_MOUSE_LL, hookProceduces.MouseHookCallback);
            this.hookKeyId=SetHook(WH_KEYBOARD_LL, hookProceduces.KeyHookCallback);
            hookProceduces.hookKeyId = this.hookKeyId;
            hookProceduces.hookMouseId = this.hookMouseId;
        }

        ~HookApp()
        {
            UnhookWindowsHookEx(hookMouseId);
            UnhookWindowsHookEx(hookKeyId);
        }


        private static IntPtr SetHook(int idHook,HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    return SetWindowsHookEx(idHook, proc,GetModuleHandle(curModule.ModuleName), 0);
                }
            }
            
        }

    }
}
