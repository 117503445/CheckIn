using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class PageAbout : Page
    {
        public PageAbout()
        {
            InitializeComponent();

            TxtInfo.Text = TxtVersion()+"\n"+TxtGithub();
        }
        private string TxtVersion()
        {
            string appVersion = string.Format("CheckIn {0}.{1}.{2}.{3},Copyright © 2017-2018 HT",
        Package.Current.Id.Version.Major,
        Package.Current.Id.Version.Minor,
        Package.Current.Id.Version.Build,
        Package.Current.Id.Version.Revision);

#if DEBUG
            appVersion += ",DEBUG";
#endif
            return appVersion;
        }
        private string TxtGithub() {
            return "https://github.com/117503445/CheckIn";
        }
    }
}
