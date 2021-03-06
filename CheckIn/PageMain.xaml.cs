﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
            InitializeComponent();
            //if (App.PageCheck == null)
            //{
            //    App.PageCheck = new PageCheck();
            //}
            //MyFrame.Content = App.PageCheck;
            IconsListBox.SelectedIndex=1;

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
        private async void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Task.Delay(20);

            if (LsteCheck.IsSelected)
            {
                if (App.PageCheck == null)
                {
                    App.PageCheck = new PageCheck();
                }
                MyFrame.Content = App.PageCheck;
            }
            else if (LsteAbout.IsSelected)
            {
                if (App.PageAbout == null)
                {
                    App.PageAbout = new PageAbout();
                }
                MyFrame.Content = App.PageAbout;
            }
            else if (LsteAdmin.IsSelected)
            {
                if (App.PageAdmin == null)
                {
                    App.PageAdmin = new PageAdmin();
                }
                MyFrame.Content = App.PageAdmin;
            }
            else if (LsteOption.IsSelected)
            {
                if (App.PageOption == null)
                {
                    App.PageOption = new PageOption();
                }
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

        private void MySplitView_DragOver(object sender, DragEventArgs e)
        {
           
        }
    }
}
