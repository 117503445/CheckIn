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
            this.InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;
            timer.Start();
            CbDayOfWeek.ItemsSource = new List<string> { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            CbDayOfWeek.SelectedIndex = (int)DateTime.Now.DayOfWeek;
            CbCheckKind.ItemsSource = new List<string> { "早读", "晨练", "早眼", "午休", "午眼", "晚修", "晚眼" };
            int i= GetIndex(Enum.GetName(typeof(CheckKind), App.CurrentCheckKind));
            CbCheckKind.SelectedIndex = i;
        }

        private void Timer_Tick(object sender, object e)
        {
            TbTime.Text = "签到时间:" + App.TimeStamp();
        }
        private int GetIndex(string kind)
        {
            int i = 0;
            System.Diagnostics.Debug.WriteLine(kind);
            foreach (string item in Enum.GetNames(typeof(CheckKind)))
            {
                System.Diagnostics.Debug.WriteLine(item);
                if (item == kind)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
    }
}
