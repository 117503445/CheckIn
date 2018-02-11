using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CheckIn
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PageOption : Page
    {
        public PageOption()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;
            timer.Start();
            Timer_Tick(this,new object());
            CbDayOfWeek.ItemsSource = new List<string> { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            CbDayOfWeek.SelectedIndex = (int)App.CheckDayOfWeek;
            CbCheckKind.ItemsSource = new List<string> { "早读", "晨练", "早眼", "午休", "午眼", "晚修", "晚眼", "无" };
            int i = GetIndex(Enum.GetName(typeof(CheckKind), App.CurrentCheckKind));
            CbCheckKind.SelectedIndex = i;
        }

        private void Timer_Tick(object sender, object e)
        {
            TbTime.Text = "签到时间:" + App.TimeStamp();
        }
        private int GetIndex(string kind)
        {
            int i = 0;
            //System.Diagnostics.Debug.WriteLine(kind);
            foreach (string item in Enum.GetNames(typeof(CheckKind)))
            {
                //System.Diagnostics.Debug.WriteLine(item);
                if (item == kind)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        private void CbCheckKind_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.CurrentCheckKind = GetCheckKindByIndex(CbCheckKind.SelectedIndex);
        }
        private CheckKind GetCheckKindByIndex(int i)
        {
            switch (i)
            {
                case 0: return CheckKind.MorningRead;
                case 1: return CheckKind.MorningExercise;
                case 2: return CheckKind.MorningEye;
                case 3: return CheckKind.NoonSleep;
                case 4: return CheckKind.AfternoonEye;
                case 5: return CheckKind.NightStudy;
                case 6: return CheckKind.NightEye;
                case 7: return CheckKind.None;
                default: return CheckKind.None;
            }

        }

        private void CbDayOfWeek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.CheckDayOfWeek = CbDayOfWeek.SelectedIndex;
        }
    }
}
