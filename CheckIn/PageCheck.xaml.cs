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
using Windows.Storage;
using System.Text;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace CheckIn
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PageCheck : Page
    {
        List<Student> stus = new List<Student>();
        private CheckKind CurrentCheckKind { get { return GetCheckKind(); } }
        public PageCheck()
        {
            this.InitializeComponent();
            LoadStu();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine("path={0};", folder.Path);
        }
        /// <summary>
        ///从student.xml中加载学生数据 
        /// </summary>
        private void LoadStu()
        {
            XElement xElement = XElement.Load(@"Assets\Student.xml");
            foreach (var item in xElement.Elements())
            {
                //Debug.WriteLine(item);
                string name = item.Attribute("name").Value;
                int id = int.Parse(item.Attribute("id").Value);
                int row = int.Parse(item.Attribute("row").Value);
                int column = int.Parse(item.Attribute("column").Value);
                Student student = new Student(name, id, row, column, GridTable);
                stus.Add(student);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveLog();
        }
        private async void SaveLog()
        {
            if (CurrentCheckKind == CheckKind.None)
            {
                Debug.WriteLine("你在干什么???");
#if !DEBUG
return;
#endif

            }
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                XDocument xDoc;
                StorageFile file = await storageFolder.CreateFileAsync("log.xml", CreationCollisionOption.OpenIfExists);
                try
                {
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        xDoc = XDocument.Load(stream);
                    }
                }
                catch (Exception)
                {
                    xDoc = new XDocument();
                    XElement root = new XElement("Logs");
                    xDoc.Add(root);
                }
                string missId = "";
                foreach (var item in stus)
                {
                    //Debug.WriteLine(item.Id);
                    //Debug.WriteLine(item.Button.IsChecked);
                    if ((bool)item.Button.IsChecked)
                    {
                        missId += item.Id.ToString() + ",";
                    }
                }
                if (missId.Length != 0)
                {
                    missId = missId.Substring(0, missId.Length - 1);
                }
                DateTime t = DateTime.Now;
                xDoc.Element("Logs").Add(new XElement("Log",
                    new XAttribute("checkKind", CurrentCheckKind),
                    new XAttribute("dayOfWeek", (int)DateTime.Now.DayOfWeek),
                    new XAttribute("missId", missId),
                    new XAttribute("time", string.Format("{0},{1},{2},{3}", t.Month, t.Day, t.Hour, t.Minute, t.Second))
                    ));
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    xDoc.Save(stream);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex);
            }
        }
        private CheckKind GetCheckKind()
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
    }
    public class Student
    {
        private ToggleButton button = new ToggleButton();
        private string name = "";
        private int id = 0;
        private int row = 0;
        private int column = 0;
        private CheckType CType
        {
            get { return CType; }
            set
            {
                CType = value; switch (value)
                {
                    case CheckType.Present:
                        break;
                    case CheckType.Absent:
                        break;
                    case CheckType.Leave:
                        break;
                    default:
                        break;
                }
            }
        }
        public void BtnStu_Click(object sender, RoutedEventArgs e)
        {
            Debug.Write(Name);
        }
        public Student(string name, int id, int row, int column, Grid grid)
        {
            this.Name = name;
            this.Id = id;
            this.Row = row;
            Column = column;
            //Button.Name = "Btn" + id.ToString();
            Button.Content = name;
            grid.Children.Add(Button);
            Grid.SetRow(Button, 2 * row - 2);
            Grid.SetColumn(Button, 2 * column - 2);
            //button.Margin = new Thickness(150 * column, 90 * row, 0, 0);
            Button.HorizontalAlignment = HorizontalAlignment.Stretch;
            Button.VerticalAlignment = VerticalAlignment.Stretch;
            Button.Click += BtnStu_Click;
        }

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public ToggleButton Button { get => button; set => button = value; }
    }
    public enum CheckKind
    {
        MorningRead = 6,
        MorningExercise = 9,
        MorningEye = 10,
        NoonSleep = 12,
        AfternoonEye = 15,
        NightStudy = 17,
        NightEye = 20,
        None = 37628,
    }
    public enum CheckType
    {
        /// <summary>
        /// 在场
        /// </summary>
        Present,
        /// <summary>
        /// 缺席
        /// </summary>
        Absent,
        /// <summary>
        /// 请假
        /// </summary>
        Leave
    }
}
