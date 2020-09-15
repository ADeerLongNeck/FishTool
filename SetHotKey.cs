using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace FishTool
{
    public class SetHotKey
    {
        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
     
        public static Keys hotkey = Keys.F2;
        public static Keys hotkeyHide = Keys.LShiftKey;
        public static int SET_HOTKEY_CLOSE = 0;
        public static int SET_HOTKEY_HIDE = 1;

        //委托 
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
        static int hHook = 0;
        public const int WH_KEYBOARD_LL = 13;
        //LowLevel键盘截获，如果是WH_KEYBOARD＝2，并不能对系统键盘截取，Acrobat Reader会在你截取之前获得键盘。 
        static HookProc KeyBoardHookProcedure;

        //设置钩子 
        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        //抽掉钩子 
        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll")]
        //调用下一个钩子 
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        static int hookFlag = 0;
        public static int Hook_Start(int Flag)
        {
            hookFlag = Flag;
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(KeyBoardHookProc);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyBoardHookProcedure,
                        GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //如果设置钩子失败. 
                if (hHook == 0)
                {
                    Hook_Clear();
                    return 1;
                }
                return 0;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// 取消钩子事件
        /// </summary>
        public static void Hook_Clear()
        {
            bool retKeyboard = true;
            if (hHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hHook);
                hHook = 0;
            }
        }

        public static int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                Keys k = (Keys)Enum.Parse(typeof(Keys), kbh.vkCode.ToString());
            
                if(kbh.flags == 0)
                {
                    if(hookFlag == SET_HOTKEY_CLOSE)
                    {
                        hotkey = k;
                    }
                    if(hookFlag == SET_HOTKEY_HIDE)
                    {
                        hotkeyHide = k;
                    }
                    
                  
                }
               
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }


}