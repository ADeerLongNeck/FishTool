using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebClick_Tool;

namespace FishTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            buttonSetHotKeyHight = buttonSetHotKey.Height;
            stateHook.Content = "等待钩子加载";
            int res = KeyboardHook.Hook_Start();
            if(res == 0)
            {
                stateHook.Content = "钩子加载成功";
                stateHook.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5FB878"));
            }else if(res == 1)
            {
                stateHook.Content = "钩子加载失败";
                stateHook.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF5722"));
            }
            else
            {
                stateHook.Content = "钩子加载异常";
                stateHook.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB800"));
            }
            reloadSystemPids();
        }
        public void reloadSystemPids()
        {
            lisBoxPid.Items.Clear();
            HashSet<String> pidList = getAllProcess();
     
            foreach (String item in pidList)
            {
                lisBoxPid.Items.Add(item);
            }
   
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        public HashSet<string> getAllProcess()
        {
            List<String> list = new List<string>();
            Process[] allProcess = Process.GetProcesses();
            foreach (Process p in allProcess)
            {
                System.Console.WriteLine(p.ProcessName + "===" + p.ToString());
                list.Add(p.ProcessName);
            }
          
            HashSet<string> hs = new HashSet<string>(list);
            
            return hs;
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            string selectedText = lisBoxPid.SelectedItem.ToString();
            int flag = 0;
            if (listBoxPidSelected.Items.Count > 0)
            {
                foreach (String item in listBoxPidSelected.Items)
                {
                    if (item.Equals(selectedText))
                    {
                        flag++;
                    }
                }
            }
            if (flag == 0)
            {
                listBoxPidSelected.Items.Add(selectedText);
                reloadListOfPid();
            }
         
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            
            int SelectItems = listBoxPidSelected.SelectedItems.Count;
            if (listBoxPidSelected.SelectedItems.Count > 0)
            {
                for (int i = 0; i < SelectItems; i++)
                {
                    listBoxPidSelected.Items.Remove(listBoxPidSelected.SelectedItems[0]);
                }
                reloadListOfPid();
            }
            else
            {
                MessageBox.Show("没有选择");
                return;
            }
        }
        public void reloadListOfPid()
        {
            KeyboardHook.pids.Clear();
            foreach (String item in listBoxPidSelected.Items)
            {
                KeyboardHook.pids.Add(item);
            }
         
        }
       

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            reloadSystemPids();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            List<String> list = new List<string>();
            List<String> list_o = new List<string>();
            foreach (String s in getAllProcess())
            {
                list_o.Add(s);
                String ls =  s.ToLower();
              //  list.Add(ls);
                
            }
            //list = list.Where(x => x.Contains(textSearch.Text)).ToList();
            foreach(String s in list_o)
            {
                if(s.IndexOf(textSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    list.Add(s);
                }
            }
            lisBoxPid.Items.Clear();
            foreach (String item in list)
            {
                lisBoxPid.Items.Add(item);
            }
           
        }
       
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            listBoxPidSelected.Items.Clear();
            KeyboardHook.pids.Clear();
          
            reloadSystemPids();
        }

       
        public static void setHotKeyText(String text)
        {
           
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            String helpText = " ===========【FishTool V 2.3】By 长脖子鹿===========\n" +
                "【1】将要关闭的进程从左侧选中后点击‘>’按钮导入到关闭栏\n" +
                "【2】点击F2热键(默认)即可秒杀右侧列表中的进程。\n" +
                "【3】点击右下角“设置”可以更改热键和窗口大小等\n" +           
                "【4】点击左Shift键(默认)可以切换进程的显示/隐藏,设定窗口大小\n" +
                "=======================================\n" +
                "T1:出现问题可以点击“重置一切”按钮\n" +
                "T2:点击“<”按钮可以取消导入右侧选中的进程\n" +
                "T3:想要连带杀掉本应用的进程，在左侧搜索‘FishTool’后导入右侧，切记本应用应作为最后一个应用导入，否则会先被杀掉导致后续进程无法被杀掉。\n" +
                "T4:最好以管理员权限运行本程序";
            MessageBox.Show(helpText);
        }
        public double buttonSetHotKeyHight = 67;
        public Boolean isSettingHotKey = false;
        private void buttonSetHotKey_Click(object sender, RoutedEventArgs e)
        {
            SettingPage settingPage = new SettingPage();
            settingPage.Show();
        }
    }
}
