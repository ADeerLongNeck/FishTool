using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using FishTool;

namespace WebClick_Tool
{
    public class KeyboardHook
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

        public static List<String> pids = new List<string>();

        public static int windowX = 500;

        public static int windowY = 400;
        /// <summary>
        /// 结束进程
        /// </summary>
        /// <param name="procName">进程名称</param>
        public static void KillProc(string procName)
        {
            // 获取系统当前运行的所有进程
            Process[] allProcess = Process.GetProcesses();
            foreach (Process p in allProcess)
            {
                System.Console.WriteLine(p.ToString());
            }
            // 结束指定进程名称的进程
            Process[] killprocess = Process.GetProcessesByName(procName);
            foreach (System.Diagnostics.Process p in killprocess)
            {
                try
                {
                    p.Kill();
                }
                catch(Exception ex)
                {
                    
                   //   MessageBox.Show(ex.ToString()); 
                  
                }
               
            }
        }

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

        public static int Hook_Start()
        {
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
        //调用Win32 API

        //窗口透明
        /**
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        // hWnd是句柄，factor是透明度0~255
          public static  bool  MakeWindowTransparent(String hWnd, byte factor)
        {
            Process[] allProcess = Process.GetProcesses();
            foreach (Process p in allProcess)
            {
                System.Console.WriteLine(p.ToString());
            }
            // 结束指定进程名称的进程
            Process[] processes = Process.GetProcessesByName(hWnd);
            foreach(Process hWnd2 in processes){
            const int GWL_EXSTYLE = (-20);
            const uint WS_EX_LAYERED = 0x00080000;
            int Cur_STYLE = GetWindowLong(hWnd2.MainWindowHandle, GWL_EXSTYLE);
            SetWindowLong(hWnd2.MainWindowHandle, GWL_EXSTYLE, (uint)(Cur_STYLE | WS_EX_LAYERED));
            const uint LWA_COLORKEY = 1;
            const uint LWA_ALPHA = 2;
            const uint WHITE = 0xffffff;
            return SetLayeredWindowAttributes(hWnd2.MainWindowHandle, WHITE, factor, LWA_COLORKEY | LWA_ALPHA);
            }
            return false;
        }


        **/


        //移动窗口
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
        public static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        //打开窗体方法，fileName是的窗体名称，包含路径
        static Boolean isHide = false;
        private static void OpenAndSetWindow(String procName,Boolean Hide)
        {
            /**
            Process p = new Process();//新建进程
            p.StartInfo.FileName = fileName;//设置进程名字
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            p.Start();
            MoveWindow(p.MainWindowHandle, 200, 300, 500, 400, true);
            **/

            // 获取系统当前运行的所有进程
            Process[] allProcess = Process.GetProcesses();
            foreach (Process p in allProcess)    
            {
                System.Console.WriteLine(p.ToString());
            }
            // 结束指定进程名称的进程
            Process[] killprocess = Process.GetProcessesByName(procName);
            foreach (System.Diagnostics.Process p in killprocess)
            {
                try
                {
                    if (Hide)
                    {
                       
                       MoveWindow(p.MainWindowHandle, 200, 99999, 500, 400, true);

                    }
                    else
                    {
                        MoveWindow(p.MainWindowHandle, 0, 0, windowX, windowY, true);

                    }
                   
                }
                catch (Exception ex)
                {

                    //   MessageBox.Show(ex.ToString()); 

                }

            }

            //p.MainWindowHandle是你要移动的窗口的句柄；200,300是移动后窗口左上角的横纵坐标；500,400是移动后窗口的宽度和高度；true表示移动后的窗口是需要重画

        }

        public static int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                Keys k = (Keys)Enum.Parse(typeof(Keys), kbh.vkCode.ToString());
                if(k == SetHotKey.hotkey)
                {
                    for (int i = 0; i < pids.Count; i++)
                    {
                        KillProc(pids[i]);
                    }
                }
             /**   if (k == Keys.Space)
                {
                    if (kbh.flags == 0)
                    {
                        for (int i = 0; i < pids.Count; i++)
                        {
                            MakeWindowTransparent(pids[i], 128);

                        }
                    }
                   
                }**/
                if (k == SetHotKey.hotkeyHide)
                {
                    if (kbh.flags == 0)
                    {
                        for (int i = 0; i < pids.Count; i++)
                        {
                            if (isHide)
                            {
                                OpenAndSetWindow(pids[i],true);
                            }
                            else
                            {
                                OpenAndSetWindow(pids[i],false);
                            }
                           
                        }
                        if (isHide)
                        {
                            isHide = false;
                        }
                        else
                        {
                            isHide = true;
                        }
                    }
          }
                /**
                switch (k)
                {
                    case :
                        if (kbh.flags == 0)
                        {
                            // 这里写按下后做什么事
                            //Main.GB = true;
                            Console.WriteLine("芜湖芜湖");
                         
                        }
                        else if (kbh.flags == 128)
                        {
                            //放开后做什么事
                        }
                        return 1;  
                }**/
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }
    }
    /**
     * // 枚举窗体
    public static class WindowsEnumerator
    {
        private delegate bool EnumWindowsProc(IntPtr windowHandle, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool EnumChildWindows(IntPtr hWndStart, EnumWindowsProc callback, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);
        private static List<IntPtr> handles = new List<IntPtr>();
        private static string targetName;
        public static List<IntPtr> GetWindowHandles(string target)
        {
            targetName = target;
            EnumWindows(EnumWindowsCallback, IntPtr.Zero);
            return handles;
        }
        private static bool EnumWindowsCallback(IntPtr HWND, IntPtr includeChildren)
        {
            StringBuilder name = new StringBuilder(GetWindowTextLength(HWND) + 1);
            GetWindowText(HWND, name, name.Capacity);
            if (name.ToString() == targetName)
                handles.Add(HWND);
            EnumChildWindows(HWND, EnumWindowsCallback, IntPtr.Zero);
            return true;
        }
    }
    **/
}

