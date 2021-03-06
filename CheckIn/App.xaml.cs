﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CheckIn
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

        }
        private static SortedSet<Student> stus = new SortedSet<Student>();
        static PageCheck pageCheck;
        static PageAbout pageAbout;
        static PageAdmin pageAdmin;
        static PageOption pageOption;
        static int checkDayOfWeek = (int)DateTime.Now.DayOfWeek;
        public static string TimeStamp()
        {
            var t = DateTime.Now;
            return string.Format("{0},{1},{2},{3},{4}", t.Month, t.Day, t.Hour, t.Minute, t.Second);
        }
        public static PageCheck PageCheck { get => pageCheck; set => pageCheck = value; }
        public static PageAbout PageAbout { get => pageAbout; set => pageAbout = value; }
        public static PageAdmin PageAdmin { get => pageAdmin; set => pageAdmin = value; }
        public static PageOption PageOption { get => pageOption; set => pageOption = value; }
        public async static void SaveStudentsAsync()
        {
            XDocument xDoc = new XDocument(
                   new XElement(
                    "students"
)
                                             );
            foreach (var item in App.Stus)
            {
                xDoc.Element("students").Add(new XElement("student", new XAttribute("id", item.Id), new XAttribute("name", item.Name), new XAttribute("column", item.Column), new XAttribute("row", item.Row)));
            }
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync("student.xml", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, xDoc.ToString());


        }
        public static string XmlFileName
        {
            get
            {
                System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("zh-CN");
                System.Globalization.Calendar calendar = cultureInfo.Calendar;

                int weekOfYear = calendar.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
#if DEBUG
                return weekOfYear.ToString() + "_DEBUG.xml";
#else
                return weekOfYear.ToString() + ".xml";
#endif

            }
        }

        private static CheckKind currentCheckKind = CheckKind.None;
        public static CheckKind CurrentCheckKind
        {
            set
            {
                currentCheckKind = value;
            }
            get
            {
                if (currentCheckKind == CheckKind.None)
                {
                    int hour = DateTime.Now.Hour;
                    foreach (int item in Enum.GetValues(typeof(CheckKind)))
                    {
                        if (hour == item)
                        {
                            return (CheckKind)item;
                        }
                    }
                    return CheckKind.None;
                }
                else
                {
                    return currentCheckKind;
                }

            }
        }

        public static int CheckDayOfWeek { get => checkDayOfWeek; set => checkDayOfWeek = value; }
        public static SortedSet<Student> Stus
        {
            get
            {
                if (stus.Count != NumStudents)
                {
                    Task.Run(async () => { await Logger.WriteAsync("未满足48人的限制"); });
                }
                return stus;
            }
            set => stus = value;
        }
        private const int NumStudents = 48;

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    rootFrame.Navigate(typeof(PageMain), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }
        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
