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
            InitializeComponent();
            LoadGVItem();
            LoadCboFileListAsync();

        }
        private void BtnChangeSeat_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in App.Stus)
            {
                if (item.Column == 1 || item.Column == 2 || item.Column == 6 || item.Column == 7)
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
            App.SaveStudentsAsync();
        }
        private async void CboFile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await CalculateStusScoreAsync(CboFile.SelectedValue + ".xml");
            //ShowGridViewItemOfStusAsync(App.Stus, );
        }
        /// <summary>
        /// 把App.Stus的GridViewItem加载进gridview
        /// </summary>
        private void LoadGVItem()
        {
            foreach (var item in App.Stus)
            {
                Gv.Items.Add(item.GvItem);
            }
        }
        private async void LoadCboFileListAsync()
        {
            try
            {
                filesName = await GetFiles();
                CboFile.ItemsSource = filesName;
                CboFile.SelectedIndex = CboFile.Items.Count - 1;
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync(ex);
            }

        }
        List<string> filesName = new List<string>();
        private async Task<List<string>> GetFiles()
        {
            try
            {
                var i = await storageFolder.GetFilesAsync();
#if DEBUG
                var files = (from x in i where x.DisplayName.Contains("_DEBUG") select x.DisplayName).ToList();
#else
                 var files = from x in i where x.DisplayName.Length==1 select x.DisplayName;
#endif
                files.Insert(0,"All");
                return files;
            }
            catch (Exception ex)
            {
                await Logger.WriteAsync(ex);
                return null;
            }
        }
        private async Task<bool> CalculateStusScoreAsync(string xmlFileName)
        {
            try
            {
                if (xmlFileName != "All.xml")
                {

                    List<int> list = await GetMissId(xmlFileName);
                    //重置分数
                    foreach (var item in App.Stus)
                    {
                        item.Score = 0;
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
                else
                {

                    //var files = await storageFolder.GetFileAsync();
                    foreach (var item in App.Stus)
                    {
                        item.Score = 0;
                    }
                    foreach (var file in filesName)
                    {
                        if (file=="All")
                        {
                            continue;
                        }
                        List<int> list = await GetMissId(file+".xml");
                        foreach (var item in list)
                        {
                            //System.Diagnostics.Debug.WriteLine();
                            App.Stus.ElementAt(item - 1).Score -= 1;
                            //System.Diagnostics.Debug.WriteLine(App.Stus.ElementAt(item - 1).Name);
                            //System.Diagnostics.Debug.WriteLine(App.Stus.ElementAt(item - 1).Score);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                await Logger.WriteAsync(ex);
                return false;
            }
            return true;
        }

        private async Task<List<int>> GetMissId(string xmlFileName)
        {
            StorageFile file = await storageFolder.CreateFileAsync(xmlFileName, CreationCollisionOption.OpenIfExists);
            return await GetMissId(file);
        }
        private async Task<List<int>> GetMissId(StorageFile file)
        {
            XElement xEle;
            using (var stream = await file.OpenStreamForReadAsync())
            {
                xEle = XElement.Load(stream);
            }
            var missids = from x in xEle.Elements() select x.Attribute("missId").Value;
            List<int> list = new List<int>();
            foreach (var strMissid in missids)
            {
                string[] strMissids = strMissid.Split(',');
                foreach (var t in strMissids)
                {
                    list.Add(int.Parse(t));
                }
            }
            return list;
        }
    }
}
