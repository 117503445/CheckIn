using System;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CheckIn_WPF
{
    /// <summary>
    /// WdAdmin.xaml 的交互逻辑
    /// </summary>
    public partial class WdAdmin : Window
    {
        public WdAdmin()
        {
            InitializeComponent();
            LoadCboFileList();
            TxtVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
                Console.WriteLine(item.Name + item.Column);
            }
            App.SaveStudentsAsync();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void LoadCboFileList()
        {
            try
            {
                filesName = GetFiles();
                CboStus.ItemsSource = filesName;
                CboStus.SelectedIndex = CboStus.Items.Count - 1;
            }
            catch (Exception ex)
            {
                //await Logger.WriteAsync(ex);
            }

        }
        List<string> filesName = new List<string>();
        private List<string> GetFiles()
        {
            try
            {
                Console.WriteLine(App.path_Dir_File);
                DirectoryInfo dirInfo = new DirectoryInfo(App.path_Dir_File);
                var i = dirInfo.GetFiles();
#if DEBUG
                var files = (from x in i where x.Name.Contains("_DEBUG") select x.Name).ToList();
#else
                 var files = (from x in i where x.Length<=2 select x).ToList();
#endif
                files.Insert(0, "All");
                return files;
            }
            catch (Exception ex)
            {

                //await Logger.WriteAsync(ex);
                return null;
            }
        }

        private void CboStus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XElement element;
            List<string> missIds = new List<string>();
            if ((string)CboStus.SelectedValue != "All")
            {
                element = XElement.Load(App.path_Dir_File + (string)CboStus.SelectedValue);
                missIds = (from x in element.Elements() select x.Attribute("missId").Value).ToList();
            }
            else
            {

                foreach (string item in CboStus.Items)
                {
                    if (item != "All")
                    {
                        element = XElement.Load(App.path_Dir_File + item);
                        var missId = (from x in element.Elements() select x.Attribute("missId").Value).ToList();
                        missIds = missIds.Concat(missId).ToList();
                    }
                }
            }



            var finalMissIds = GetFinalMissIds(missIds);
            foreach (var item in App.Stus)
            {
                item.Score = 0;
            }
            foreach (var item in finalMissIds)
            {
                App.Stus.ElementAt(item - 1).Score -= 1;
            }
            ShowStus();
        }
        private List<int> GetFinalMissIds(List<string> s)
        {
            List<int> t = new List<int>();
            foreach (var item in s)
            {
                var i = item.Split(',');
                foreach (var u in i)
                {
                    t.Add(Int32.Parse(u));
                }
            }
            return t;
        }
        private void ShowStus()
        {
            TxtStus.Text = "";
            int i = 0;
            foreach (var item in App.Stus)
            {
                TxtStus.Text += $"{item.Name} {item.Score} ";
                i++;
                if (i == 6)
                {
                    TxtStus.Text += "\r\n";
                    i = 0;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
