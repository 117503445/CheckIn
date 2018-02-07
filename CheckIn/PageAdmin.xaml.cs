using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        public  PageAdmin()
        {
            this.InitializeComponent();  
            ShowGridViewItemOfStusAsync(App.Stus);
        }
        private async Task CalculateStusScoreAsync()
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
            List<int> list = new List<int>();
            foreach (var item in i)
            {
                string[] s = item.Split(',');
                foreach (var t in s)
                {
                    list.Add(int.Parse(t));
                }
            }
            foreach (var item in list)
            {
                //System.Diagnostics.Debug.WriteLine();
                App.Stus.ElementAt(item - 1).Score -= 1;
                //System.Diagnostics.Debug.WriteLine(App.Stus.ElementAt(item - 1).Name);
                //System.Diagnostics.Debug.WriteLine(App.Stus.ElementAt(item - 1).Score);
            }
            //System.Diagnostics.Debug.WriteLine("Finished");
        }
        private async void ShowGridViewItemOfStusAsync(IEnumerable<Student> stus)
        {
            await CalculateStusScoreAsync();
            foreach (var item in stus)
            { TextBlock block = new TextBlock() { Text = item.Name + " " + item.Score.ToString(), FontSize = 20 };
                GridViewItem gvItem = new GridViewItem()
                {
                    Content =block,
                    Width=100
                };
                //System.Diagnostics.Debug.WriteLine(item.Name);
                //System.Diagnostics.Debug.WriteLine(item.Score);
                Gv.Items.Add(gvItem);
            }
            //System.Diagnostics.Debug.WriteLine("Show Finish");
        }
    }
}
