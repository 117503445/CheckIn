using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
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
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public PageAdmin()
        {
            this.InitializeComponent();

            DemoAsync();
            ShowGridViewItemOfStusAsync(App.Stus);
        }
        private async void DemoAsync()
        {
            try
            {
                var i = await GetFiles();
                CboFile.ItemsSource = i;
            }
            catch (Exception)
            {


            }

        }

        private async Task<List<string>> GetFiles()
        {
            try
            {

                var i = await storageFolder.GetFilesAsync();
                var files = from x in i where x.DisplayName.Contains("_DEBUG") select x.DisplayName;
                return files.ToList();

            }
            catch (Exception)
            {

                Debug.WriteLine("Error");
                return null;
            }



        }
        private async Task<bool> CalculateStusScoreAsync()
        {

            StorageFile file = await storageFolder.CreateFileAsync(App.XmlFileName, CreationCollisionOption.OpenIfExists);
            XElement xEle;
            try
            {
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    xEle = XElement.Load(stream);
                }
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
            catch (Exception)
            {
                return false;
            }
            return true;
            // System.Diagnostics.Debug.WriteLine(xEle);

        }

        private async void ShowGridViewItemOfStusAsync(IEnumerable<Student> stus)
        {
            if (await CalculateStusScoreAsync())
            {
                TxtWrong.Visibility = Visibility.Collapsed;
            }
            else
            {
                TxtWrong.Visibility = Visibility.Visible;
                return;
            }
            foreach (var item in stus)
            {
                TextBlock block = new TextBlock() { Text = item.Name + " " + item.Score.ToString(), FontSize = 20 };
                GridViewItem gvItem = new GridViewItem()
                {
                    Content = block,
                    Width = 100
                };
                //System.Diagnostics.Debug.WriteLine(item.Name);
                //System.Diagnostics.Debug.WriteLine(item.Score);
                Gv.Items.Add(gvItem);
            }
            //System.Diagnostics.Debug.WriteLine("Show Finish");
        }

        private void BtnChangeSeat_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in App.Stus)
            {
                if (item.Column == 1 || item.Column ==2 || item.Column == 6 || item.Column == 7)
                {
                    item.Column += 2;
                }
                else if (item.Column == 3 || item.Column == 4)
                {
                    item.Column += 3;
                }
                else if (item.Column == 8 || item.Column == 9)
                {
                    item.Column -= 7;
                }
            }
            SaveStudentsAsync();
        }
        private async void SaveStudentsAsync()
        {
            XDocument xDoc = new XDocument(
                   new XElement(
                    "students"
)
                                             );
            foreach (var item in App.Stus)
            {
                xDoc.Element("students").Add(new XElement("student", new XAttribute("id", item.Id), new XAttribute("name", item.Name),new XAttribute("column",item.Column),new XAttribute("row",item.Row)));
            }
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync("student.xml", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, xDoc.ToString());


        }
    }
}
