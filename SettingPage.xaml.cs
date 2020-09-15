using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WebClick_Tool;

namespace FishTool
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Window
    {
        public SettingPage()
        {
            InitializeComponent();
            textWindowX.Text =  KeyboardHook.windowX.ToString();
            textWindowY.Text = KeyboardHook.windowY.ToString();
            textboxHotkey.Text = SetHotKey.hotkey.ToString();
            textboxHotkey_Hide.Text = SetHotKey.hotkeyHide.ToString();
        }

        private void textboxHotkey_LostFocus(object sender, RoutedEventArgs e)
        {

        }


        private void textboxHotkey_GotFocus(object sender, RoutedEventArgs e)
        {
            SetHotKey.Hook_Start(0);
            MessageBox.Show("按下一个键后关闭本弹窗");
            SetHotKey.Hook_Clear();
            textboxHotkey.Text = SetHotKey.hotkey.ToString();
            textWindowX.Focus();
        }
        private void textboxHotkeyHide_GotFocus(object sender, RoutedEventArgs e)
        {
            SetHotKey.Hook_Start(1);
            MessageBox.Show("按下一个键后关闭本弹窗");
            SetHotKey.Hook_Clear();
            textboxHotkey_Hide.Text = SetHotKey.hotkeyHide.ToString();
            textWindowY.Focus();
        }

        private void textWindowX_TextChanged(object sender, TextChangedEventArgs e)
        {
           // getXY();
        }

        private void textWindowY_TextChanged(object sender, TextChangedEventArgs e)
        {
           // getXY();
        }
        void getXY()
        {
            try
            {
                int x = int.Parse(textWindowX.Text);
                int y = int.Parse(textWindowY.Text);
                KeyboardHook.windowX = x;
                KeyboardHook.windowY = y;
            }
            catch
            {
                MessageBox.Show("请设置为整数");
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getXY();
            this.Close();
        }
    }
}
