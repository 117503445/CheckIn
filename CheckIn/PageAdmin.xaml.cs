using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
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
    public sealed partial class PageAdmin : Page
    {
        public PageAdmin()
        {
            this.InitializeComponent();
            CalculateStusScoreAsync();
            ShowGridViewItemOfStus(App.Stus);

        }
        private async void CalculateStusScoreAsync()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync(App.XmlFileName, CreationCollisionOption.OpenIfExists);
            XElement xEle;
            using (var stream = await file.OpenStreamForReadAsync())
            {
                xEle = XElement.Load(stream);
            }
           // System.Diagnostics.Debug.WriteLine(xEle);
            var i = from x in xEle.Elements() select x.Attribute("missId").Value;
            List<string> list = new List<string>();

            foreach (var item in i)
            {
                string[] s = item.Split(',');
                foreach (var t in s)
                {
                    list.Add(t);
                }
            }

//#warning cht;
            foreach (var item in list)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }
        }
        private void ShowGridViewItemOfStus(List<Student> stus)
        {
            foreach (var item in stus)
            {
                GridViewItem gvItem = new GridViewItem()
                {
                    Content = new TextBlock() { Text = item.Name + " 0", FontSize = 24 }
                };
                Gv.Items.Add(gvItem);
            }
        }
    }
}
