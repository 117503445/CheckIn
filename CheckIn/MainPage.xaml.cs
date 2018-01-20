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
using System.Xml.Linq;
using System.Threading.Tasks;
using SQLite.Net.Attributes;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.Text;
using Windows.UI.Popups;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace CheckIn
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        List<Student> stus = new List<Student>();
        private void Btn1_Click(object sender, RoutedEventArgs e)
        {
            //string s;
            //using (Stream file = await file_demonstration.OpenStreamForReadAsync())
            //{
            //    using (StreamReader read = new StreamReader(file))
            //    {
            //        s = read.ReadToEnd();
            //    }
            //}


        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            XElement xElement = XElement.Load(@"Assets\Student.xml");
            foreach (var item in xElement.Elements())
            {
                Debug.WriteLine(item);
                string name = item.Attribute("name").Value;
                int id = int.Parse(item.Attribute("id").Value);
                int row = int.Parse(item.Attribute("row").Value);
                int column = int.Parse(item.Attribute("column").Value);
                Student student = new Student(name, id, row, column, grid);
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in stus)
            {
                Debug.WriteLine(item.Id);
            }
            using (var conn = AppDatabase.GetDbConnection())
            {
                StringBuilder msg = new StringBuilder();
                var dbPerson = conn.Table<Demo>();
                msg.AppendLine($"数据库中总共 {dbPerson.Count()} 个 Person 对象。");
                foreach (var person in dbPerson)
                {
                    msg.AppendLine($"Id：{person.Id}；Name：{person.Name}");
                }

                await new MessageDialog(msg.ToString()).ShowAsync();
            }
        }
    }
    public class Student
    {
        private Button button = new Button();
        private string name = "";
        private int id = 0;
        private int row = 0;
        private int column = 0;

        public Student(string name, int id, int row, int column, Grid grid)
        {
            this.Name = name;
            this.Id = id;
            this.Row = row;
            Column = column;

            button.Width = 120;
            button.Height = 60;
            button.Name = "Btn" + id.ToString();
            button.Content = name;

            grid.Children.Add(button);
            Grid.SetRow(button, 1);
            Grid.SetColumn(button, 1);
            button.Margin = new Thickness(150 * column, 90 * row, 0, 0);
            button.HorizontalAlignment = HorizontalAlignment.Left;
            button.VerticalAlignment = VerticalAlignment.Top;

            button.Click += BtnStu_Click;
        }
        public void BtnStu_Click(object sender, RoutedEventArgs e)
        {
            Debug.Write(Name);
        }

        public Button Button { get => button; set => button = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
    }
    public class Demo {
        [PrimaryKey]// 主键。  
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
    public static class AppDatabase
    {
        /// <summary>  
        /// 数据库文件所在路径，这里使用 LocalFolder，数据库文件名叫 test.db。  
        /// </summary>  
        public readonly static string DbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "test.db");
   
        public static SQLiteConnection GetDbConnection()
        {
            Debug.WriteLine(DbPath);
       // 连接数据库，如果数据库文件不存在则创建一个空数据库。  
       var conn = new SQLiteConnection(new SQLitePlatformWinRT(), DbPath);
            // 创建 Person 模型对应的表，如果已存在，则忽略该操作。  
            conn.CreateTable<Demo>();
            return conn;
        }
    }
}
