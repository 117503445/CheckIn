using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class PageMain : Page
    {

        public PageMain()
        {
            this.InitializeComponent();
            MyFrame.Content = App.PageCheck;

            //DispatcherTimer timer = new DispatcherTimer()
            //{
            //    Interval = TimeSpan.FromSeconds(1)
            //};
            //timer.Tick += (s, e) =>
            //{
            //    //Debug.WriteLine(MyFrame.ActualHeight);
            //    Debug.WriteLine(ActualWidth);
            //};
            //timer.Start();
        }
        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LsteCheck.IsSelected)
            {
                MyFrame.Content = null;
                MyFrame.Content = App.PageCheck;
                Debug.WriteLine(3);
            }
            else if (LsteAbout.IsSelected)
            {
                MyFrame.Content = App.PageAbout;
            }
            else if (LsteAdmin.IsSelected)
            {
                MyFrame.Content = App.PageAdmin;
            }
            else if (LsteOption.IsSelected)
            {
                MyFrame.Content = App.PageOption;
            }
            else if (HamburgerItem.IsSelected)
            {
                //MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            }

            //if (LsteCheck.IsSelected)
            //{
            //    MySplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
            //}
            //else
            //{
            //    MySplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
            //}

        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

    }
}
